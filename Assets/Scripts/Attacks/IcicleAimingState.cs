using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviourMachine;

public class IcicleAimingState : StateBehaviour
{

    private Rigidbody rb;

    public float turnSpeed;

    public GameObjectVar targettedEnemy;

    private GameObjectVar myCaster;

    

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        targettedEnemy = GetComponent<Blackboard>().GetGameObjectVar("target");
        myCaster = GetComponent<Blackboard>().GetGameObjectVar("caster");
    }

    private void FixedUpdate()
    {
        Aim();
    }

    public void Aim()
    {
        if (myCaster.Value)
        {
            if (targettedEnemy.Value)
            {
                if (Vector3.Angle(transform.forward, ((targettedEnemy.Value.GetComponent<Rigidbody>().position + Vector3.up) - rb.position).normalized) <= 1)
                {
                    SendEvent("Fire");
                    rb.velocity = Vector3.zero;
                }
                else
                {
                    rb.velocity = myCaster.Value.GetComponent<Rigidbody>().velocity;
                    rb.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, ((targettedEnemy.Value.GetComponent<Rigidbody>().position + Vector3.up) - rb.position).normalized, turnSpeed * Time.fixedDeltaTime, 0));
                }
            }
            else if (myCaster.Value.GetComponent<Blackboard>().GetGameObjectVar("attackRange").Value.GetComponent<ScanSightArea>().targetsInRange.Count >= 1)
            {
                var casterTargets = myCaster.Value.GetComponent<Blackboard>().GetGameObjectVar("attackRange").Value.GetComponent<ScanSightArea>().targetsInRange;
                targettedEnemy = casterTargets[Random.Range(0, casterTargets.Count)];
            }
            else
            {
                rb.velocity = myCaster.Value.GetComponent<Rigidbody>().velocity;
            }
        }
        else
        {
            SendEvent("Fire");
            rb.velocity = Vector3.zero;
        }
    }



}


