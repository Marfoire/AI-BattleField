using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviourMachine;

public class ClericSearchState : StateBehaviour
{
    private Rigidbody rb;
    private Blackboard bb;

    private GameObject visionRangeObject;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        bb = GetComponent<Blackboard>();

        visionRangeObject = bb.GetGameObjectVar("visionRange").Value;
    }

    public void FindAFriend()
    {
        if (visionRangeObject.GetComponent<ScanSightArea>().targetsInRange.Count > 0 && visionRangeObject.GetComponent<ScanSightArea>().targetsInRange.Exists(character => character.GetComponent<Blackboard>().GetStringVar("characterClass").Value != "Cleric"))
        {
            rb.angularVelocity = Vector3.zero;
            rb.velocity = Vector3.zero;
            SendEvent("GetHealTarget");
        }
        else
        {
            SendEvent("PanicNow");
        }
    }

    private void FixedUpdate()
    {
        FindAFriend();
    }
}


