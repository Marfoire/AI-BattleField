﻿using UnityEngine;
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


    private ScanSightArea visionRangeObject;
    private ScanSightArea attackRangeObject;


    public GameObjectVar targettedEnemy;

    private BoolVar inMotion;

    private float lowestSqrMagnitude;

    private bool invokedTeleport;

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

        targettedEnemy = bb.GetGameObjectVar("targetEnemy");

        invokedTeleport = false;

    }

    public void ChargeAtEnemy()
    {
        if (targettedEnemy.Value)
        {
            rb.angularVelocity = Vector3.zero;
            rb.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, (targettedEnemy.Value.GetComponent<Rigidbody>().position - rb.position).normalized, turnSpeed.Value * Time.fixedDeltaTime, 0), Vector3.up);

            attackRangeObject.CleanNullCharactersFromTargetList();
            if (!attackRangeObject.targetsInRange.Contains(targettedEnemy.Value))
            {              
                inMotion.Value = true;
                rb.velocity = transform.forward * (moveSpeed.Value * Time.fixedDeltaTime);
            }
            else
            {
                AttackTarget();
            }

            if (bb.GetStringVar("characterClass").Value == "Ninja" && invokedTeleport == false && targettedEnemy.Value.GetComponent<Blackboard>().GetStringVar("characterClass").Value != "Cleric")
            {
                SquishyEnemyInRange();
            }
        }
    }

    public void IsAreaClearOfEnemies()
    {
        visionRangeObject.CleanNullCharactersFromTargetList();

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

        lowestSqrMagnitude = 10000000;

        if(bb.GetStringVar("characterClass").Value == "Ninja" && targettedEnemy.Value != null)
        {
            if (targettedEnemy.Value.GetComponent<Blackboard>().GetStringVar("characterClass").Value == "Cleric")
            {
                return;
            }
        }

        foreach (GameObject potentialTarget in visionRangeObject.targetsInRange)
        {
            if (lowestSqrMagnitude > Vector3.SqrMagnitude(rb.position - potentialTarget.GetComponent<Rigidbody>().position))
            {
                lowestSqrMagnitude = Vector3.SqrMagnitude(rb.position - potentialTarget.GetComponent<Rigidbody>().position);
                targettedEnemy.Value = potentialTarget;
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
            targettedEnemy.Value.GetComponent<HPValueHandler>().TakeDamage();
        }
    }

    public void SquishyEnemyInRange()
    {
        visionRangeObject.CleanNullCharactersFromTargetList();

        if (visionRangeObject.targetsInRange.Exists(x => x.GetComponent<Blackboard>().GetStringVar("characterClass").Value == "Cleric"))
        {
            invokedTeleport = true;
            Invoke("InitiateTeleport", Random.Range(2, 5));
        }
    }

    void InitiateTeleport()
    {
        visionRangeObject.CleanNullCharactersFromTargetList();
        if (visionRangeObject.targetsInRange.Exists(x => x.GetComponent<Blackboard>().GetStringVar("characterClass").Value == "Cleric"))
        {
            List<GameObject> clericsToPickFrom = visionRangeObject.targetsInRange.FindAll(x => x.GetComponent<Blackboard>().GetStringVar("characterClass").Value == "Cleric");
            targettedEnemy.Value = clericsToPickFrom[Random.Range(0,clericsToPickFrom.Count)];
            SendEvent("ItsSmokeBombTime");
            rb.angularVelocity = Vector3.zero;
            rb.velocity = Vector3.zero;
        }
    }


    void FixedUpdate()
    {
        IsAreaClearOfEnemies();
        TargetTheClosestEnemy();
        ChargeAtEnemy();
    }
}


