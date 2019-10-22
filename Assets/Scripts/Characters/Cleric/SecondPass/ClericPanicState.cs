using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviourMachine;
using UnityEngine.AI;

public class ClericPanicState : StateBehaviour
{
    private Rigidbody rb;
    private Blackboard bb;
    private NavMeshAgent agent;

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
        agent = GetComponent<NavMeshAgent>();

        visionRangeObject = bb.GetGameObjectVar("visionRange").Value;

        maxTimeUntilRotation = bb.GetFloatVar("rotationTimeMax");
        minTimeUntilRotation = bb.GetFloatVar("rotationTimeMin");

        bb.GetBoolVar("wasIPanicking").Value = true;

        turnSpeed = bb.GetFloatVar("turnSpeed");
        moveSpeed = bb.GetFloatVar("moveSpeed");

        newRotationVector = new Vector3(Random.Range(-180, 180), transform.position.y, Random.Range(-180, 180));

        GetComponentInChildren<ParticleSystem>().Play();

        Invoke("SearchForTeammates", 2);
    }

    public void Panic()
    {
        

        if (lastRotationStartTime + timeUntilNextRotationChange < Time.fixedTime)
        {
            newRotationVector = new Vector3(Random.Range(-180, 180), transform.position.y, Random.Range(-180, 180));
            lastRotationStartTime = Time.time;
            timeUntilNextRotationChange = Random.Range(minTimeUntilRotation.Value, maxTimeUntilRotation.Value);
        }

        // agent.destination = (newRotationVector - rb.position).normalized * 3 + rb.position;
        bb.GetBoolVar("inMotion").Value = true;

        rb.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, (newRotationVector - rb.position).normalized, turnSpeed.Value * Time.fixedDeltaTime, 0), Vector3.up);
        rb.velocity = transform.forward * (moveSpeed.Value * Time.fixedDeltaTime);
    }

    public void SearchForTeammates()
    {
        SendEvent("CheckTeammates");
    }

    private void FixedUpdate()
    {
        Panic();
    }
}


