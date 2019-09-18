using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourMachine;

public class ScanSightArea : MonoBehaviour
{

    /// <summary>
    /// String that represents the tag that I'll filter for when scanning for targets
    /// </summary>
    public string tagToScanFor;

    /// <summary>
    /// List of gameObjects that fulfill my targetting criteria
    /// </summary>
    public List<GameObject> targetsInRange = new List<GameObject>();

    private void Start()
    {
        //if the character this scanner is attached to is a cleric
        if(transform.parent.GetComponent<Blackboard>().GetStringVar("characterClass").Value == "Cleric")
        {
            //i'm going to scan for characters on my team instead of enemies
            tagToScanFor = transform.parent.tag;
        }
        else if (transform.parent.tag == "BlueTeam")//if i'm not a cleric and my team is blue
        {
            //lets scan for those reds, I don't like them
            tagToScanFor = "RedTeam";
        }
        else //if i'm not on blue team, so if i'm on red team
        {
            //lets scan for those blues, they're stinky
            tagToScanFor = "BlueTeam";
        }        
    }

    //add targets that are now in range
    private void OnTriggerEnter(Collider other)
    {
        //if the collider has the tag we're scanning for and that object isn't in the list already for whatever reason
        if(other.tag == tagToScanFor && !targetsInRange.Contains(other.gameObject))
        {
            //add to the list
            targetsInRange.Add(other.gameObject);
        }
    }

    //remove targets that are no longer in range
    private void OnTriggerExit(Collider other)
    {
        //if the list has the departing collider object
        if (targetsInRange.Contains(other.gameObject))
        {
            //remove it from the list
            targetsInRange.Remove(other.gameObject);
        }
    }

    /// <summary>
    /// Removes all instances of null from the list
    /// </summary>
    public void CleanNullCharactersFromTargetList()
    {
        //something something predicate finds if queried character value is null
        targetsInRange.RemoveAll(character => character == null);
    }

}
