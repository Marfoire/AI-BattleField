using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviourMachine;

public class ClericHealState : StateBehaviour
{
    private Rigidbody rb;
    private Blackboard bb;

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

        visionRangeObject = bb.GetGameObjectVar("visionRange").Value;
        healRangeObject = bb.GetGameObjectVar("healRange").Value;

        iWasJustPanicking = bb.GetBoolVar("wasIPanicking");

        inMotion = bb.GetBoolVar("inMotion");
        inMotion.Value = false;

        turnSpeed = bb.GetFloatVar("turnSpeed");
        moveSpeed = bb.GetFloatVar("moveSpeed");

        healTimeInSeconds = bb.GetFloatVar("healSpeedInSeconds");

        targettedFriend = bb.GetGameObjectVar("healTarget");

        if (targettedFriend.Value != null)
        {
            lowestHPRatio = targettedFriend.Value.GetComponent<HPValueHandler>().myHP / targettedFriend.Value.GetComponent<HPValueHandler>().maxHP;
        }
    }

    public void IsItTimeToPanic()
    {
        if (visionRangeObject.GetComponent<ScanSightArea>().targetsInRange.Count == 0 || !visionRangeObject.GetComponent<ScanSightArea>().targetsInRange.Exists(character => character.GetComponent<Blackboard>().GetStringVar("characterClass").Value != "Cleric"))
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            inMotion.Value = true;
            SendEvent("ImAllAlone");
        }
    }

    public void FindAFriend()
    {

        if (targettedFriend.Value == null)
        {
            lowestHPRatio = 2;
        }

        foreach (GameObject potentialTarget in visionRangeObject.GetComponent<ScanSightArea>().targetsInRange)
        {
            HPValueHandler hpScript = potentialTarget.GetComponent<HPValueHandler>();
            if (hpScript.myHP / hpScript.maxHP < lowestHPRatio)
            {
                lowestHPRatio = hpScript.myHP / hpScript.maxHP;
                targettedFriend.Value = potentialTarget;
            }
        }

        if (targettedFriend.Value != null)
        {
            if (!healRangeObject.GetComponent<ScanSightArea>().targetsInRange.Contains(targettedFriend.Value) && targettedFriend.Value.GetComponent<Blackboard>().GetStringVar("characterClass").Value != "Cleric")
            {
                SendEvent("EveryoneIsHealthy");
            }
        }

    }

    public void HealMyFriend()
    {
        if (targettedFriend.Value != null && lowestHPRatio < 1)
        {
            rb.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, (targettedFriend.Value.GetComponent<Rigidbody>().position - rb.position).normalized, turnSpeed.Value * Time.fixedDeltaTime, 0), Vector3.up);
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;


            if (lastHealStart + healTimeInSeconds.Value < Time.fixedTime )
            {
                lastHealStart = Time.fixedTime;
                targettedFriend.Value.GetComponent<HPValueHandler>().HealHp();
                
            }
            lowestHPRatio = targettedFriend.Value.GetComponent<HPValueHandler>().myHP / targettedFriend.Value.GetComponent<HPValueHandler>().maxHP;
        }
    }

    public void IsEveryoneHealthy()
    {
        foreach (GameObject potentialTarget in healRangeObject.GetComponent<ScanSightArea>().targetsInRange)
        {
            HPValueHandler hpScript = potentialTarget.GetComponent<HPValueHandler>();
            if (hpScript.myHP / hpScript.maxHP < 1)
            {
                return;
            }
        }
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        SendEvent("EveryoneIsHealthy");
    }


    void FixedUpdate()
    {
        FindAFriend();
        IsItTimeToPanic();       
        HealMyFriend();
        IsEveryoneHealthy();
    }
}


