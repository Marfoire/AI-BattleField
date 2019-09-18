using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamTexturePicker : MonoBehaviour
{
    public Material blueTexture;
    public Material redTexture;

    private void Start()
    {
        if (transform.parent.tag == "BlueTeam")
        {
            GetComponent<MeshRenderer>().material = blueTexture;
        }
        else
        {
            GetComponent<MeshRenderer>().material = redTexture;
        }
    }

}
