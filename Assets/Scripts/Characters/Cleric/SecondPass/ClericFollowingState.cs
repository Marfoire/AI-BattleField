using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviourMachine;
using UnityEngine.AI;

public class ClericFollowingState : StateBehaviour
{
    private Rigidbody rb;
    private Blackboard bb;
    private NavMeshAgent agent;

    private FloatVar turnSpeed, moveSpeed;


    private ScanSightArea visionRangeObject;
    private ScanSightArea healRangeObject;

    private GameObjectVar targettedFriend;

    public BoolVar iWasJustPanicking;
    private BoolVar inMotion;


    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        bb = GetComponent<Blackboard>();
        agent = GetComponent<NavMeshAgent>();

        targettedFriend = bb.GetGameObjectVar("healTarget");

        visionRangeObject = bb.GetGameObjectVar("visionRange").Value.GetComponent<ScanSightArea>();
        healRangeObject = bb.GetGameObjectVar("healRange").Value.GetComponent<ScanSightArea>();

        iWasJustPanicking = bb.GetBoolVar("wasIPanicking");
        inMotion = bb.GetBoolVar("inMotion");

        turnSpeed = bb.GetFloatVar("turnSpeed");
        moveSpeed = bb.GetFloatVar("moveSpeed");

        Invoke("ScanForWounded", 0.8f);
    }

    public void FriendDown()
    {
        if (!targettedFriend.Value)
        {
            CancelInvoke("ScanForWounded");
            SendEvent("CheckTeammates");
        }
    }

    public void FollowFriend()
    {
        if (targettedFriend.Value != null)
        {
            agent.destination = targettedFriend.Value.transform.position - (targettedFriend.Value.transform.forward * 10);
            rb.angularVelocity = Vector3.zero;
            rb.velocity = Vector3.zero;
            inMotion.Value = true;
            agent.speed = 7;

            if (healRangeObject.targetsInRange.Contains(targettedFriend.Value))
            {

                iWasJustPanicking.Value = false;
                GetComponentInChildren<ParticleSystem>().Stop();

                if (targettedFriend.Value.GetComponent<Blackboard>().GetFloatVar("hpValue").Value != targettedFriend.Value.GetComponent<Blackboard>().GetFloatVar("hpMax").Value)
                {
                    inMotion.Value = false;
                    CancelInvoke("ScanForWounded");
                    SendEvent("HealMyFriend");
                }
            }
        }
    }

    private void ScanForWounded()
    {
        SendEvent("GetHealTarget");
    }


    void FixedUpdate()
    {
        FriendDown();
        FollowFriend();
    }
}


