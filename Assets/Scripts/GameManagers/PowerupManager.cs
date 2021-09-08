using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    [Header("Powerup Settings")]
    public GameObject[] powerups;
    public float spawnInterval = 30f;
    
    private readonly List<SpawnerData> _spawnerData = new List<SpawnerData>();

    private float _currentTime;

    [Header("UI Settings")] 
    public PlayerStats ps;
    public GameObject fallProtectionUI;
    public GameObject secondChanceUI;
    public GameObject overchargeUI;

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

        UpdateUI();
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
                Pos = spawnPos
            };
            
            //Destroy the Game Objects associated with the Spawner Data to clean up
            Destroy(spawner.gameObject); 
            _spawnerData.Add(data);
        }
    }

    //Spawn a random Powerup at a random Unoccupied Position.
    private void SpawnPowerup()
    {
        var spawner = _spawnerData[Random.Range(0, _spawnerData.Count)];
        var powerup = powerups[Random.Range(0, powerups.Length)];

        Instantiate(powerup, spawner.Pos, Quaternion.identity);
        print("spawned powerup!");
    }

    private void UpdateUI()
    {
        fallProtectionUI.SetActive(ps.hasFallProtection);
        secondChanceUI.SetActive(ps.hasSecondChance);
        overchargeUI.SetActive(ps.hasOvercharge);
    }

    private class SpawnerData
    {
        public Vector3 Pos { get; set; }
    }
}
