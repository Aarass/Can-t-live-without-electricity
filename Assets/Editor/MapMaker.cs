using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;

//[ExecuteInEditMode]
public class MapMaker : EditorWindow
{
    [SerializeField] GameObject meshWrapper;
    [SerializeField] Material mapMaterial;
    [SerializeField] Material planeMaterial;

    [SerializeField] List<Mesh> meshes;
    [SerializeField] List<Texture2D> colors;
    [SerializeField] List<Texture2D> types;

    [SerializeField] GameObject rocksWrapper;
    [SerializeField] GameObject rockPrefab;
    [SerializeField] Material rocksMaterial;

    [SerializeField] GameObject housesWrapper;
    [SerializeField] GameObject housePrefab;


    private int index = 0;

    void OnGUI()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("meshWrapper"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("mapMaterial"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("planeMaterial"));

        EditorGUILayout.PropertyField(serializedObject.FindProperty("meshes"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("colors"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("types"));

        EditorGUILayout.PropertyField(serializedObject.FindProperty("rocksWrapper"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("rockPrefab"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("rocksMaterial"));

        EditorGUILayout.PropertyField(serializedObject.FindProperty("housesWrapper"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("housePrefab"));

        serializedObject.ApplyModifiedProperties(); //once

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Previous"))
        {
            index = Assets.Helper.mod(index - 1, meshes.Count);
        }
        else if (GUILayout.Button("Next"))
        {
            index = Assets.Helper.mod(index + 1, meshes.Count);
        }
        GUILayout.EndHorizontal();
        if (GUILayout.Button("Map"))
            Mesh();
        else if (GUILayout.Button("Plane"))
            Plane();
        else if (GUILayout.Button("Managers"))
            Managers();
        else if (GUILayout.Button("Rocks"))
            Rocks();
        else if (GUILayout.Button("Houses"))
            Houses();
    }
    void Mesh()
    {
        Map.Width = colors[index].width;
        Map.Height = colors[index].height;

        while (meshWrapper.transform.childCount > 0)
            DestroyImmediate(meshWrapper.transform.GetChild(0).gameObject);

        GameObject obj = new GameObject("Map");
        obj.transform.parent = meshWrapper.transform;
        MeshFilter mf = obj.AddComponent<MeshFilter>();
        mf.mesh = meshes[index];
        MeshRenderer mr = obj.AddComponent<MeshRenderer>();
        mr.material = mapMaterial;
        obj.AddComponent<MeshCollider>();
    }
    void Plane()
    {
        planeMaterial.SetTexture("_Texture2D", colors[index]);
        planeMaterial.SetVector("_Dimensions", new Vector3(Map.Width, 1, Map.Height));
    }
    void Managers()
    {
        GridManager.GetInstance().Setup(colors[index]);
        CameraController.GetInstance().Setup();
    }
    void Rocks()
    {
        rocksMaterial.SetTexture("_Texture2D", colors[index]);
        rocksMaterial.SetVector("_Size", new Vector2(Map.Width * 3f, Map.Height * 3f));

        while (rocksWrapper.transform.childCount > 0)
            DestroyImmediate(rocksWrapper.transform.GetChild(0).gameObject);
        GridManager gridManager = GridManager.GetInstance();

        for (int i = 0; i < types[index].width; i++)
        {
            for (int j = 0; j < types[index].height; j++)
            {
                Color color = types[index].GetPixel(i, j);
                int k = Mathf.FloorToInt((color.r + .1f) * 4.0f);

                if (k == 3)
                {
                    Instantiate(rockPrefab, gridManager.CellToWorld(new Cell(i, j)), Quaternion.identity, rocksWrapper.transform);
                }
            }
        }
    }
    void Houses()
    {
        while (housesWrapper.transform.childCount > 0)
            DestroyImmediate(housesWrapper.transform.GetChild(0).gameObject);
        GridManager gridManager = GridManager.GetInstance();

        for (int i = 0; i < types[index].width; i++)
        {
            for (int j = 0; j < types[index].height; j++)
            {
                Color color = types[index].GetPixel(i, j);
                int k = Mathf.FloorToInt((color.r + .1f) * 4.0f);
                if (k == 1)
                {
                    Instantiate(housePrefab, gridManager.CellToWorld(new Cell(i, j)), Quaternion.identity, housesWrapper.transform);
                }
            }
        }
    }
    #region Staff I kinda understand but dont wanna look at yet
    private SerializedObject serializedObject;
    private void Awake()
    {
        var data = EditorPrefs.GetString("Map maker", JsonUtility.ToJson(this, false));
        JsonUtility.FromJsonOverwrite(data, this);
    }
    private void OnEnable()
    {
        serializedObject = new SerializedObject(this);
    }
    protected void OnDisable()
    {
        var data = JsonUtility.ToJson(this, false);
        EditorPrefs.SetString("Map maker", data);
    }
    [MenuItem("Window/Map maker")]
    public static void ShowWindow()
    {
        GetWindow<MapMaker>("Map maker");
    }
    #endregion
}
