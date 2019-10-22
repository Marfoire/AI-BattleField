using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviourMachine;

public class ClericFindTargetState : StateBehaviour
{
    private Rigidbody rb;
    private Blackboard bb;

    private ScanSightArea visionRangeObject;
    private ScanSightArea healRangeObject;

    private GameObjectVar healTarget;

    private float lowestHPRatio;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        bb = GetComponent<Blackboard>();

        healTarget = bb.GetGameObjectVar("healTarget");

        visionRangeObject = bb.GetGameObjectVar("visionRange").Value.GetComponent<ScanSightArea>();
        healRangeObject = bb.GetGameObjectVar("healRange").Value.GetComponent<ScanSightArea>();

        lowestHPRatio = 2;
    }

    public void FindTeammateWithLowestHPRatio()
    {
        foreach (GameObject potentialTarget in visionRangeObject.targetsInRange)
        {
            if (potentialTarget)
            {
                HPValueHandler hpScript = potentialTarget.GetComponent<HPValueHandler>();
                if (hpScript.myHP.Value / hpScript.maxHP.Value < lowestHPRatio)
                {
                    lowestHPRatio = hpScript.myHP.Value / hpScript.maxHP.Value;
                    healTarget.Value = potentialTarget;
                }
            }
        }
    }

    public void MoveToNextState()
    {
        if (!healTarget.Value)
        {
            SendEvent("PanicNow");
        }
        else if(healRangeObject.targetsInRange.Contains(healTarget.Value))
        {
            SendEvent("HealMyFriend");
            GetComponentInChildren<ParticleSystem>().Stop();
        }
        else if (!healRangeObject.targetsInRange.Contains(healTarget.Value) && visionRangeObject.targetsInRange.Contains(healTarget.Value))
        {
            if (bb.GetBoolVar("wasIPanicking").Value == false)
            {
                SendEvent("FollowFriend");                
            }
            else
            {
                SendEvent("RetreatToFriend");
            }
        }
    }

    private void FixedUpdate()
    {
        FindTeammateWithLowestHPRatio();
        MoveToNextState();
    }

}


