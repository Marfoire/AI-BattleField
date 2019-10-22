using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamBehaviour : MonoBehaviour
{
    public string myTeamTag;

    private List<GameObject> objectsHit = new List<GameObject>();

    public void GiveMeMyTagToIgnore(string tag)
    {
        myTeamTag = tag;
        GetComponent<Collider>().enabled = true;
        Destroy(transform.parent.gameObject, 1);
    }

    private void FixedUpdate()
    {
        transform.parent.localScale += (Vector3.forward * 20 * Time.fixedDeltaTime);
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
