using UnityEngine;

public class GridFunctions
{
    // Helper function to quantize a Vector3 into a Vector3Int
    public static Vector3Int QuantizeVector3Int(Vector3 vec, float containerSize)
    {
        return new Vector3Int
        (
            (int) Mathf.Floor(vec.x / containerSize),
            (int) Mathf.Floor(vec.y / containerSize),
            (int) Mathf.Floor(vec.z / containerSize)
        );
    }
}
