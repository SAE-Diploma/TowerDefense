using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform MuzzleTransform { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        MuzzleTransform = transform.GetChild(0).GetComponent<Transform>();
        if (MuzzleTransform == null) MuzzleTransform = transform;
    }
}
