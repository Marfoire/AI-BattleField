using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointScoreParticleBehaviour : MonoBehaviour
{
    public Color blueColour;
    public Color redColour;

    public bool onBlueTeam;

    public float risingRate;

    public float fadeRate;


    void GraduallyRiseAndFade()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + risingRate * Time.fixedDeltaTime, transform.position.z);
        transform.forward = Camera.main.transform.forward;
        if (onBlueTeam)
        {           
            blueColour = new Color(blueColour.r, blueColour.g, blueColour.b, blueColour.a - fadeRate * Time.fixedDeltaTime);
            GetComponent<TextMesh>().color = blueColour;
        }
        else
        {
            redColour = new Color(redColour.r, redColour.g, redColour.b, redColour.a - fadeRate * Time.fixedDeltaTime);
            GetComponent<TextMesh>().color = redColour;
        }
        if(GetComponent<TextMesh>().color.a <= 0)
        {
            Destroy(gameObject);
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        GraduallyRiseAndFade();
    }
}
