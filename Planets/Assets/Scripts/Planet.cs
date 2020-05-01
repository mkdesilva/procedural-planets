using UnityEngine;

public class Planet : MonoBehaviour
{
    [Range(2, 256)] public int resolution = 10;
    [SerializeField, HideInInspector] private MeshFilter[] meshFilters;
    private TerrainFace[] terrainFaces;

    private void OnValidate()
    {
        Initialize();
        GenerateMesh();
    }

    private void Initialize()
    {
        const int numOfFaces = 6;
        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[numOfFaces];
        }

        terrainFaces = new TerrainFace[6];

        Vector3[] directions = {Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back,};

        for (var i = 0; i < numOfFaces; i++)
        {
            if (meshFilters[i] == null)
            {
                var meshObj = new GameObject("Mesh");
                meshObj.transform.parent = transform;
                meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }

            terrainFaces[i] = new TerrainFace(meshFilters[i].sharedMesh, resolution, directions[i]);
        }
    }

    private void GenerateMesh()
    {
        foreach (var face in terrainFaces)
        {
            face.ConstructMesh();
        }
    }
}