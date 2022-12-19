using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    [SerializeField] Transform otherGear;
    [SerializeField] float ratio = 2f;
    [SerializeField] float offset = 0f;

    Quaternion startRotation;

    // Start is called before the first frame update
    void Start()
    {
        startRotation = transform.rotation;   
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(new Vector3(startRotation.eulerAngles.x, -otherGear.rotation.eulerAngles.y * ratio + offset, startRotation.eulerAngles.z));

    }
}
