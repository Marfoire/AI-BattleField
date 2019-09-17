using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourMachine;

public class HPValueHandler : MonoBehaviour
{

    public float myHP;
    public float maxHP;
    public GameObject healParticles;
    public GameObject hurtParticles;

    void Awake()
    {
        myHP = GetComponent<Blackboard>().GetFloatVar("hpValue").Value;
        maxHP = myHP;
    }

    void UpdateHPVar()
    {
        GetComponent<Blackboard>().GetFloatVar("hpValue").Value = myHP;
    }

    public void TakeDamage()
    {
        myHP--;
        Instantiate(hurtParticles, transform);
        UpdateHPVar();
    }

    public void HealHp()
    {
        if (myHP != maxHP)
        {
            myHP++;
            Instantiate(healParticles, transform);
            UpdateHPVar();
        }
    }

    private void Update()
    {
        if (myHP <= 0)
        {
            Destroy(gameObject);
        }
    }

}
