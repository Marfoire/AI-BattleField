using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviourMachine;

public class CharacterDefendState : StateBehaviour
{
    private Rigidbody rb;
    private GameObject visionRangeObject;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        visionRangeObject = GetComponent<Blackboard>().GetGameObjectVar("visionRange").Value;
    }

    void StandInPlace()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    void IsAnEnemyApproaching()
    {
        visionRangeObject.GetComponent<ScanSightArea>().CleanNullCharactersFromTargetList();
        if (visionRangeObject.GetComponent<ScanSightArea>().targetsInRange.Count != 0)
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


