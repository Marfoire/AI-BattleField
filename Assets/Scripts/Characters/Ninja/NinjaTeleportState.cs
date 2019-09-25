using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviourMachine;

public class NinjaTeleportState : StateBehaviour
{
    public GameObject smokeEffect;
    private Rigidbody rb;

    // Called when the state is enabled
    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        Instantiate(smokeEffect, transform.position, Quaternion.identity);
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponentInChildren<MeshRenderer>().enabled = false;
        Invoke("Reappear", 1);
    }

    // Called when the state is disabled
    void OnDisable()
    {
        Debug.Log("Stopped *State*");
    }

    void Reappear()
    {
        

        Instantiate(smokeEffect, transform.position, Quaternion.identity);
        GetComponent<CapsuleCollider>().enabled = true;
        GetComponentInChildren<MeshRenderer>().enabled = true;
        SendEvent("EnemySighted");
    }
}


