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

    [Header("Other")] 
    public Rigidbody rb;
    
    private readonly List<CellData> _cells = new List<CellData>();
    private float _numberSpawned;

    private void Awake()
    {
        //Make sure there cannot be more islands than there are cells
        maxIslands = Mathf.Clamp(maxIslands, 0, gridX * gridZ);
        
        SpawnGrid();
        CreateSpawnPoint();
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
                var data = new CellData
                {
                    Pos = spawnPos,
                    Occupied = false
                };
                _cells.Add(data);
            }
        }

        //Spawn Islands on random cells in the grid
        while (_numberSpawned < maxIslands)
        {
            var cell = _cells[Random.Range(0, _cells.Count)];

            if (cell.Occupied) continue; //Check if cell isn't already occupied
            var pos = cell.Pos;
            var prefabIndex = Random.Range(0, islandPrefabs.Length);
            
            //Instantiate Island on Cell
            Instantiate(islandPrefabs[prefabIndex], pos, Quaternion.identity);
            cell.Occupied = true;

            _numberSpawned++;
        }

        generated = true;
    }

    private void CreateSpawnPoint()
    {
        //Destroy all duplicate SpawnPoints
        var spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint").ToList();

        for (var i = spawnPoints.Count - 1; i >= 1; i--)
        {
            Destroy(spawnPoints[i]);
            spawnPoints.Remove(spawnPoints[i]);
        }
    }
    
    private void UpdatePlayer()
    {
        //Spawn the Player on the SpawnPoint as soon as the map has been generated
        if (generated)
        {
            rb.position = GameObject.Find("SpawnPoint").transform.position;
        }
    }

    //Store data for every Cell on the Grid
    private class CellData
    {
        public Vector3 Pos { get; set; }
        public bool Occupied { get; set; }
    }
}