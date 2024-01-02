using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footstep : MonoBehaviour
{
    private CharacterController controller;
    private OVRPlayerController ovrPlayer;

    public AudioSource footStepWalk;
    public AudioSource footStepRun;

    public bool moving = false;
    public bool running = false;
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        ovrPlayer = GetComponent<OVRPlayerController>(); 
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (controller.isGrounded && controller.velocity.magnitude > 0.0f)
            moving = true;
        else moving = false;

        running = ovrPlayer.Running;

        if (moving)
        {
            if (running && !footStepRun.isPlaying)
            {
                footStepRun.Play();
                Debug.Log("run");
            }
            else if (!running && !footStepWalk.isPlaying)
            {
                footStepWalk.Play();
                Debug.Log("walk");
            }
        }
        else
        {
            footStepRun.Stop();
            footStepWalk.Stop();
        }

    }
}
