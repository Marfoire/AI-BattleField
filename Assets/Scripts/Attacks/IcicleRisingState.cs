using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviourMachine;

public class IcicleRisingState : StateBehaviour
{
    public float maxHeight;

    private float startHeight;

    private Rigidbody rb;

    public float riseSpeed;

    private GameObjectVar myCaster;

    private float spawnTime;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        myCaster = GetComponent<Blackboard>().GetGameObjectVar("caster");
        startHeight = rb.position.y;
        spawnTime = Time.fixedTime;
    }

    private void FixedUpdate()
    {
        if (myCaster.Value)
        {
            if (rb.position.y >= startHeight + maxHeight)
            {
                SendEvent("TakeAim");
            }
            else
            {
                rb.velocity = myCaster.Value.GetComponent<Rigidbody>().velocity;
                rb.position += Vector3.up * riseSpeed * Time.fixedDeltaTime;
            }
        }
        else if (Time.fixedTime > spawnTime + 0.5f)
        {
            SendEvent("Fire");
            rb.velocity = Vector3.zero;
        }
    }

}


