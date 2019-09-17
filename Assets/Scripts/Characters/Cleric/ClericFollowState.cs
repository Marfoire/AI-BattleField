using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviourMachine;

public class ClericFollowState : StateBehaviour
{
    private Rigidbody rb;    
    private Blackboard bb;

    private FloatVar turnSpeed, moveSpeed;


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

        targettedFriend = bb.GetGameObjectVar("healTarget");

        visionRangeObject = bb.GetGameObjectVar("visionRange").Value;
        healRangeObject = bb.GetGameObjectVar("healRange").Value;

        iWasJustPanicking = bb.GetBoolVar("wasIPanicking");
        inMotion = bb.GetBoolVar("inMotion");

        turnSpeed = bb.GetFloatVar("turnSpeed");
        moveSpeed = bb.GetFloatVar("moveSpeed");

        lowestHPRatio = 2;

    }

    public void IsItTimeToPanic()
    {
        visionRangeObject.GetComponent<ScanSightArea>().CleanNullCharactersFromTargetList();
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
        visionRangeObject.GetComponent<ScanSightArea>().CleanNullCharactersFromTargetList();

        if (targettedFriend.Value == null || !visionRangeObject.GetComponent<ScanSightArea>().targetsInRange.Contains(targettedFriend.Value))
        {
            lowestHPRatio = 2;
        }

        foreach (GameObject potentialTarget in visionRangeObject.GetComponent<ScanSightArea>().targetsInRange)
        {
            HPValueHandler hpScript = potentialTarget.GetComponent<HPValueHandler>();
            if(hpScript.myHP / hpScript.maxHP < lowestHPRatio && potentialTarget.GetComponent<Blackboard>().GetStringVar("characterClass").Value != "Cleric")
            {
                lowestHPRatio = hpScript.myHP / hpScript.maxHP;
                targettedFriend.Value = potentialTarget;
            }
        } 
        



    }

    public void FollowMyFriend()
    {
        if (targettedFriend.Value != null)
        {
            rb.angularVelocity = Vector3.zero;
            Vector3 pointToMoveTo = new Vector3(targettedFriend.Value.GetComponent<Rigidbody>().position.x, rb.position.y, targettedFriend.Value.GetComponent<Rigidbody>().position.z);
            rb.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, (pointToMoveTo - rb.position).normalized, turnSpeed.Value * Time.fixedDeltaTime, 0), Vector3.up);

            inMotion.Value = true;

            if (iWasJustPanicking.Value == true)
            {
                rb.velocity = transform.forward * (moveSpeed.Value * 1.5f * Time.fixedDeltaTime);
            }
            else
            {
                rb.velocity = transform.forward * (moveSpeed.Value * Time.fixedDeltaTime);
            }

            healRangeObject.GetComponent<ScanSightArea>().CleanNullCharactersFromTargetList();
            if (healRangeObject.GetComponent<ScanSightArea>().targetsInRange.Contains(targettedFriend.Value)) {

                iWasJustPanicking.Value = false;
                inMotion.Value = false;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                if (lowestHPRatio < 1)
                {                                  
                    SendEvent("SomeoneIsHurt");
                }
            }
        }
    }


    void FixedUpdate ()
    {       
        FindAFriend();
        IsItTimeToPanic();
        FollowMyFriend();
	}
}


