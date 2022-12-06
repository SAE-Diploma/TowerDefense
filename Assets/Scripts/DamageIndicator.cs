using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageIndicator : MonoBehaviour
{
    [SerializeField] DamageNumber textPrefab;
    Camera mainCamera;
    Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<Canvas>();
        mainCamera = Camera.main;
        canvas.worldCamera = mainCamera;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(mainCamera.transform.position);
    }

    public void AddDamageNumber(int damage)
    {
        if (textPrefab != null)
        {
            DamageNumber damageNumber = Instantiate(textPrefab, transform);
            damageNumber.Initialize(damage, Color.red);
        }
    }
}
