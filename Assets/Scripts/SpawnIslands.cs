using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnIslands : MonoBehaviour
{
    [Header("Island Settings")]
    public GameObject[] islandPrefabs;
    public float maxIslands = 25;
    
    [Header("Grid Settings")] 
    public Vector3 gridOrigin = Vector3.zero;
    [Space] [Min(0)] public int gridX = 5;
    [Min(0)] public int gridZ = 5;
    [Space] public float gridSpacing = 1;

    [HideInInspector]
    public bool generated;

    public Rigidbody rb;
    
    private readonly List<GridData> _gridData = new List<GridData>();
    private float _numberSpawned;

    private void Awake()
    {
        //Lock Player Until Generation is Complete
        rb.isKinematic = true;
        maxIslands = Mathf.Clamp(maxIslands, 0, gridX * gridZ);
        
        SpawnGrid();
        UpdatePlayer();
    }
    
    private void SpawnGrid()
    {
        //Create grid and save grid data
        for (var x = 0; x < gridX; x++)
        {
            for (var z = 0; z < gridZ; z++)
            {
                var spawnPos = new Vector3(x * gridSpacing, 0, z * gridSpacing) + gridOrigin;
                var data = new GridData
                {
                    Pos = spawnPos,
                    Occupied = false
                };
                _gridData.Add(data);
            }
        }

        //Spawn Islands on random cells in the grid
        while (_numberSpawned < maxIslands)
        {
            var prefabIndex = Random.Range(0, islandPrefabs.Length);
            var instance = _gridData[Random.Range(0, _gridData.Count)];
            var pos = instance.Pos;

            if (instance.Occupied) continue; //Check if cell isn't already occupied
            Instantiate(islandPrefabs[prefabIndex], pos, Quaternion.identity);
            instance.Occupied = true;

            _numberSpawned++;
        }
        
        var spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint").ToList();

        for (var i = spawnPoints.Count - 1; i >= 1; i--)
        {
            Destroy(spawnPoints[i]);
            spawnPoints.Remove(spawnPoints[i]);
        }

        generated = true;
    }
    
    private void UpdatePlayer()
    {
        if (generated)
        {
            rb.isKinematic = false;
        }
    }

    //Store data for every grid
    private class GridData
    {
        public Vector3 Pos { get; set; }
        public bool Occupied { get; set; }
    }
}