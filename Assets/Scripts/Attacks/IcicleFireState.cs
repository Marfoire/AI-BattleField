using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviourMachine;

public class IcicleFireState : StateBehaviour
{
    private Rigidbody rb;

    public float fireSpeed;

    private string myTeamTag;

    float timeStartedFiring;

    public float duration;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        myTeamTag = GetComponent<Blackboard>().GetStringVar("myTeam").Value;
        timeStartedFiring = Time.fixedTime;
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.forward * (fireSpeed * Time.fixedDeltaTime);

        if(timeStartedFiring + duration < Time.fixedTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != myTeamTag && other.isTrigger == false)
        {
            if (other.tag == "BlueTeam" || other.tag == "RedTeam")
            {
                other.GetComponent<HPValueHandler>().TakeDamage();
            }
                Destroy(gameObject);
        }
    }

}


