using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviourMachine;

public class PlayerMovementState : StateBehaviour
{
    public Vector3 inputVector;
    private Rigidbody rb;
    private ScanSightArea attackRangeObject;
    private Blackboard bb;
    private FloatVar turnSpeed, moveSpeed;
    private BoolVar inMotion;

    private FloatVar coolDown;
    private float coolDownStartTime;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        bb = GetComponent<Blackboard>();

        turnSpeed = bb.GetFloatVar("turnSpeed");
        moveSpeed = bb.GetFloatVar("moveSpeed");

        attackRangeObject = bb.GetGameObjectVar("attackRange").Value.GetComponent<ScanSightArea>();

        inMotion = bb.GetBoolVar("inMotion");

        coolDownStartTime = Time.fixedTime;

        coolDown = bb.GetFloatVar("castCooldown");
    }

    private void OnGUI()
    {
        inputVector = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (coolDown.Value + coolDownStartTime < Time.time)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                GoToIceCast();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                GoToFireCast();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                GoToBeamCast();
            }
        }
    }

    private void GoToIceCast()
    {
        SendEvent("CastIce");
    }

    private void GoToFireCast()
    {
        SendEvent("CastFire");
    }

    private void GoToBeamCast()
    {
        SendEvent("CastBeam");
    }

    public void MovePlayer()
    {
        rb.angularVelocity = Vector3.zero;
        if (inputVector != Vector3.zero)
        {
            inMotion.Value = true;
            transform.Rotate(0, Input.GetAxisRaw("Horizontal") * turnSpeed.Value * Time.fixedDeltaTime, 0, Space.Self);
            rb.velocity = transform.forward * (moveSpeed.Value * Input.GetAxisRaw("Vertical") * Time.fixedDeltaTime);
        }
        else
        {
            inMotion.Value = false;
            rb.angularVelocity = Vector3.zero;
            rb.velocity = Vector3.zero;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }


}


