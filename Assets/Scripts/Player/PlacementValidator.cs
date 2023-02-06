using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementValidator : MonoBehaviour
{
    [SerializeField] private bool canPlace = true;
    public bool CanPlace => canPlace;

    // Stop calculation when needed
    [SerializeField] private bool isActive = false;
    public bool IsActive { get { return isActive; } set { isActive = value; } }

    // Radius depends on the object to place
    [SerializeField] private float radius = 1f;
    public float Radius { get { return radius; } set { radius = value; } }

    [Header("Colors")]
    [SerializeField] Color valid = Color.green;
    [SerializeField] Color invalid = Color.red;


    Collider[] colliders;

    Material blueprintMaterial;

    private void Awake()
    {
        Renderer renderer = GetComponent<Renderer>();
        blueprintMaterial = renderer.material;
    }

    private void FixedUpdate()
    {
        if (isActive)
        {
            colliders = Physics.OverlapSphere(transform.position, radius);
            bool hasGround = false;
            bool placeable = true;
            if (colliders.Length == 0)
            {
                canPlace = false;
                return;
            }
            foreach (Collider collider in colliders)
            {
                if (collider.tag == "Ground")
                {
                    hasGround = true;
                    continue;
                }
                else
                {
                    placeable = false;
                }
            }
            canPlace = hasGround && placeable;

            if (canPlace) blueprintMaterial.color = valid; 
            else blueprintMaterial.color = invalid;
        }
    }
}
