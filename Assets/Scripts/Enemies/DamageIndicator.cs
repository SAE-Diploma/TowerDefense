using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageIndicator : MonoBehaviour
{
    [SerializeField] DamageNumber textPrefab;
    Camera mainCamera;
    Canvas canvas;

    private Color[] damageColors = new Color[]
    {
        Color.white,
        Color.red,
        new Color(0.25f,0.99f,0.078f),
    };

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

    public void AddDamageNumber(float damage, EffectType type)
    {
        if (textPrefab != null)
        {
            DamageNumber damageNumber = Instantiate(textPrefab, transform);
            damageNumber.Initialize(damage, GetDamageColor(type));
        }
    }

    private Color GetDamageColor(EffectType damageType)
    {
        if ((int)damageType < damageColors.Length && (int)damageType >= 0)
        {
            return damageColors[(int)damageType];
        }
        else return Color.white;
    }
}
