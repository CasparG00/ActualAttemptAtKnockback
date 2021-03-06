﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GenerateMap : MonoBehaviour
{
    [Header("Island Settings")]
    public GameObject[] islandPrefabs;
    public int maxIslands = 25;
    public Vector2Int MinMaxHeight = new Vector2Int(-10, 10);
    public Vector2Int MinMaxOffset = new Vector2Int(-10, 10);
    
    [Header("Grid Settings")] 
    public Vector3 gridOrigin = Vector3.zero;
    [Min(0)]
    public Vector2Int Grid = new Vector2Int(5, 5);
    [Space] public float gridSpacing;

    [Header("Other")] 
    public Rigidbody rb;
    
    private readonly List<CellData> _cells = new List<CellData>();
    private int _numberOfIslandsSpawned;
    private Vector3 _spawnPoint;

    private void Awake()
    {
        //Make sure there cannot be more islands than there are cells
        maxIslands = Mathf.Clamp(maxIslands, 0, Grid.x * Grid.y);
        
        SpawnGrid();
        CreateSpawnPoint();

        UpdatePlayer();
    }
    
    private void SpawnGrid()
    {
        //Create grid and save grid data
        for (var x = 0; x < Grid.x; x++)
        {
            for (var z = 0; z < Grid.y; z++)
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
        while (_numberOfIslandsSpawned < maxIslands)
        {
            var cell = _cells[Random.Range(0, _cells.Count)];

            if (cell.Occupied) continue; //Check if cell isn't already occupied
            
            //Offset each island randomly
            var yOffset = Random.Range(MinMaxHeight.x, MinMaxHeight.y);
            var xOffset = Random.Range(MinMaxOffset.x, MinMaxOffset.y);
            var zOffset = Random.Range(MinMaxOffset.x, MinMaxOffset.y);
            
            var pos = new Vector3(cell.Pos.x + xOffset, cell.Pos.y + yOffset, cell.Pos.z + zOffset);
            var prefabIndex = Random.Range(0, islandPrefabs.Length);
            
            //Instantiate Island on Cell
            Instantiate(islandPrefabs[prefabIndex], pos, Quaternion.identity);
            cell.Occupied = true;

            _numberOfIslandsSpawned++;
        }
    }

    private void CreateSpawnPoint()
    {
        //Destroy all duplicate SpawnPoints
        var spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint").ToList();

        for (var i = spawnPoints.Count; i > 1; i--)
        {
            var random = Random.Range(0, spawnPoints.Count);
            Destroy(spawnPoints[random]);
            spawnPoints.Remove(spawnPoints[random]);
        }
        _spawnPoint = spawnPoints[0].transform.position;
    }

    private void UpdatePlayer()
    {
        //Spawn the Player on the SpawnPoint as soon as the map has been generated
        rb.position = _spawnPoint;
    }

    //Store data for every Cell on the Grid
    private class CellData
    {
        public Vector3 Pos { get; set; }
        public bool Occupied { get; set; }
    }
}