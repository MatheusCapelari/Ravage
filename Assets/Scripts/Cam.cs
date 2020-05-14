using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public Transform textMeshTransform;
    public Transform cameras;
    private void Update()
    {
        textMeshTransform.rotation = Quaternion.LookRotation(textMeshTransform.position - cameras.position);
    }
}
