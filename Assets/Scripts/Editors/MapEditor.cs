using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.EventSystems;
public class MapEditor : EditorWindow
{
    bool placeMode = false;
    private float GridSize = 1.0f;

    [SerializeField] private List<GameObject> palette = new List<GameObject>();
    [SerializeField] private int paletteIndex;
    [SerializeField] private int placementLayer  = 0;
    [SerializeField] private bool useGrid  = true;

    private string path = "Assets/Editor/Resources/Palette";

    [MenuItem("MapEditor/MapEditor")]
    private static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(MapEditor));
    }

    private void OnGUI()
    {
        placeMode = GUILayout.Toggle(placeMode, "Start Placing", "Button", GUILayout.Height(60.0f));
        placeMode = EditorGUILayout.Toggle("Use Grid",useGrid);
        placementLayer = EditorGUILayout.IntField("Grid Layer:", placementLayer);
        // Get a list of previews, one for each of our prefabs
        List<GUIContent> paletteIcons = new List<GUIContent>();

        foreach (GameObject prefab in palette)
        {
            // Get a preview for the prefab
            Texture2D texture = AssetPreview.GetAssetPreview(prefab);
            paletteIcons.Add(new GUIContent(texture));
        }

        // Display the grid
        paletteIndex = GUILayout.SelectionGrid(paletteIndex, paletteIcons.ToArray(), 6); 
    }

    private void OnSceneGUI(SceneView sceneView) 
    {
        if(placeMode)
        {
          //  Vector2 cellCenter = GetSelectedGridCell();
            Vector3 gridCenter = GetSelectedGridPosition();


            DisplayVisualHelp(gridCenter);
            HandleSceneViewInputs(gridCenter);

            // Refresh the view
            sceneView.Repaint();
        }
    }
    private Vector3 GetSelectedGridPosition()
    {

        Vector3 returnValue = new Vector3(0.0f, 0.0f, 0.0f);
        Ray guiRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

        Vector3 origin = guiRay.origin;
        Vector3 mouseRay = guiRay.direction;
        Vector3 planeNormal = new Vector3(0.0f, 1.0f, 0.0f);

        float planeDotCheck = Vector3.Dot(planeNormal, mouseRay.normalized);

        if(planeDotCheck < 0.0f && placementLayer < origin.y)
        {
     
            float dot1 = Vector3.Dot(planeNormal, guiRay.origin) - (float)placementLayer;
            float dot2 = Vector3.Dot(planeNormal, guiRay.direction);

            float VectorTimesVal = dot1 / (dot2 * -1.0f);

            returnValue = guiRay.origin + (guiRay.direction * VectorTimesVal);
            returnValue.x = Mathf.RoundToInt(returnValue.x);
            returnValue.y = Mathf.RoundToInt(returnValue.y);
            returnValue.z = Mathf.RoundToInt(returnValue.z);

        }

        return returnValue;
    }

    private void HandleSceneViewInputs(Vector3 cellCenter)
    {
        // Filter the left click so that we can't select objects in the scene
        if (Event.current.type == EventType.Layout)
        {
            HandleUtility.AddDefaultControl(0); // Consume the event
        }

        if (paletteIndex < palette.Count && Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            // Create the prefab instance while keeping the prefab link
            GameObject prefab = palette[paletteIndex];
            GameObject gameObject = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            gameObject.transform.position = cellCenter;

            // Allow the use of Undo (Ctrl+Z, Ctrl+Y).
            Undo.RegisterCreatedObjectUndo(gameObject, "");
        }
    }

    private void DisplayVisualHelp(Vector3 gridCenter)
    {

        // Vertices of our square
        Vector3 topLeftFront        = gridCenter + Vector3.left * GridSize * 0.5f + Vector3.up * GridSize * 0.5f + Vector3.forward * GridSize * -0.5f;
        Vector3 topRightFront       = gridCenter - Vector3.left * GridSize * 0.5f + Vector3.up * GridSize * 0.5f + Vector3.forward * GridSize * -0.5f;
        Vector3 bottomLeftFront     = gridCenter + Vector3.left * GridSize * 0.5f - Vector3.up * GridSize * 0.5f + Vector3.forward * GridSize * -0.5f;
        Vector3 bottomRightFront    = gridCenter - Vector3.left * GridSize * 0.5f - Vector3.up * GridSize * 0.5f + Vector3.forward * GridSize * -0.5f;
        Vector3 topLeftBack         = gridCenter + Vector3.left * GridSize * 0.5f + Vector3.up * GridSize * 0.5f + Vector3.forward * GridSize * 0.5f;
        Vector3 topRightBack        = gridCenter - Vector3.left * GridSize * 0.5f + Vector3.up * GridSize * 0.5f + Vector3.forward * GridSize * 0.5f;
        Vector3 bottomLeftBack      = gridCenter + Vector3.left * GridSize * 0.5f - Vector3.up * GridSize * 0.5f + Vector3.forward * GridSize * 0.5f;
        Vector3 bottomRightBack     = gridCenter - Vector3.left * GridSize * 0.5f - Vector3.up * GridSize * 0.5f + Vector3.forward * GridSize * 0.5f;

        // Rendering
        Handles.color = Color.green;
        Vector3[] lines = {
            topLeftFront, topRightFront,
            topRightFront, bottomRightFront,
            bottomRightFront, bottomLeftFront,
            bottomLeftFront, topLeftFront,
            topLeftBack, topRightBack,
            topRightBack, bottomRightBack,
            bottomRightBack, bottomLeftBack,
            bottomLeftBack, topLeftBack,
            topLeftFront, topLeftBack,
            topRightFront, topRightBack,
            bottomLeftFront, bottomLeftBack,
            bottomRightFront, bottomRightBack
        };
        Handles.DrawLines(lines);

    }

    private void OnFocus()
    {
        SceneView.duringSceneGui -= this.OnSceneGUI;
        SceneView.duringSceneGui += this.OnSceneGUI;
        RefreshPalette();
    }

    private void RefreshPalette()
    {
        palette.Clear();

        string[] prefabFiles = System.IO.Directory.GetFiles(path, "*.prefab");
        foreach (string prefabFile in prefabFiles)
            palette.Add(AssetDatabase.LoadAssetAtPath(prefabFile, typeof(GameObject)) as GameObject);
    }

    void OnDestroy()
    {
        SceneView.duringSceneGui -= this.OnSceneGUI;
    }

}
