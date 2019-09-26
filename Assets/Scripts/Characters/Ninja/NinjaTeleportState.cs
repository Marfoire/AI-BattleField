using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviourMachine;

public class NinjaTeleportState : StateBehaviour
{
    public GameObject smokeEffect;
    private Rigidbody rb;
    private Vector3 pointToAppearAt;
    private GameObjectVar targettedEnemy;
    

    // Called when the state is enabled
    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        smokeEffect = GetComponent<Blackboard>().GetGameObjectVar("smokeParticles").Value;
        targettedEnemy = GetComponent<Blackboard>().GetGameObjectVar("targetEnemy");
        pointToAppearAt = targettedEnemy.Value.transform.position + (-targettedEnemy.Value.transform.forward * 15);
        pointToAppearAt = new Vector3(pointToAppearAt.x, transform.position.y, pointToAppearAt.y);
        Instantiate(smokeEffect, rb.position, Quaternion.identity);
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponentInChildren<MeshRenderer>().enabled = false;
        Invoke("Reappear", 3);
    }

    // Called when the state is disabled
    void OnDisable()
    {
        Debug.Log("Stopped *State*");
    }

    private void Update()
    {
        rb.angularVelocity = Vector3.zero;
        rb.velocity = Vector3.zero;
        if (targettedEnemy.Value != null)
        {
            pointToAppearAt = targettedEnemy.Value.transform.position + (-targettedEnemy.Value.transform.forward * 15);
            pointToAppearAt = new Vector3(pointToAppearAt.x, transform.position.y, pointToAppearAt.y);
        }
    }


    void Reappear()
    {
        transform.position = pointToAppearAt;
        Instantiate(smokeEffect, transform.position, Quaternion.identity);
        GetComponent<CapsuleCollider>().enabled = true;
        GetComponentInChildren<MeshRenderer>().enabled = true;
        SendEvent("EnemySighted");
    }
}


