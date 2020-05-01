using UnityEngine;

public class Planet : MonoBehaviour
{
    [Range(2, 256)] public int resolution = 10;
    public bool autoUpdate = true;

    [SerializeField] public ShapeSettings shapeSettings;
    [SerializeField] public ColourSettings colourSettings;

    [HideInInspector] public bool shapeSettingsFoldout;
    [HideInInspector] public bool colourSettingsFoldout;

    private ShapeGenerator shapeGenerator;

    [SerializeField, HideInInspector] private MeshFilter[] meshFilters;
    private TerrainFace[] terrainFaces;

    private void Initialize()
    {
        shapeGenerator = new ShapeGenerator(shapeSettings);

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

            terrainFaces[i] = new TerrainFace(shapeGenerator, meshFilters[i].sharedMesh, resolution, directions[i]);
        }
    }

    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColours();
    }

    public void OnShapeSettingsUpdated()
    {
        if (!autoUpdate) return;
        Initialize();
        GenerateMesh();
    }

    public void OnColourSettingsUpdated()
    {
        if (!autoUpdate) return;
        Initialize();
        GenerateColours();
    }

    private void GenerateMesh()
    {
        foreach (var face in terrainFaces)
        {
            face.ConstructMesh();
        }
    }

    private void GenerateColours()
    {
        foreach (var meshFilter in meshFilters)
        {
            meshFilter.GetComponent<MeshRenderer>().sharedMaterial.color = colourSettings.planetColour;
        }
    }
}