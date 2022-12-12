using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour
{
    TextMeshProUGUI text;
    [SerializeField] float livetime = 1f;
    [SerializeField] AnimationCurve curve;
    [SerializeField] AnimationCurve colorCurve;
    float timer = 0;
    float fraction;
    Color clearColor;

    Vector2 startPos = Vector2.zero;
    Vector2 endPos;


    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        fraction = livetime / 2;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= livetime) Destroy(gameObject);
        else if (timer > fraction)
        {
            text.color = Color.Lerp(text.color, clearColor, colorCurve.Evaluate((timer - fraction) / (livetime - fraction)));
        }


        float value = curve.Evaluate(timer / livetime);

        text.rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, value);

    }

    public void Initialize(float number, Color color)
    {
        text.text = number.ToString();
        text.color = color;
        clearColor = new Color(color.r, color.g, color.b, 0);
        endPos = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(0.8f, 1.2f));
        text.rectTransform.anchoredPosition = startPos;
        text.enabled = true;
    }
}
