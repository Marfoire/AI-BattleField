using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementHop : MonoBehaviour
{
    public float hopRate;

    public float hopHeight;

    public float groundedHeight;

    private GameObject parentBody;

    public float testValue;

    private void Awake()
    {
        parentBody = transform.parent.gameObject;
        groundedHeight = transform.localPosition.y;
    }


    void Hop()
    {
        if(parentBody.GetComponent<Rigidbody>().velocity.x != 0 || parentBody.GetComponent<Rigidbody>().velocity.z != 0)
        {
            Vector3 targetVector = new Vector3(transform.localPosition.x, groundedHeight + hopHeight, transform.localPosition.z);
            transform.localPosition = Vector3.Lerp(new Vector3(transform.localPosition.x, groundedHeight, transform.localPosition.z), targetVector, Mathf.PingPong(Time.time * hopRate, 1));
        }
        else
        {
            transform.localPosition = new Vector3(transform.localPosition.x, groundedHeight, transform.localPosition.z);
        }
    }

    private void Update()
    {
        parentBody.GetComponent<Rigidbody>().velocity = new Vector3(testValue, 0, 0);
        Hop();
    }


}
