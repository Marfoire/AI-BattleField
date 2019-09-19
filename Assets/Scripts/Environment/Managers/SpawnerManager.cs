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
        SpawnCharacters(knights, knightPrefab, knightSpawnLimit);
        SpawnCharacters(clerics, clericPrefab, clericSpawnLimit);
        SpawnCharacters(archers, archerPrefab, archerSpawnLimit);
        SpawnCharacters(ninjas, ninjaPrefab, ninjaSpawnLimit);
    }

    public void SpawnCharacters(List<GameObject> characterList, GameObject characterPrefab, int spawnCap)
    {
        characterList.RemoveAll(character => character == null);
        while (characterList.Count < spawnCap)
            {
                Vector3 maxBounds = GetComponent<Collider>().bounds.max;
                Vector3 minBounds = GetComponent<Collider>().bounds.min;
                Vector3 spawnPosition = new Vector3(Random.Range(minBounds.x, maxBounds.x), transform.position.y, Random.Range(minBounds.z, maxBounds.z));
                GameObject characterToBeSpawned = Instantiate(characterPrefab, spawnPosition, Quaternion.identity);
                characterList.Add(characterToBeSpawned);
                characterToBeSpawned.tag = spawnerTeam;
                characterToBeSpawned.layer = teamLayer;
            }
    }


    private void Update()
    {
        if (lastSpawnTime + spawnWaveInterval < Time.time)
        {
            lastSpawnTime = Time.time;


            SpawnCharacters(knights, knightPrefab, knightSpawnLimit);
            SpawnCharacters(clerics, clericPrefab, clericSpawnLimit);
            SpawnCharacters(archers, archerPrefab, archerSpawnLimit);
            SpawnCharacters(ninjas, ninjaPrefab, ninjaSpawnLimit);
        }
    }



}
