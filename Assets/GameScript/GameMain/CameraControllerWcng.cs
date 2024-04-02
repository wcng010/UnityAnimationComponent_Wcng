using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraControllerWcng : MonoBehaviour
{
    [SerializeField] private Transform targetFollow;
    [SerializeField] private Vector3 followDistacne;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        //transform.transform.position = targetFollow.position + followDistacne;
    }
}
