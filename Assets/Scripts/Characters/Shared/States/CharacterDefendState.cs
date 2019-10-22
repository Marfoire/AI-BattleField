using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviourMachine;
using UnityEngine.AI;

public class CharacterDefendState : StateBehaviour
{
    private Rigidbody rb;
    private ScanSightArea visionRangeObject;
    private BoolVar inMotion;
    private NavMeshAgent agent;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        visionRangeObject = GetComponent<Blackboard>().GetGameObjectVar("visionRange").Value.GetComponent<ScanSightArea>();
        inMotion = GetComponent<Blackboard>().GetBoolVar("inMotion");
    }

    void StandInPlace()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        agent.destination = transform.position;
        inMotion.Value = false;
    }

    void IsAnEnemyApproaching()
    {
        if (visionRangeObject.targetsInRange.Count != 0)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            SendEvent("EnemySighted");
        }
    }

	void FixedUpdate ()
    {
        StandInPlace();
        IsAnEnemyApproaching();
	}
}


