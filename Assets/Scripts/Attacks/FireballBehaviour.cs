using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballBehaviour : MonoBehaviour
{
    public string myTeamTag;

    private List<GameObject> objectsHit = new List<GameObject>();

    public void GiveMeMyTagToIgnore(string tag)
    {
        myTeamTag = tag;
        GetComponent<Collider>().enabled = true;
        Destroy(transform.parent.gameObject, 4);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != myTeamTag && (other.isTrigger == false) && !objectsHit.Contains(other.gameObject))
        {
            if (other.tag == "BlueTeam" || other.tag == "RedTeam")
            {
                other.GetComponent<HPValueHandler>().TakeDamage();
                objectsHit.Add(other.gameObject);
            }
        }
    }
}
