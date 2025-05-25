using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{

    [Header ("Chunk Settings")]
    [SerializeField] private bool infiniteTerrain = false;
    private Vector3Int chunkCount = Vector3Int.one;
    
    private GameObject gridGameObject;
    
    private List<Chunk> chunks;
    private Dictionary<Vector3Int, Chunk> chunkDictionary;
    private Queue<Chunk> recyucledChunks;
}
