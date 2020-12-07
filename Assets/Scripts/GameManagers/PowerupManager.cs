using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    [Header("Powerup Settings")]
    public GameObject[] powerups;
    public float spawnInterval = 30f;
    
    private readonly List<SpawnerData> _spawnerData = new List<SpawnerData>();

    private float _currentTime;

    private void Start()
    {
        GetData();
    }

    private void Update()
    {
        _currentTime += Time.deltaTime;
        
        if (_currentTime >= spawnInterval)
        {
            SpawnPowerup();
            _currentTime = 0;
        }
    }

    private void GetData()
    {
        var spawners = GameObject.FindGameObjectsWithTag("PowerupSpawner");

        //Save all Spawner Data in List
        foreach (var spawner in spawners)
        {
            var spawnPos = spawner.transform.position;
            var data = new SpawnerData()
            {
                Pos = spawnPos,
                Occupied = false
            };
            
            //Destroy the Game Objects associated with the Spawner Data to clean up
            Destroy(spawner.gameObject); 
            _spawnerData.Add(data);
        }
    }

    //Spawn a random Powerup at a random Unoccupied Position.
    public void SpawnPowerup()
    {
        var spawner = _spawnerData[Random.Range(0, _spawnerData.Count)];
        
        if (!spawner.Occupied)
        {
            var powerup = powerups[Random.Range(0, powerups.Length)];
            
            Instantiate(powerup, spawner.Pos, Quaternion.identity);
            spawner.Occupied = true;
        }
        print("spawned powerup!");
    }
    
    private class SpawnerData
    {
        public Vector3 Pos { get; set; }
        public bool Occupied { get; set; }
    }
}
