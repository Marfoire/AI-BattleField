using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviourMachine;

public class CharacterAttackState : StateBehaviour
{


    private Rigidbody rb;
    private Blackboard bb;

    private GameObject visionRangeObject;
    private GameObject attackRangeObject;
    public GameObject targettedEnemy;

    private float lowestSqrMagnitude;

    private float attackStartTime;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        bb = GetComponent<Blackboard>();


        visionRangeObject = bb.GetGameObjectVar("visionRange").Value;
        attackRangeObject = bb.GetGameObjectVar("attackRange").Value;

    }

    public void ChargeAtEnemy()
    {
        if (targettedEnemy)
        {
            rb.angularVelocity = Vector3.zero;
            rb.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, (targettedEnemy.GetComponent<Rigidbody>().position - rb.position).normalized, bb.GetFloatVar("turnSpeed").Value * Time.fixedDeltaTime, 0), Vector3.up);

            if (!attackRangeObject.GetComponent<ScanSightArea>().targetsInRange.Contains(targettedEnemy))
            {
                rb.velocity = transform.forward * (bb.GetFloatVar("moveSpeed").Value * Time.fixedDeltaTime);
            }
            else
            {
                AttackTarget();
            }
        }
    }

    public void IsAreaClearOfEnemies()
    {
        visionRangeObject.GetComponent<ScanSightArea>().CleanNullCharactersFromTargetList();
        attackRangeObject.GetComponent<ScanSightArea>().CleanNullCharactersFromTargetList();
        if (visionRangeObject.GetComponent<ScanSightArea>().targetsInRange.Count == 0)
        {
            if (bb.GetBoolVar("atObjective"))
            {
                rb.angularVelocity = Vector3.zero;
                rb.velocity = Vector3.zero;
                SendEvent("ObjectiveIsClear");
            }
            else
            {
                rb.angularVelocity = Vector3.zero;
                rb.velocity = Vector3.zero;
                SendEvent("MoveToPoint");
            }
        }
    }

    public void TargetTheClosestEnemy()
    {
        visionRangeObject.GetComponent<ScanSightArea>().CleanNullCharactersFromTargetList();
        attackRangeObject.GetComponent<ScanSightArea>().CleanNullCharactersFromTargetList();

        if (targettedEnemy == null)
        {
            lowestSqrMagnitude = 10000000000;
        }

        foreach (GameObject potentialTarget in visionRangeObject.GetComponent<ScanSightArea>().targetsInRange)
        {
            if(lowestSqrMagnitude > Vector3.SqrMagnitude(rb.position - potentialTarget.GetComponent<Rigidbody>().position))
            {
                lowestSqrMagnitude = Vector3.SqrMagnitude(rb.position - potentialTarget.GetComponent<Rigidbody>().position);
                targettedEnemy = potentialTarget;
            }            
        }
    }

    public void AttackTarget()
    {
        rb.angularVelocity = Vector3.zero;
        rb.velocity = Vector3.zero;
        if (attackStartTime + bb.GetFloatVar("attackSpeedInSeconds") < Time.fixedTime)
        {
            attackStartTime = Time.fixedTime;
            targettedEnemy.GetComponent<HPValueHandler>().TakeDamage();
        }
    }


    void FixedUpdate()
    {
        IsAreaClearOfEnemies();
        TargetTheClosestEnemy();
        ChargeAtEnemy();        
    }
}


