using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;

public class Test : MonoBehaviour
{
    public Animator Animator;

    public AnimationClip clip;

    public AnimationClip Clip1;
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Animator.Play("attack");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Animator.Play("defense");
        }
    }
}
