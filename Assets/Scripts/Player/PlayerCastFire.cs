using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviourMachine;

public class PlayerCastFire : StateBehaviour
{
    //shortform component getting for rigidbody and blackboard
    private Rigidbody rb;
    private Blackboard bb;

    public GameObject firePrefab;

    private FloatVar coolDownTime;

    // Called when the state is enabled
    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        bb = GetComponent<Blackboard>();

        coolDownTime = bb.GetFloatVar("castCooldown");
        coolDownTime.Value = 6.5f;

        CastFireball();
    }

    public void CastFireball()
    {
        SpawnFireball();
        SendEvent("Casted");
    }

    public void SpawnFireball()
    {
        GameObject spell = Instantiate(firePrefab, transform.position + (transform.forward * 25), Quaternion.identity);
        spell.GetComponentInChildren<FireballBehaviour>().GiveMeMyTagToIgnore(gameObject.tag);
    }
}


