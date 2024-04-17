using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class Move : MonoBehaviour
{
    [SerializeField] private Rigidbody Rigidbody;

    private void Update()
    {
        if (Input.anyKey)
        {
            Rigidbody.velocity = new Vector3(5, 0, 0);
            Debug.Log(1);
        }
        else
        {
            Rigidbody.velocity = Vector3.zero;
        }
    }
}
