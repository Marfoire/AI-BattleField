using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourMachine;

public class ScanSightArea : MonoBehaviour
{
    private Collider visionCollider;

    public string tagToScanFor;

    public bool imACleric;

    public List<GameObject> targetsInRange = new List<GameObject>();


    private void Start()
    {
        visionCollider = GetComponent<Collider>();

        if (imACleric)
        {
            tagToScanFor = transform.parent.tag;
        }
        else if (transform.parent.tag == "BlueTeam")
        {
            tagToScanFor = "RedTeam";
        }
        else
        {
            tagToScanFor = "BlueTeam";
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == tagToScanFor && !targetsInRange.Contains(other.gameObject))
        {
            targetsInRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (targetsInRange.Contains(other.gameObject))
        {
            targetsInRange.Remove(other.gameObject);
        }
    }


    public void CleanNullCharactersFromTargetList()
    {
        targetsInRange.RemoveAll(character => character == null);
    }

}
