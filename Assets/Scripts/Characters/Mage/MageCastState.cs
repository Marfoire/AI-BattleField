using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviourMachine;

public class MageCastState : StateBehaviour
{
    //shortform component getting for rigidbody and blackboard
    private Rigidbody rb;
    private Blackboard bb;

    //blackboard vars for floats
    private FloatVar turnSpeed;//rotation speed
    private FloatVar attackSpeed;//attack speed in seconds

    private ScanSightArea attackRangeObject;

    public GameObject targettedEnemy;

    private BoolVar inMotion;

    private float attackStartTime;

    private BoolVar amIAtObjective;

    public GameObject fireballPrefab;
    public GameObject iciclePrefab;

    // Called when the state is enabled
    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        bb = GetComponent<Blackboard>();

        turnSpeed = bb.GetFloatVar("turnSpeed");
        attackSpeed = bb.GetFloatVar("attackSpeedInSeconds");

        attackRangeObject = bb.GetGameObjectVar("attackRange").Value.GetComponent<ScanSightArea>();

        inMotion = bb.GetBoolVar("inMotion");
        amIAtObjective = bb.GetBoolVar("atObjective");

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

    public void TargetRandomEnemy()
    {
        targettedEnemy = attackRangeObject.targetsInRange[Random.Range(0, attackRangeObject.targetsInRange.Count)];
    }



    public void CastSpell()
    {
        rb.angularVelocity = Vector3.zero;
        rb.velocity = Vector3.zero;
        inMotion.Value = false;

        if (targettedEnemy)
        {
            rb.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, (targettedEnemy.GetComponent<Rigidbody>().position - rb.position).normalized, turnSpeed.Value * Time.fixedDeltaTime, 0), Vector3.up);

            if (attackStartTime + attackSpeed.Value < Time.fixedTime)
            {
                attackStartTime = Time.fixedTime;
                ConjureRandomSpell();
            }
        }
    }

    public void ConjureRandomSpell()
    {
      /* if(Random.Range(0,1) == 0)
        {
            GameObject spell = Instantiate(fireballPrefab, rb.position + Vector3.up * 3, gameObject.transform.rotation);
        }
        else
        {*/
            GameObject spell = Instantiate(iciclePrefab, rb.position + Vector3.up * 2, gameObject.transform.rotation);
            spell.GetComponent<Blackboard>().GetGameObjectVar("target").Value = targettedEnemy;
            spell.GetComponent<Blackboard>().GetStringVar("myTeam").Value = tag;
       // }
    }

    void FixedUpdate()
    {
        IsAreaClearOfEnemies();

        if (!targettedEnemy && attackRangeObject.targetsInRange.Count > 0)
        {
            TargetRandomEnemy();
        }
    }
}


