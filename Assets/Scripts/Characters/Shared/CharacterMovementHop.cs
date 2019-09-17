using System.Collections;
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

    private void Awake()
    {
        parentBody = transform.parent.gameObject;
        groundedHeight = transform.localPosition.y;
    }


    void Hop()
    {
        if(transform.parent.GetComponent<Blackboard>().GetBoolVar("inMotion").Value == true)
        {
            timeSpentHopping += Time.deltaTime; 
            targetVector = new Vector3(transform.localPosition.x, groundedHeight + hopHeight, transform.localPosition.z);
            hopTValue = Mathf.PingPong(Mathf.Sin(timeSpentHopping * hopRate), 1);
            transform.localPosition = Vector3.Lerp(new Vector3(transform.localPosition.x, groundedHeight, transform.localPosition.z), targetVector, hopTValue);
        }
        else if (hopTValue != 0)
        {
            timeSpentHopping = 0;
            hopTValue -= Mathf.Clamp(Mathf.Sin(Time.deltaTime * hopRate), 0,1);
            targetVector = new Vector3(transform.localPosition.x, groundedHeight + hopHeight, transform.localPosition.z);
            transform.localPosition = Vector3.Lerp(new Vector3(transform.localPosition.x, groundedHeight, transform.localPosition.z), targetVector, hopTValue);
        }
        
    }

    private void Update()
    {
        Hop();
    }


}
