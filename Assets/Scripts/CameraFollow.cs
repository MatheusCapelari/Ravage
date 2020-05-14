using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;
    //public PlayerManager player;

    private Vector3 _cameraOffset;

    [Range(0.01f, 1.0f)]
    public float smoothFactor = 0.2f;
    Quaternion rotation;



    void Start()
    {
        _cameraOffset = transform.position - playerTransform.transform.position;
        rotation = transform.rotation;
    }
    void LateUpdate()
    {
        Vector3 newPos = playerTransform.position + _cameraOffset;

        transform.position = Vector3.Slerp(transform.position, newPos, smoothFactor);
        transform.rotation = rotation;

    }
}
