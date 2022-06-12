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
    Player_Movement movement;

    [Header("Clamping")]
    [SerializeField, Range(-90f, 90f)] float minimumY;
    [SerializeField, Range(-90f, 90f)] float maximumY;


    void Start()
    {
        player = transform.parent;
        movement = player.GetComponent<Player_Movement>();
        originalCamRotation = transform.localRotation;
        originalPlayerRotation = player.localRotation;
    }

    void Update()
    {

        // Mouselook logic
        if (movement.CanMove)
        {
            rotationX += Input.GetAxis("Mouse X") * sensitivityX;
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationX = CapRotation(rotationX);
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
        }
        Quaternion quaternionX = Quaternion.AngleAxis(rotationX, Vector3.up);
        Quaternion quaternionY = Quaternion.AngleAxis(rotationY, -Vector3.right);
        player.localRotation = originalPlayerRotation * quaternionX;
        transform.localRotation = originalCamRotation * quaternionY;
    }

    private float CapRotation(float rotation)
    {
        if (rotation > 360) return rotation - 360;
        else if (rotation < -360) return rotation + 360;
        else return rotation;
    }


}
