using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviourMachine;
using UnityEngine.AI;

public class ArcherShootState : StateBehaviour
{

    //shortform component getting for rigidbody and blackboard
    private Rigidbody rb;
    private Blackboard bb;
    private NavMeshAgent agent;

    //blackboard vars for floats
    private FloatVar turnSpeed;//rotation speed
    private FloatVar attackSpeed;//attack speed in seconds


    private ScanSightArea visionRangeObject;
    private ScanSightArea attackRangeObject;


    public GameObject targettedEnemy;

    private BoolVar inMotion;

    private float lowestHP;

    private float attackStartTime;

    private BoolVar amIAtObjective;

    public GameObject arrowPrefab;

    private bool invokeScan;

    // Called when the state is enabled
    void OnEnable () {
        rb = GetComponent<Rigidbody>();
        bb = GetComponent<Blackboard>();
        agent = GetComponent<NavMeshAgent>();

        turnSpeed = bb.GetFloatVar("turnSpeed");
        attackSpeed = bb.GetFloatVar("attackSpeedInSeconds");

        visionRangeObject = bb.GetGameObjectVar("visionRange").Value.GetComponent<ScanSightArea>();
        attackRangeObject = bb.GetGameObjectVar("attackRange").Value.GetComponent<ScanSightArea>();

        inMotion = bb.GetBoolVar("inMotion");
        amIAtObjective = bb.GetBoolVar("atObjective");

        invokeScan = false;
    }

    public void IsAreaClearOfEnemies()
    {

        if (attackRangeObject.targetsInRange.Count == 0)
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

    public void TargetWeakestEnemy()
    {
        lowestHP = 10000000;

        invokeScan = false;

        foreach (GameObject potentialTarget in attackRangeObject.targetsInRange)
        {
            if (potentialTarget)
            {
                if (lowestHP > potentialTarget.GetComponent<HPValueHandler>().myHP.Value)
                {
                    lowestHP = potentialTarget.GetComponent<HPValueHandler>().myHP.Value;
                    targettedEnemy = potentialTarget;
                }
            }
        }
    }



    public void FireArrows()
    {
        rb.angularVelocity = Vector3.zero;
        rb.velocity = Vector3.zero;
        agent.destination = transform.position;
        inMotion.Value = false;

        if (targettedEnemy)
        {
            rb.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, (targettedEnemy.GetComponent<Rigidbody>().position - rb.position).normalized, turnSpeed.Value * Time.fixedDeltaTime, 0), Vector3.up);

            if (attackStartTime + attackSpeed.Value < Time.fixedTime)
            {
                attackStartTime = Time.fixedTime;
                SetUpArrow();
            }
        }        
    }

    public void SetUpArrow()
    {
        var dir = targettedEnemy.GetComponent<Rigidbody>().position + (Vector3.up) - rb.position; // get target direction
        var h = dir.y;  // get height difference
        dir.y = 0;  // retain only the horizontal direction
        var dist = dir.magnitude;  // get horizontal distance
        dir.y = dist * 1.5f;  // set elevation to 45 degrees
        dist += h;  // correct for different heights
        var vel = Mathf.Sqrt(dist * Physics.gravity.magnitude);
        GameObject arrow = Instantiate(arrowPrefab, rb.position + Vector3.up*3, Quaternion.identity);
        arrow.GetComponent<ArrowBehaviour>().GiveMeMyTagToIgnore(tag);
        arrow.GetComponent<Rigidbody>().velocity = vel * dir.normalized;
    }


    
    void FixedUpdate ()
    {
        IsAreaClearOfEnemies();

        if (!targettedEnemy)
        {
            TargetWeakestEnemy();
        }
        else if (!invokeScan)
        {
            invokeScan = true;
            Invoke("TargetWeakestEnemy", 0.5f);
        }

        FireArrows();
    }
}


