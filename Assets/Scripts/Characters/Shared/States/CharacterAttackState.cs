using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviourMachine;

public class CharacterAttackState : StateBehaviour
{


    //shortform component getting for rigidbody and blackboard
    private Rigidbody rb;
    private Blackboard bb;

    //blackboard vars for floats
    private FloatVar moveSpeed;//movement speed
    private FloatVar turnSpeed;//rotation speed
    private FloatVar attackSpeed;//attack speed in seconds

    /// <summary>
    /// kkkkkk
    /// </summary>
    private ScanSightArea visionRangeObject;
    private ScanSightArea attackRangeObject;
    public GameObject targettedEnemy;

    private BoolVar inMotion;

    private float lowestSqrMagnitude;

    

    private float attackStartTime;

    private BoolVar amIAtObjective;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        bb = GetComponent<Blackboard>();

        moveSpeed = bb.GetFloatVar("moveSpeed");
        turnSpeed = bb.GetFloatVar("turnSpeed");
        attackSpeed = bb.GetFloatVar("attackSpeedInSeconds");

        visionRangeObject = bb.GetGameObjectVar("visionRange").Value.GetComponent<ScanSightArea>();
        attackRangeObject = bb.GetGameObjectVar("attackRange").Value.GetComponent<ScanSightArea>();

        inMotion = bb.GetBoolVar("inMotion");
        amIAtObjective = bb.GetBoolVar("atObjective");

    }

    public void ChargeAtEnemy()
    {
        if (targettedEnemy)
        {
            rb.angularVelocity = Vector3.zero;
            rb.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, (targettedEnemy.GetComponent<Rigidbody>().position - rb.position).normalized, turnSpeed.Value * Time.fixedDeltaTime, 0), Vector3.up);

            if (!attackRangeObject.targetsInRange.Contains(targettedEnemy))
            {
                inMotion.Value = true;
                rb.velocity = transform.forward * (moveSpeed.Value * Time.fixedDeltaTime);
            }
            else
            {
                AttackTarget();
            }
        }
    }

    public void IsAreaClearOfEnemies()
    {
        visionRangeObject.CleanNullCharactersFromTargetList();
        attackRangeObject.CleanNullCharactersFromTargetList();

        if (visionRangeObject.targetsInRange.Count == 0)
        {
            if (amIAtObjective.Value == true)
            {
                rb.angularVelocity = Vector3.zero;
                rb.velocity = Vector3.zero;
                inMotion.Value = false;
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
        visionRangeObject.CleanNullCharactersFromTargetList();
        attackRangeObject.CleanNullCharactersFromTargetList();

        if (targettedEnemy == null)
        {
            lowestSqrMagnitude = 10000000000;
        }

        foreach (GameObject potentialTarget in visionRangeObject.targetsInRange)
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
        inMotion.Value = false;
        if (attackStartTime + attackSpeed.Value < Time.fixedTime)
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


