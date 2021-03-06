﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourMachine;

public class CharacterMovementHop : MonoBehaviour
{
    public float hopRate;

    public float hopHeight;

    public float groundedHeight;

    private GameObject parentBody;

    private float hopTValue;

    Vector3 targetVector;

    private float timeSpentHopping;

    private float startingHopValue;

    private void Awake()
    {
        parentBody = transform.parent.gameObject;
        groundedHeight = transform.localPosition.y;
        startingHopValue = hopRate;
    }


    void Hop()
    {
        if(parentBody.GetComponent<Blackboard>().GetStringVar("characterClass").Value == "Cleric")
        {
            if (parentBody.GetComponent<Blackboard>().GetBoolVar("wasIPanicking").Value == true)
            {
                hopRate = startingHopValue * 2;
            }
            else
            {
                hopRate = startingHopValue;
            }
        }

        if(transform.parent.GetComponent<Blackboard>().GetBoolVar("inMotion").Value == true)
        {
            timeSpentHopping += Time.fixedDeltaTime; 
            targetVector = new Vector3(transform.localPosition.x, groundedHeight + hopHeight, transform.localPosition.z);
            hopTValue = Mathf.PingPong(Mathf.Sin(timeSpentHopping * hopRate), 1);
            transform.localPosition = Vector3.Lerp(new Vector3(transform.localPosition.x, groundedHeight, transform.localPosition.z), targetVector, hopTValue);
        }
        else if (hopTValue != 0)
        {
            timeSpentHopping = 0;
            hopTValue -= Mathf.Clamp(Mathf.Sin(Time.fixedDeltaTime * hopRate), 0,1);
            targetVector = new Vector3(transform.localPosition.x, groundedHeight + hopHeight, transform.localPosition.z);
            transform.localPosition = Vector3.Lerp(new Vector3(transform.localPosition.x, groundedHeight, transform.localPosition.z), targetVector, hopTValue);
        }
        
    }

    private void FixedUpdate()
    {
        Hop();
    }


}
