using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviourMachine;

public class PlayerCastIce : StateBehaviour
{

    //shortform component getting for rigidbody and blackboard
    private Rigidbody rb;
    private Blackboard bb;

    public GameObject iciclePrefab;

    private ScanSightArea attackRangeObject;

    private FloatVar coolDownTime;

    // Called when the state is enabled
    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        bb = GetComponent<Blackboard>();

        attackRangeObject = bb.GetGameObjectVar("attackRange").Value.GetComponent<ScanSightArea>();

        coolDownTime = bb.GetFloatVar("castCooldown");
        coolDownTime.Value = 4.5f;

        CastIcicles();
    }

    public void CastIcicles()
    {
        SpawnIcicle();
        Invoke("SpawnIcicle", 0.2f);
        Invoke("SpawnIcicle", 0.4f);
        SendEvent("Casted");
    }

    public void SpawnIcicle()
    {
        GameObject spell = Instantiate(iciclePrefab, rb.position + (Vector3.up * 4) + (Vector3.right * Random.Range(-5, 5)) + (Vector3.forward * Random.Range(-5, 5)), gameObject.transform.rotation);
        if (attackRangeObject.targetsInRange.Count > 0)
        {
            spell.GetComponent<Blackboard>().GetGameObjectVar("target").Value = attackRangeObject.targetsInRange[Random.Range(0, attackRangeObject.targetsInRange.Count)];
        }
        spell.GetComponent<Blackboard>().GetGameObjectVar("caster").Value = gameObject;
        spell.GetComponent<Blackboard>().GetStringVar("myTeam").Value = tag;
    }
}


