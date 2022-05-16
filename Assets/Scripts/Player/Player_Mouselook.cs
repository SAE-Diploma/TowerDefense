using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Mouselook : MonoBehaviour
{
    [SerializeField, Range(0, 10)] float sensitivityX;
    [SerializeField, Range(0, 10)] float sensitivityY;
    Quaternion originalCamRotation;
    Quaternion originalPlayerRotation;
    float rotationX = 0;
    float rotationY = 0;

    Transform player;

    [Header("Clamping")]
    float minimumX = -360;
    float maximumX = 360;
    [SerializeField, Range(-90f, 90f)] float minimumY;
    [SerializeField, Range(-90f, 90f)] float maximumY;


    void Start()
    {
        player = transform.parent;
        originalCamRotation = transform.localRotation;
        originalPlayerRotation = player.localRotation;
    }

    void Update()
    {
        rotationX += Input.GetAxis("Mouse X") * sensitivityX;
        rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
        rotationX = Mathf.Clamp(rotationX, minimumX, maximumX);
        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
        Quaternion quaternionX = Quaternion.AngleAxis(rotationX, Vector3.up);
        Quaternion quaternionY = Quaternion.AngleAxis(rotationY, -Vector3.right);
        transform.localRotation = originalCamRotation * quaternionY;
        player.localRotation = originalPlayerRotation * quaternionX;
    }


}
