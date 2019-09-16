using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourMachine;

public class HPValueHandler : MonoBehaviour
{

    public float myHP;

    void Awake()
    {
        myHP = GetComponent<Blackboard>().GetFloatVar("hpValue").Value;
    }

    void UpdateHPVar()
    {
        GetComponent<Blackboard>().GetFloatVar("hpValue").Value = myHP;
    }

    public void TakeDamage()
    {
        myHP--;
        UpdateHPVar();
    }

    public void HealHp()
    {
        myHP++;
        UpdateHPVar();
    }

    private void Update()
    {
        if (myHP <= 0)
        {
            Destroy(gameObject);
        }
    }

}
