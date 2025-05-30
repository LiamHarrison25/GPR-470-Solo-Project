using UnityEngine;

public class Chunk : MonoBehaviour
{
    public Vector3Int chunkCenter;
    
    public Mesh mesh;
    
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private MeshCollider meshCollider;

    [SerializeField] private bool collisionEnabled;

    public void InitChunk(Material material, bool collisionEnabled)
    {
        this.collisionEnabled = collisionEnabled;
        
        // Ensures that the fields are set

        if (meshFilter == null)
        {
            meshFilter = GetComponent<MeshFilter>();
        }

        if (meshRenderer == null)
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        if (meshCollider == null)
        {
            meshCollider = GetComponent<MeshCollider>();
        }
        
        mesh = meshFilter.sharedMesh; // NOTE: May want to use mesh here
        // Creates a new mesh if no mesh is set
        if (mesh == null)
        {
            mesh = new Mesh();
            mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            meshFilter.sharedMesh = mesh;
        }
        
        // Checks if collision is enabled
        if (collisionEnabled)
        {
            if (meshCollider.sharedMesh == null)
            {
                meshCollider.sharedMesh = mesh;
            }
            
            meshCollider.enabled = collisionEnabled;
        }
        else
        {
            if (meshCollider != null)
            {
                Destroy(meshCollider);
            }
        }
        
        // Sets the material of the mesh
        meshRenderer.material = material;
        
    }
    
}
