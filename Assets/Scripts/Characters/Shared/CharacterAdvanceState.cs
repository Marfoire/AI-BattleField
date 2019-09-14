using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviourMachine;

public class CharacterAdvanceState : StateBehaviour
{

    public Vector3 pointToTravelTo;
    public float turnSpeed;
    public float moveSpeed;

    private void Start()
    {
        CapturePointManager.ObjectiveContainsVector(GetComponent<Rigidbody>().position);
    }

    public void MoveToGoal()
    {
        Vector3.RotateTowards(transform.forward, (pointToTravelTo - transform.position).normalized, turnSpeed, 0);
        GetComponent<Rigidbody>().velocity = (transform.forward * moveSpeed) * Time.fixedDeltaTime;
    }

    // Called when the state is enabled
    void OnEnable () {
		Debug.Log("Started *State*");
	}
 
	// Called when the state is disabled
	void OnDisable () {
		Debug.Log("Stopped *State*");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}


