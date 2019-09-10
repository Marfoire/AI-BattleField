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

    private void Awake()
    {
        blueScoreValue = 0;
        redScoreValue = 0;

        blueScoreText.GetComponent<Text>().text = blueScoreValue.ToString();
        redScoreText.GetComponent<Text>().text = redScoreValue.ToString();

        outlineParticle = transform.GetComponentInChildren<LineRenderer>();
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

    void UpdateLineRendererColour()
    {
        int redCharactersOnPoint = 0;
        int blueCharactersOnPoint = 0;
        foreach (GameObject character in charactersOnCapturePoint)
        {
            if(character.tag == "BlueTeam")
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
        outlineParticle.startColor = new Color(outlineParticle.startColor.r + redCharactersOnPoint * (20f / byte.MaxValue), outlineParticle.startColor.g, outlineParticle.startColor.b + blueCharactersOnPoint * (20f / byte.MaxValue));
    }



    private void Update()
    {
        AddToScoreValues();
    }

}
