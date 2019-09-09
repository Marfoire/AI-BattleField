using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CapturePointManager : MonoBehaviour
{
    public List<GameObject> charactersOnCapturePoint = new List<GameObject>();

    public float blueScoreValue;
    public float redScoreValue;

    public GameObject blueScoreText;
    public GameObject redScoreText;

    private void Awake()
    {
        blueScoreValue = 0;
        redScoreValue = 0;

        blueScoreText.GetComponent<Text>().text = blueScoreValue.ToString();
        redScoreText.GetComponent<Text>().text = redScoreValue.ToString();
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
            foreach (GameObject c in charactersOnCapturePoint)
            {
                if (c.tag == "BlueTeam")
                {
                    blueScoreValue++;
                    blueScoreText.GetComponent<Text>().text = blueScoreValue.ToString();
                }
                else if (c.tag == "RedTeam")
                {
                    redScoreValue++;
                    redScoreText.GetComponent<Text>().text = redScoreValue.ToString();
                }
            }
        }
    }

    private void Update()
    {
        AddToScoreValues();
    }

}
