using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.EventSystems;


public class MapEditor : EditorWindow
{
    [SerializeField] bool placeMode = false;
    [SerializeField] private float gridSize = 1.0f;
    [SerializeField] private bool useGrid = true;
    [SerializeField] private bool useRayGrid = false;

    [SerializeField] private List<GameObject> palette = new List<GameObject>();
    [SerializeField] private int paletteIndex;
    [SerializeField] private int placementLayer = 0;
    [SerializeField] Transform outputTransform;
    [SerializeField] private string path = "Assets/Editor/Resources/Palette";

    [MenuItem("MapEditor/MapEditor")]
    private static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(MapEditor));
    }

    private void OnGUI()
    {


        placeMode = GUILayout.Toggle(placeMode, "Start Placing", "Button", GUILayout.Height(60.0f));
        useGrid = EditorGUILayout.Toggle("Use Grid", useGrid);
        useRayGrid = EditorGUILayout.Toggle("Use Ray Grid", useRayGrid);
        placementLayer = EditorGUILayout.IntField("Grid Layer:", placementLayer);
        outputTransform = EditorGUILayout.ObjectField("OutputTransform", outputTransform, typeof(Transform), true) as Transform;



        if (useGrid && useRayGrid)
        {
            useGrid = !useGrid;
            useRayGrid = !useRayGrid;
        }
      
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
        if (placeMode)
        {
            //  Vector2 cellCenter = GetSelectedGridCell();
            Vector3 hoverPos = GetHoverPosition();

            if (useRayGrid || useGrid)
            {
                DisplayVisualHelp(hoverPos);
            }

            HandleSceneViewInputs(hoverPos);

            // Refresh the view
            sceneView.Repaint();
        }
    }
    private Vector3 GetHoverPosition()
    {

        Vector3 returnValue = new Vector3(0.0f, -2.0f, 0.0f);
        Ray guiRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);



        Vector3 origin = guiRay.origin;
        Vector3 mouseRay = guiRay.direction;
        Vector3 planeNormal = new Vector3(0.0f, 1.0f, 0.0f);

        float planeDotCheck = Vector3.Dot(planeNormal, mouseRay.normalized);

        if (placementLayer < origin.y)
        {


            if (useGrid)
            {
                if (planeDotCheck < 0.0f)
                {
                    float dot1 = Vector3.Dot(planeNormal, guiRay.origin) - (float)placementLayer;
                    float dot2 = Vector3.Dot(planeNormal, guiRay.direction);

                    float VectorTimesVal = dot1 / (dot2 * -1.0f);

                    returnValue = guiRay.origin + (guiRay.direction * VectorTimesVal);


                    returnValue.x = Mathf.RoundToInt(returnValue.x);
                    returnValue.y = Mathf.RoundToInt(returnValue.y);
                    returnValue.z = Mathf.RoundToInt(returnValue.z);
                }
            }
            else
            {



                RaycastHit[] hits = Physics.RaycastAll(origin, mouseRay, 100.0f);
                RaycastHit closestTarget = new RaycastHit();
                bool targetHit = false;
                float lastDistance = 1000.0f;

                foreach (RaycastHit hit in hits)
                {
                    if (hit.transform.GetComponent<Cube>() != null)
                    {
                        Vector3 vectorFromOrigin = hit.transform.position - origin;

                        if (vectorFromOrigin.magnitude < lastDistance)
                        {
                            targetHit = true;
                            closestTarget = hit;
                            lastDistance = vectorFromOrigin.magnitude;
                        }

                    }
                }

                if (targetHit)
                {

                    if (useRayGrid)
                    {

                        int x = (int)closestTarget.transform.position.x;
                        int y = (int)closestTarget.transform.position.y;
                        int z = (int)closestTarget.transform.position.z;

                        returnValue = new Vector3(x, y, z);
                        returnValue += closestTarget.normal * gridSize;
                    }
                    else
                    {
                        returnValue = closestTarget.point;
                    }
                }

            }
        }
        return returnValue;
    }

    private void HandleSceneViewInputs(Vector3 spawnPosition)
    {
        // Filter the left click so that we can't select objects in the scene
        if (Event.current.type == EventType.Layout)
        {
            HandleUtility.AddDefaultControl(0); // Consume the event
        }

        if (paletteIndex < palette.Count && Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            bool spawn = true;
            if (useGrid || useRayGrid)
            {
                if (Physics.CheckBox(spawnPosition, new Vector3((gridSize / 4.0f), (gridSize / 4.0f), (gridSize / 4.0f))))
                {
                    spawn = false;
                }
            }

            if(spawn)
            {
                GameObject prefab = palette[paletteIndex];
                GameObject gameObject = PrefabUtility.InstantiatePrefab(prefab, outputTransform) as GameObject;

                gameObject.transform.position = spawnPosition;
                Undo.RegisterCreatedObjectUndo(gameObject, "");
            }

        }
    }

    private void DisplayVisualHelp(Vector3 gridCenter)
    {

        // Vertices of our square
        Vector3 topLeftFront = gridCenter + Vector3.left * gridSize * 0.5f + Vector3.up * gridSize * 0.5f + Vector3.forward * gridSize * -0.5f;
        Vector3 topRightFront = gridCenter - Vector3.left * gridSize * 0.5f + Vector3.up * gridSize * 0.5f + Vector3.forward * gridSize * -0.5f;
        Vector3 bottomLeftFront = gridCenter + Vector3.left * gridSize * 0.5f - Vector3.up * gridSize * 0.5f + Vector3.forward * gridSize * -0.5f;
        Vector3 bottomRightFront = gridCenter - Vector3.left * gridSize * 0.5f - Vector3.up * gridSize * 0.5f + Vector3.forward * gridSize * -0.5f;
        Vector3 topLeftBack = gridCenter + Vector3.left * gridSize * 0.5f + Vector3.up * gridSize * 0.5f + Vector3.forward * gridSize * 0.5f;
        Vector3 topRightBack = gridCenter - Vector3.left * gridSize * 0.5f + Vector3.up * gridSize * 0.5f + Vector3.forward * gridSize * 0.5f;
        Vector3 bottomLeftBack = gridCenter + Vector3.left * gridSize * 0.5f - Vector3.up * gridSize * 0.5f + Vector3.forward * gridSize * 0.5f;
        Vector3 bottomRightBack = gridCenter - Vector3.left * gridSize * 0.5f - Vector3.up * gridSize * 0.5f + Vector3.forward * gridSize * 0.5f;

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
