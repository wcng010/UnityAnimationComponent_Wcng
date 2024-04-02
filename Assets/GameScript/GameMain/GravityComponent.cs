using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GravityComponent : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private CharacterController controller; 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!controller.isGrounded)
        {
            Vector3 velocity = controller.velocity;
            controller.velocity.Set(velocity.x,-5f,velocity.z);
        }
    }
}
