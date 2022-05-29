using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    Transform origin;
    [SerializeField] float rotationSpeed;

    [SerializeField] float bobingHeight;
    [SerializeField] float bobingSpeed;
    Vector3 originStartPosition;
    float counter = 0f;
    public int Value { get; private set; }

    void Start()
    {
        Value = 0;
        origin = transform.GetChild(0);
        originStartPosition = origin.position;

        // randomize startValues
        counter = Random.Range(0, 90);
        origin.Rotate(transform.up, Random.Range(0, 360));
    }

    void Update()
    {
        // rotation
        origin.Rotate(transform.up, rotationSpeed * Time.deltaTime);

        // bobing
        counter += bobingSpeed * Time.deltaTime;
        origin.transform.position = originStartPosition + (transform.up * Mathf.Sin(counter) * bobingHeight / 2);
    }

    /// <summary>
    /// Set the Value of the coin
    /// </summary>
    /// <param name="value">value the coin should have</param>
    public void SetValue(int value)
    {
        Value = value;
    }
}
