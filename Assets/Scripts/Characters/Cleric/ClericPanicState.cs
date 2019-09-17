using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviourMachine;

public class ClericPanicState : StateBehaviour
{

    private Rigidbody rb;
    private Blackboard bb;

    private GameObject visionRangeObject;

    private FloatVar turnSpeed, moveSpeed;


    private Vector3 newRotationVector;

    private float lastRotationStartTime;

    public float timeUntilNextRotationChange;

    public FloatVar maxTimeUntilRotation, minTimeUntilRotation;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        bb = GetComponent<Blackboard>();

        visionRangeObject = bb.GetGameObjectVar("visionRange").Value;

        maxTimeUntilRotation = bb.GetFloatVar("rotationTimeMax");
        minTimeUntilRotation = bb.GetFloatVar("rotationTimeMin");

        bb.GetBoolVar("wasIPanicking").Value = true;

        turnSpeed = bb.GetFloatVar("turnSpeed");
        moveSpeed = bb.GetFloatVar("moveSpeed");

        newRotationVector = new Vector3(0, Random.Range(0, 1), 0);

        GetComponentInChildren<ParticleSystem>().Play();
    }




    void Panic()
    {
        if (lastRotationStartTime + timeUntilNextRotationChange < Time.fixedTime) {
            newRotationVector = new Vector3(Random.Range(-180,180), transform.position.y, Random.Range(-180, 180));
            lastRotationStartTime = Time.time;
            timeUntilNextRotationChange = Random.Range(minTimeUntilRotation.Value, maxTimeUntilRotation.Value);            
        }

        rb.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, (newRotationVector - rb.position).normalized, turnSpeed.Value * Time.fixedDeltaTime, 0), Vector3.up);
        rb.velocity = transform.forward * (moveSpeed.Value * Time.fixedDeltaTime);
    }

    public void FindAFriend()
    {
        visionRangeObject.GetComponent<ScanSightArea>().CleanNullCharactersFromTargetList();

        if(visionRangeObject.GetComponent<ScanSightArea>().targetsInRange.Count > 0 && visionRangeObject.GetComponent<ScanSightArea>().targetsInRange.Exists(character => character.GetComponent<Blackboard>().GetStringVar("characterClass").Value != "Cleric"))
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            GetComponentInChildren<ParticleSystem>().Stop();
            SendEvent("ISeeAFriend");
        }
    }


    private void FixedUpdate()
    {
        FindAFriend();
        Panic();
    }

}


