using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviourMachine;

public class CharacterAdvanceState : StateBehaviour
{

    public Vector3 pointToTravelTo;
    private Rigidbody rb;
    private ScanSightArea visionRangeObject;
    private Blackboard bb;
    private FloatVar turnSpeed, moveSpeed;
    private BoolVar inMotion;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        bb = GetComponent<Blackboard>();

        turnSpeed = bb.GetFloatVar("turnSpeed");
        moveSpeed = bb.GetFloatVar("moveSpeed");

        visionRangeObject = bb.GetGameObjectVar("visionRange").Value.GetComponent<ScanSightArea>();

        inMotion = bb.GetBoolVar("inMotion");

        Invoke("GetAPositionToMoveTo", Time.maximumDeltaTime);
    }

   

    public void MoveToGoal()
    {
        rb.angularVelocity = Vector3.zero;
        if (rb.position != pointToTravelTo)
        {
            inMotion.Value = true;
            rb.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, (pointToTravelTo - rb.position).normalized, turnSpeed.Value * Time.fixedDeltaTime, 0), Vector3.up);
            rb.velocity = transform.forward * (moveSpeed.Value * Time.fixedDeltaTime);
        }

        if (bb.GetBoolVar("atObjective"))
        {
            if (GetComponent<Collider>().bounds.Contains(pointToTravelTo))
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                inMotion.Value = false;
                SendEvent("ArrivedAtPoint");
            }
        }

    }

    public void IsItTimeToAggro()
    {
        if (visionRangeObject.targetsInRange.Count != 0)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            SendEvent("EnemySighted");
        }
    }

    

    public void GetAPositionToMoveTo()
    {
        pointToTravelTo = CapturePointManager.GetRandomPositionOnObjective(rb.position);
    }
	

	void FixedUpdate ()
    {
        MoveToGoal();
        IsItTimeToAggro();
	}
}


