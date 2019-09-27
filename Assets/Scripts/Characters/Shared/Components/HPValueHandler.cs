using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourMachine;

public class HPValueHandler : MonoBehaviour
{
    /// <summary>
    /// This character's HP value
    /// </summary>
    public FloatVar myHP;

    /// <summary>
    /// This character's maximum HP value
    /// </summary>
    public FloatVar maxHP;

    /// <summary>
    /// Prefab for the heal particle effect
    /// </summary>
    public GameObject healParticles;

    /// <summary>
    /// Prefab for the hurt particle effect
    /// </summary>
    public GameObject hurtParticles;

    void Awake()
    {
        //get blackboard vars for hp and max hp
        myHP = GetComponent<Blackboard>().GetFloatVar("hpValue");
        maxHP = GetComponent<Blackboard>().GetFloatVar("hpMax");

        //stop trying to overheal the character blackboard user
        if(myHP.Value > maxHP.Value)
        {
            myHP.Value = maxHP.Value;
        }
    }

    /// <summary>
    /// Lower my hp by 1 and instantiate a hurt particle effect
    /// </summary>
    public void TakeDamage()
    {
        //decrease hp value by 1
        myHP.Value--;
        //instantiate the hurt particle as a child
        Instantiate(hurtParticles, transform);
    }

    /// <summary>
    /// Increase my hp by 1 and instantiate a healing particle effect, will not heal if at max hp
    /// </summary>
    public void HealHp()
    {
        //if i'm not at max hp
        if (myHP.Value != maxHP.Value)
        {
            //add 1 hp
            myHP.Value++;
            //instantiate the healing particle as a child
            Instantiate(healParticles, transform);
        }
    }

    private void FixedUpdate()
    {
        //temporary death check
        if (myHP.Value <= 0)
        {
            Destroy(gameObject);
        }
    }

}
