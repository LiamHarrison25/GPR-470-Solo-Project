using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{
    [Header ("Chunk Settings")]
    [SerializeField] private bool infiniteTerrain = false;
    [SerializeField] private Vector3Int chunkCount = Vector3Int.one;
    [SerializeField] private float renderDistance = 30f;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private int worldChunkRadius = 30;
    [SerializeField] private float halfChunkSize = 0.5f; // NOTE: Stored in half size to avoid division

    [SerializeField] private GameObject chunkPrefab;
    
    private GameObject gridGameObject;
    
    private List<Chunk> chunks;
    private Dictionary<Vector3Int, Chunk> chunkDictionary;
    private Queue<Chunk> recycledChunks;
    private Chunk playerChunk;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        // Init the data structures
        chunks = new List<Chunk>();
        chunkDictionary = new Dictionary<Vector3Int, Chunk>();
        recycledChunks = new Queue<Chunk>();
        
        // Create the chunks
        TemporaryCreateChunks();
        
        // Set the player chunk
        
        
    }

    private void TemporaryCreateChunks()
    {
        int i, j;
        for (i = 0; i < worldChunkRadius; i++)
        {
            for (j = 0; j < worldChunkRadius; j++)
            {
                GameObject c = Instantiate(chunkPrefab, new Vector3(i, 0, j), Quaternion.identity);
                c.transform.SetParent(transform);
            }
        }
    }

    private void UpdatePlayerPosition()
    {
        // Check if the player has moved out of the chunk
        if (playerChunk != null)
        {
            if (!IsPlayerInChunk(playerChunk, playerTransform.position))
            {
                // If the player has moved out of the chunk, switch the playerChunk
                Vector3Int newChunkQuantized = GridFunctions.QuantizeVector3Int(playerTransform.position, halfChunkSize * 2);

                playerChunk = chunkDictionary[newChunkQuantized];

                if (playerChunk == null)
                {
                    throw new NullReferenceException("playerChunk is null. Could not find a chunk in the dictionary");
                }
            }
        }
    }

    /// <summary>
    /// Checks if the player is within the bounds of a chunk
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="playerTransform"></param>
    /// <returns></returns>
    private bool IsPlayerInChunk(Chunk chunk, Vector3 playerTransform)
    {
        if (playerTransform.x - chunk.chunkCenter.x < halfChunkSize)
        {
            return false;
        }
        else if (playerTransform.y - chunk.chunkCenter.y < halfChunkSize)
        {
            return false;
        }
        else if (playerTransform.z - chunk.chunkCenter.z < halfChunkSize)
        {
            return false;
        }
        
        return true;
    }
    
}
