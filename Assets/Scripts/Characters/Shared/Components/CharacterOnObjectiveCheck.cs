using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourMachine;

public class CharacterOnObjectiveCheck : MonoBehaviour
{

    private void FixedUpdate()
    {
        GetComponent<Blackboard>().GetBoolVar("atObjective").Value = CapturePointManager.ObjectiveContainsVector(GetComponent<Rigidbody>().position);
    }

}
