using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{

    public float spawnWaveInterval;
    private float lastSpawnTime;
    public string spawnerTeam;
    public int teamLayer;

    public GameObject knightPrefab;
    public GameObject clericPrefab;
    public GameObject archerPrefab;
    public GameObject ninjaPrefab;

    public int knightSpawnLimit;
    public int clericSpawnLimit;
    public int archerSpawnLimit;
    public int ninjaSpawnLimit;

    public List<GameObject> knights = new List<GameObject>();
    public List<GameObject> clerics = new List<GameObject>();
    public List<GameObject> archers = new List<GameObject>();
    public List<GameObject> ninjas = new List<GameObject>();

    private void Awake()
    {
        knights.Capacity = knightSpawnLimit;
        clerics.Capacity = clericSpawnLimit;
        archers.Capacity = archerSpawnLimit;
        ninjas.Capacity = ninjaSpawnLimit;

        InitializeSpawns(knights, knightPrefab);
        InitializeSpawns(clerics, clericPrefab);
        InitializeSpawns(archers, archerPrefab);
        InitializeSpawns(ninjas, ninjaPrefab);
    }

    public void InitializeSpawns(List<GameObject> characterList, GameObject characterPrefab)
    {
        for (int i = 0; i < characterList.Capacity; i++)
        {
            Vector3 maxBounds = GetComponent<Collider>().bounds.max;
            Vector3 minBounds = GetComponent<Collider>().bounds.min;
            Vector3 spawnPosition = new Vector3(Random.Range(maxBounds.x, minBounds.x), transform.position.y, Random.Range(maxBounds.z, minBounds.z));
            GameObject characterToBeSpawned = Instantiate(characterPrefab, spawnPosition, Quaternion.identity);
            characterList.Add(characterToBeSpawned);
            characterToBeSpawned.tag = spawnerTeam;
            characterToBeSpawned.layer = teamLayer;
        }
    }




    private void SpawnCharacters(List<GameObject> characterList, GameObject characterPrefab)
    {
        for(int i = 0; i < characterList.Capacity; i++)
        {
            if(characterList[i] == null)
            {
                Vector3 maxBounds = GetComponent<Collider>().bounds.max;
                Vector3 minBounds = GetComponent<Collider>().bounds.min;
                Vector3 spawnPosition = new Vector3(Random.Range(maxBounds.x, minBounds.x), transform.position.y, Random.Range(maxBounds.z, minBounds.z));
                GameObject characterToBeSpawned = Instantiate(characterPrefab, spawnPosition, Quaternion.identity);
                characterList[i] = characterToBeSpawned;
                characterToBeSpawned.tag = spawnerTeam;
                characterToBeSpawned.layer = teamLayer;
            }
        }
    }

    private void CorrectSpawnCaps(List<GameObject> characterList, int spawnCap, GameObject characterPrefab)
    {
        if (characterList.Capacity != spawnCap)
        {
            characterList.Capacity = spawnCap;

            Vector3 maxBounds = GetComponent<Collider>().bounds.max;
            Vector3 minBounds = GetComponent<Collider>().bounds.min;
            Vector3 spawnPosition = new Vector3(Random.Range(maxBounds.x, minBounds.x), transform.position.y, Random.Range(maxBounds.x, minBounds.x));

            characterList.Add(Instantiate(characterPrefab, spawnPosition, Quaternion.identity) as GameObject);
        }
    }


    private void Update()
    {
        if(lastSpawnTime + spawnWaveInterval < Time.time)
        {
            lastSpawnTime = Time.time;

            CorrectSpawnCaps(knights, knightSpawnLimit, knightPrefab);
            CorrectSpawnCaps(clerics, clericSpawnLimit, clericPrefab);
            CorrectSpawnCaps(archers, archerSpawnLimit, archerPrefab);
            CorrectSpawnCaps(ninjas, ninjaSpawnLimit, ninjaPrefab);            

            SpawnCharacters(knights, knightPrefab);
            SpawnCharacters(clerics, clericPrefab);
            SpawnCharacters(archers, archerPrefab);
            SpawnCharacters(ninjas, ninjaPrefab);
        }
    }



}
