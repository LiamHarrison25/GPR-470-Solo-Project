using UnityEngine;

public class SoftBody : MonoBehaviour
{
     [Header("Required object references")]
        [SerializeField] private MeshFilter meshFilter;
        [SerializeField] private MeshRenderer meshRenderer;
    
        [Header("Editable Values")]
        [SerializeField] private float squishIntensity = 1;
        [SerializeField] private float mass = 1.0f;
        [SerializeField] private float stiffness = 1.0f;
        [SerializeField] private float damping = 0.75f;
    
        private SoftBodyVertex[] vertices;
        private Vector3[] vertexArray;
        
        private Mesh originalMesh;
        private Mesh meshClone;
        
    
        private void Start()
        {
            originalMesh = meshFilter.sharedMesh;
            meshClone = Instantiate(originalMesh);
            meshFilter.sharedMesh = meshClone;
    
            vertices = new SoftBodyVertex[meshClone.vertices.Length];
            int i;
            for (i = 0; i < meshClone.vertices.Length; i++)
            {
                vertices[i] = new SoftBodyVertex(i, transform.TransformPoint((meshClone.vertices[i])));
            }
    
    
        }
    
        private void FixedUpdate()
        {
            vertexArray = originalMesh.vertices;
            int i;
            for (i = 0; i < vertices.Length; i++)
            {
                Vector3 target = transform.TransformPoint(vertexArray[vertices[i].GetId()]);
                float intensity = (1 - (meshRenderer.bounds.max.y - target.y) / meshRenderer.bounds.size.y) * squishIntensity;
                vertices[i].ShakeMesh(target, mass, stiffness, damping);
                target = transform.InverseTransformPoint(vertices[i].GetPosition());
                vertexArray[vertices[i].GetId()] = Vector3.Lerp(vertexArray[vertices[i].GetId()], target, intensity);
            }
    
            meshClone.vertices = vertexArray;
        }
    
        
}
