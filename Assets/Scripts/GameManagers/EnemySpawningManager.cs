using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawningManager : MonoBehaviour
{
    [Header("Setup")]
    public Transform player;
    public Transform cam;

    [Header("Turret Spawning Settings")]
    public GameObject turretEnemy;
    
    [Min(0)]
    public Vector2Int TurretSpawnRange = new Vector2Int(12, 16);
    public float minTurretSpawnDistance = 40f;
    public float turretSpawnTimeInterval = 10f;
    
    [HideInInspector]
    public int currentTurretsSpawned;
    
    private readonly List<SpawnerData> _spawnerData = new List<SpawnerData>();
    
    private int _maxTurretsSpawned;
    private float _currentTurretTime;

    [Header("Laser Spawning Settings")] 
    public GameObject laserGenerator;
    public int maxGeneratorsSpawned = 4;
    public float generatorSpawnTimeInterval = 120f;
    
    [HideInInspector]
    public int currGeneratorsSpawned;
    [HideInInspector] 
    public int currGeneratorsOn;
    
    private float _currentGeneratorTime;
    
    public GameObject laser;
    public float laserSpawnTimeInterval = 20f;
    public float spawnDistance = 20f;
    
    private float _currentLaserTime;

    private void Start()
    {
        GetSpawnerData();
        
        _maxTurretsSpawned = Random.Range(TurretSpawnRange.x, TurretSpawnRange.y);

        while (currentTurretsSpawned < _maxTurretsSpawned)
        {
            SpawnTurret();
        }
    }

    private void Update()
    {
        if (PlayerStats.IsDead) return;
        CurrGeneratorsOn();
        
        _currentTurretTime += Time.deltaTime;
        
        if (_currentTurretTime >= turretSpawnTimeInterval)
        {
            SpawnTurret();
            _currentTurretTime = 0;
        }
        

        if (currGeneratorsOn <= 0)
        {
            _currentGeneratorTime += Time.deltaTime;

            if (_currentGeneratorTime >= generatorSpawnTimeInterval)
            {
                SpawnGenerator();
                _currentGeneratorTime = 0;
            }
        }
        else
        {
            _currentGeneratorTime = 0;
        }
        

        if (currGeneratorsOn > 0)
        {
            _currentLaserTime += Time.deltaTime;
            
            if (!(_currentLaserTime >= laserSpawnTimeInterval)) return;
            SpawnLaser();
            _currentLaserTime = 0;
        }
    }

    private void GetSpawnerData()
    {
        var spawners = GameObject.FindGameObjectsWithTag("EnemySpawner").ToList();

        //Save all Spawner Data in List
        foreach (var spawner in spawners)
        {
            var spawnPos = spawner.transform.position;
            var data = new SpawnerData()
            {
                Pos = spawnPos,
                Occupied = false
            };
            Destroy(spawner.gameObject); 
            _spawnerData.Add(data);
        }
    }

    private void SpawnTurret()
    {
        if (currentTurretsSpawned < _maxTurretsSpawned)
        {
            var spawner = _spawnerData[Random.Range(0, _spawnerData.Count)];

            if (!spawner.Occupied && Vector3.Distance(spawner.Pos, player.position) > minTurretSpawnDistance)
            {
                Instantiate(turretEnemy, spawner.Pos, Quaternion.identity);
                spawner.Occupied = true;
            }
            currentTurretsSpawned++;
        }
    }

    public void FreeSpawner(RaycastHit hit)
    {
        foreach (var t in _spawnerData.Where(t => t.Pos == hit.transform.position))
        {
            t.Occupied = false;
            break;
        }
        currentTurretsSpawned--;
    }

    private void SpawnGenerator()
    {
        while (currGeneratorsSpawned < maxGeneratorsSpawned)
        {
            var spawner = _spawnerData[Random.Range(0, _spawnerData.Count)];

            if (spawner.Occupied) continue;
            Instantiate(laserGenerator, spawner.Pos, Quaternion.identity);
            spawner.Occupied = true;
                
            currGeneratorsSpawned++;
        }
    }

    //Count the number of generators that are turned on.
    private void CurrGeneratorsOn()
    {
        var generators = GameObject.FindGameObjectsWithTag("Generator");

        currGeneratorsOn = generators.Count(generator => generator.GetComponent<LaserGenerator>().isOn);
    }

    private void SpawnLaser()
    {
        var forward = cam.forward;
        var pos = player.position + forward * spawnDistance;
        var rot = Quaternion.LookRotation(-forward);
        
        Instantiate(laser, pos, rot);
    }

    //Store data for every Spawner
    private class SpawnerData
    {
        public Vector3 Pos { get; set; }
        public bool Occupied { get; set; }
    }
}
