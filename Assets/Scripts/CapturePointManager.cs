using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CapturePointManager : MonoBehaviour
{
    public List<GameObject> charactersOnCapturePoint = new List<GameObject>();

    public Gradient g;
    public float blueScoreValue;
    public float redScoreValue;

    public GameObject blueScoreText;
    public GameObject redScoreText;

    public GameObject scoreOnePointParticle;
    private GameObject particleToSpawn;

    private LineRenderer outlineParticle;
    private Vector3 outlineStartPosition;

    private void Awake()
    {
        blueScoreValue = 0;
        redScoreValue = 0;

        blueScoreText.GetComponent<Text>().text = blueScoreValue.ToString();
        redScoreText.GetComponent<Text>().text = redScoreValue.ToString();

        outlineParticle = transform.GetComponentInChildren<LineRenderer>();
        outlineStartPosition = outlineParticle.gameObject.transform.position;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "BlueTeam" || other.gameObject.tag == "RedTeam")
        {
            charactersOnCapturePoint.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "BlueTeam" || other.gameObject.tag == "RedTeam")
        {
            charactersOnCapturePoint.Remove(other.gameObject);
        }
    }

    private void AddToScoreValues()
    {
        if (Time.frameCount % 60 == 0)
        {
            foreach (GameObject character in charactersOnCapturePoint)
            {
                if (character.tag == "BlueTeam")
                {                  
                    blueScoreValue++;
                    particleToSpawn = Instantiate(scoreOnePointParticle, character.transform.position, Quaternion.identity);
                    particleToSpawn.GetComponent<PointScoreParticleBehaviour>().onBlueTeam = true;
                    blueScoreText.GetComponent<Text>().text = blueScoreValue.ToString();
                }
                else
                {
                    redScoreValue++;
                    particleToSpawn = Instantiate(scoreOnePointParticle, character.transform.position, Quaternion.identity);
                    particleToSpawn.GetComponent<PointScoreParticleBehaviour>().onBlueTeam = false;
                    redScoreText.GetComponent<Text>().text = redScoreValue.ToString();
                }
            }
        }
    }

    void UpdateLineRenderer()
    {
        if (charactersOnCapturePoint.Count > 0)
        {
            int redCharactersOnPoint = 0;
            int blueCharactersOnPoint = 0;
            foreach (GameObject character in charactersOnCapturePoint)
            {
                if (character.tag == "BlueTeam")
                {
                    blueCharactersOnPoint++;
                }
                else
                {
                    redCharactersOnPoint++;
                }
            }

            float avg = 0.5f;
            avg -= redCharactersOnPoint / 20f;
            avg += blueCharactersOnPoint / 20f;
            var c = g.Evaluate(Mathf.Clamp01(avg));
            outlineParticle.startColor = new Color(c.r, c.g, c.b, outlineParticle.startColor.a);
            outlineParticle.endColor = new Color(c.r, c.g, c.b, outlineParticle.endColor.a);
        }
        else
        {
            outlineParticle.startColor = new Color(0.5f,0.5f,0.5f,outlineParticle.startColor.a);
            outlineParticle.endColor = new Color(0.5f, 0.5f, 0.5f, outlineParticle.endColor.a);
        }

        //change alpha
        outlineParticle.startColor -= new Color(0, 0, 0, 0.3f*Time.deltaTime);
        outlineParticle.endColor -= new Color(0, 0, 0, 0.3f*Time.deltaTime);


        //handle upwards movement
        if (outlineParticle.gameObject.transform.position.y > outlineStartPosition.y + 2.8f)
        {
            outlineParticle.gameObject.transform.position = outlineStartPosition;
            outlineParticle.startColor += new Color(0, 0, 0, 1);
            outlineParticle.endColor += new Color(0, 0, 0, 1);
        }

        outlineParticle.gameObject.transform.position += new Vector3(0, 1 * Time.deltaTime, 0);

    }



    private void Update()
    {
        AddToScoreValues();
        UpdateLineRenderer();
    }

}
