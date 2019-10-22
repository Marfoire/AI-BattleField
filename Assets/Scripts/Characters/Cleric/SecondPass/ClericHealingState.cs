using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviourMachine;
using UnityEngine.AI;

public class ClericHealingState : StateBehaviour
{
    private Rigidbody rb;
    private Blackboard bb;
    private NavMeshAgent agent;

    private FloatVar turnSpeed, moveSpeed;
    private FloatVar healTimeInSeconds;

    private float lastHealStart;

    private GameObject visionRangeObject;
    private GameObject healRangeObject;

    private GameObjectVar targettedFriend;

    private float lowestHPRatio;

    public BoolVar iWasJustPanicking;
    private BoolVar inMotion;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        bb = GetComponent<Blackboard>();
        agent = GetComponent<NavMeshAgent>();

        visionRangeObject = bb.GetGameObjectVar("visionRange").Value;
        healRangeObject = bb.GetGameObjectVar("healRange").Value;

        iWasJustPanicking = bb.GetBoolVar("wasIPanicking");

        inMotion = bb.GetBoolVar("inMotion");
        inMotion.Value = false;

        turnSpeed = bb.GetFloatVar("turnSpeed");
        moveSpeed = bb.GetFloatVar("moveSpeed");

        healTimeInSeconds = bb.GetFloatVar("healSpeedInSeconds");

        targettedFriend = bb.GetGameObjectVar("healTarget");

        agent.destination = transform.position;
    }

    public void SearchForNewTarget()
    {
        if (!targettedFriend.Value || targettedFriend.Value.GetComponent<Blackboard>().GetFloatVar("hpValue").Value == targettedFriend.Value.GetComponent<Blackboard>().GetFloatVar("hpMax").Value)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;           
            SendEvent("CheckTeammates");
        }
    }
    

    public void HealMyFriend()
    {
        if (targettedFriend.Value != null)
        {
            rb.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, (targettedFriend.Value.GetComponent<Rigidbody>().position - rb.position).normalized, turnSpeed.Value * Time.fixedDeltaTime, 0), Vector3.up);
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;


            if (lastHealStart + healTimeInSeconds.Value < Time.fixedTime)
            {
                lastHealStart = Time.fixedTime;
                targettedFriend.Value.GetComponent<HPValueHandler>().HealHp();
                CheckForWounded();
            }
        }
    }

    public void CheckForWounded()
    {
        SendEvent("GetHealTarget");
    }


    void FixedUpdate()
    {
        SearchForNewTarget();
        HealMyFriend();
    }
}


