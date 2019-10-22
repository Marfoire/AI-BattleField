using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviourMachine;

public class PlayerCastBeam : StateBehaviour
{
    //shortform component getting for rigidbody and blackboard
    private Rigidbody rb;
    private Blackboard bb;

    public GameObject beamPrefab;

    private FloatVar coolDownTime;

    // Called when the state is enabled
    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        bb = GetComponent<Blackboard>();

        coolDownTime = bb.GetFloatVar("castCooldown");
        coolDownTime.Value = 5.2f;

        CastBeam();
    }

    public void CastBeam()
    {
        SpawnBeam();
        SendEvent("Casted");
    }

    public void SpawnBeam()
    {
        GameObject spell = Instantiate(beamPrefab, transform.position + (transform.forward * 3), gameObject.transform.rotation);
        spell.GetComponentInChildren<BeamBehaviour>().GiveMeMyTagToIgnore(gameObject.tag);
    }
}


