using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehaviour : MonoBehaviour
{

    public string myTeamTag;

    public void GiveMeMyTagToIgnore(string tag)
    {
        myTeamTag = tag;
        GetComponent<Collider>().enabled = true;
    }

    private void FixedUpdate()
    {
        GetComponent<Rigidbody>().rotation = Quaternion.LookRotation(GetComponent<Rigidbody>().velocity.normalized);
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != myTeamTag && (other.isTrigger == false || other.tag == tag))
        {
            if(other.tag == "BlueTeam" || other.tag == "RedTeam")
            {
                other.GetComponent<HPValueHandler>().TakeDamage();
            }
            Destroy(gameObject);
        }
    }

}
