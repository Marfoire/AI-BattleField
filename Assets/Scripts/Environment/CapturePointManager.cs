using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CapturePointManager : MonoBehaviour
{
    private static CapturePointManager instance;

    public List<GameObject> charactersOnCapturePoint = new List<GameObject>();

    public Gradient controlGradient;
    public float blueScoreValue;
    public float redScoreValue;

    public GameObject blueScoreText;
    public GameObject redScoreText;

    public GameObject scoreOnePointParticle;
    private GameObject particleToSpawn;

    private LineRenderer outlineParticle;
    private Vector3 outlineStartPosition;
    private Collider pointCollider;

    public float outlineParticleSpeed;
    public float outlineParticleAlphaFadeRate;
    public float outlineParticleMaxHeight;

    private void Awake()
    {
        pointCollider = GetComponent<Collider>();

        blueScoreValue = 0;
        redScoreValue = 0;

        blueScoreText.GetComponent<Text>().text = blueScoreValue.ToString();
        redScoreText.GetComponent<Text>().text = redScoreValue.ToString();

        outlineParticle = transform.GetComponentInChildren<LineRenderer>();
        outlineStartPosition = outlineParticle.gameObject.transform.position;
    }

    private void OnEnable()
    {
        instance = this;
    }

    public static bool ObjectiveContainsVector(Vector3 position)
    {
        return instance.pointCollider.bounds.Contains(position);
    }

    public static Vector3 GetRandomPositionOnObjective(Vector3 characterVector)
    {
        Vector3 maxBounds = instance.pointCollider.bounds.max;
        Vector3 minBounds = instance.pointCollider.bounds.min;
        return new Vector3(Random.Range(maxBounds.x, minBounds.x), characterVector.y, Random.Range(maxBounds.z, minBounds.z));
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "BlueTeam" || other.gameObject.tag == "RedTeam")
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

    private void CleanNullCharactersFromObjectiveList()
    {
        charactersOnCapturePoint.RemoveAll(character => character == null);
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

            float controlAvg = 0.5f;
            controlAvg -= redCharactersOnPoint / 20f;
            controlAvg += blueCharactersOnPoint / 20f;
            var currentControlColour = controlGradient.Evaluate(Mathf.Clamp01(controlAvg));

            outlineParticle.startColor = SetColourWithoutChangingAlpha(currentControlColour, outlineParticle.startColor);
            outlineParticle.endColor = SetColourWithoutChangingAlpha(currentControlColour, outlineParticle.endColor);
        }
        else
        {
            outlineParticle.startColor = SetColourWithoutChangingAlpha(Color.white * 0.8f, outlineParticle.startColor);
            outlineParticle.endColor = SetColourWithoutChangingAlpha(Color.white * 0.8f, outlineParticle.endColor);
        }

        //gradually fade out the alpha of the lineRenderer
        outlineParticle.startColor -= new Color(0, 0, 0, outlineParticleAlphaFadeRate * Time.deltaTime);
        outlineParticle.endColor -= new Color(0, 0, 0, outlineParticleAlphaFadeRate * Time.deltaTime);


        //if the y position of the particle exceeds it's starting position plus the max height I want it to travel
        if (outlineParticle.gameObject.transform.position.y > outlineStartPosition.y + outlineParticleMaxHeight)
        {
            //set the particle back to it's starting position
            outlineParticle.gameObject.transform.position = outlineStartPosition;

            //reset it's alpha value to 1 but retain it's rgb values
            outlineParticle.startColor = SetColourWithoutChangingAlpha(outlineParticle.startColor, Color.grey);
            outlineParticle.endColor = SetColourWithoutChangingAlpha(outlineParticle.endColor, Color.grey);
        }

        //move the particle up by a given speed
        outlineParticle.gameObject.transform.position += new Vector3(0, outlineParticleSpeed * Time.deltaTime, 0);

    }

    //returns a colour that has the rgb values of the first argument but also the alpha value of the second argument
    public Color SetColourWithoutChangingAlpha(Color rgbToKeep, Color alphaToKeep)
    {
        return new Color(rgbToKeep.r, rgbToKeep.g, rgbToKeep.b, alphaToKeep.a);
    }

    private void Update()
    {
        CleanNullCharactersFromObjectiveList();
        AddToScoreValues();
        UpdateLineRenderer();
    }

}


public static class ColorExtensions
{
    public static Color AccelerateAlpha(this Color color, float accelValue)
    {
        color.a -= accelValue;
        return color;
    }



}