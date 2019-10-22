using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviourMachine;
using UnityEngine.AI;

public class CharacterAttackState : StateBehaviour
{


    //shortform component getting for rigidbody and blackboard
    private Rigidbody rb;
    private Blackboard bb;
    private NavMeshAgent agent;

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

    private bool invokeScan;


    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        bb = GetComponent<Blackboard>();
        agent = GetComponent<NavMeshAgent>();

        moveSpeed = bb.GetFloatVar("moveSpeed");
        turnSpeed = bb.GetFloatVar("turnSpeed");
        attackSpeed = bb.GetFloatVar("attackSpeedInSeconds");

        visionRangeObject = bb.GetGameObjectVar("visionRange").Value.GetComponent<ScanSightArea>();
        attackRangeObject = bb.GetGameObjectVar("attackRange").Value.GetComponent<ScanSightArea>();

        inMotion = bb.GetBoolVar("inMotion");
        amIAtObjective = bb.GetBoolVar("atObjective");

        targettedEnemy = bb.GetGameObjectVar("targetEnemy");

        invokedTeleport = false;
        invokeScan = false;
    }

    public void ChargeAtEnemy()
    {
        if (targettedEnemy.Value)
        {
            rb.angularVelocity = Vector3.zero;
            rb.velocity = Vector3.zero;

            if (!attackRangeObject.targetsInRange.Contains(targettedEnemy.Value))
            {              
                inMotion.Value = true;
                agent.destination = targettedEnemy.Value.transform.position;
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

        lowestSqrMagnitude = 10000000;

        if(bb.GetStringVar("characterClass").Value == "Ninja" && targettedEnemy.Value != null)
        {
            if (targettedEnemy.Value.GetComponent<Blackboard>().GetStringVar("characterClass").Value == "Cleric")
            {
                return;
            }
        }

        invokeScan = false;

        foreach (GameObject potentialTarget in visionRangeObject.targetsInRange)
        {
            if (potentialTarget)
            {
                if (lowestSqrMagnitude > Vector3.SqrMagnitude(rb.position - potentialTarget.GetComponent<Rigidbody>().position))
                {
                    lowestSqrMagnitude = Vector3.SqrMagnitude(rb.position - potentialTarget.GetComponent<Rigidbody>().position);
                    targettedEnemy.Value = potentialTarget;
                }
            }
        }
    }



    public void AttackTarget()
    {
        rb.angularVelocity = Vector3.zero;
        rb.velocity = Vector3.zero;
        inMotion.Value = false;
        agent.destination = transform.position;
        if (attackStartTime + attackSpeed.Value < Time.fixedTime)
        {
            attackStartTime = Time.fixedTime;
            targettedEnemy.Value.GetComponent<HPValueHandler>().TakeDamage();
        }
    }

    public void SquishyEnemyInRange()
    {

        if (visionRangeObject.targetsInRange.Exists(x => x.GetComponent<Blackboard>().GetStringVar("characterClass").Value == "Cleric"))
        {
            invokedTeleport = true;
            Invoke("InitiateTeleport", Random.Range(2, 5));
        }
    }

    void InitiateTeleport()
    {
        if (visionRangeObject.targetsInRange.Exists(x => x.GetComponent<Blackboard>().GetStringVar("characterClass").Value == "Cleric"))
        {
            List<GameObject> clericsToPickFrom = visionRangeObject.targetsInRange.FindAll(x => x.GetComponent<Blackboard>().GetStringVar("characterClass").Value == "Cleric");
            targettedEnemy.Value = clericsToPickFrom[Random.Range(0,clericsToPickFrom.Count)];          
            rb.angularVelocity = Vector3.zero;
            rb.velocity = Vector3.zero;
            SendEvent("ItsSmokeBombTime");
        }
    }


    void FixedUpdate()
    {
        IsAreaClearOfEnemies();

        if (!targettedEnemy.Value)
        {
            TargetTheClosestEnemy();
        }
        else if(!invokeScan)
        {
            invokeScan = true;
            Invoke("TargetTheClosestEnemy", 0.5f);
        }

        ChargeAtEnemy();
    }
}


