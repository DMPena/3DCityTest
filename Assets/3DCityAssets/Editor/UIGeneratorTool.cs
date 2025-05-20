using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class UIGeneratorTool : EditorWindow
{
    private string prefabFolder = "Assets/SimplePoly City - Low Poly Assets/Prefab/Natures"; // change as needed
    private string savePath = "Assets/3DCityAssets/Icons/Nature";       // where icons will go
    private int iconSize = 256;
    private bool sameSizeIcons = false; // New option for same-size icons

    [MenuItem("Tools/Generate UI From Folder")]
    public static void ShowWindow()
    {
        GetWindow<UIGeneratorTool>("Prefab Icon Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Prefab Icon Generator", EditorStyles.boldLabel);
        prefabFolder = EditorGUILayout.TextField("Prefab Folder", prefabFolder);
        savePath = EditorGUILayout.TextField("Save Path", savePath);
        iconSize = EditorGUILayout.IntField("Icon Size (px)", iconSize);
        sameSizeIcons = EditorGUILayout.Toggle("Same Size Icons", sameSizeIcons); // Add toggle for same-size icons

        if (GUILayout.Button("Generate UI"))
        {
            GenerateIconsFromFolder();
        }
    }

    private void GenerateIconsFromFolder()
    {
        if (!Directory.Exists(savePath))
            Directory.CreateDirectory(savePath);

        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { prefabFolder });
        List<GameObject> prefabs = new List<GameObject>();

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab != null)
                prefabs.Add(prefab);
        }

        Debug.Log($"📦 Found {prefabs.Count} prefabs in folder: {prefabFolder}");

        // Setup camera
        RenderTexture rt = new RenderTexture(iconSize, iconSize, 24);
        Camera cam = new GameObject("RenderCam").AddComponent<Camera>();
        cam.backgroundColor = new Color(0, 0, 0, 0); // Transparent background
        cam.clearFlags = CameraClearFlags.SolidColor; // Clear with solid color
        cam.targetTexture = rt;
        cam.orthographic = true;
        cam.enabled = false;

        // Create a unique layer for rendering
        int iconRenderLayer = LayerMask.NameToLayer("IconRender");
        if (iconRenderLayer == -1)
        {
            Debug.LogError("Layer 'IconRender' does not exist. Please create it in the Unity Editor.");
            return;
        }
        cam.cullingMask = 1 << iconRenderLayer; // Only render the 'IconRender' layer

        float globalMaxExtent = 0f; // To store the largest extent for same-size icons

        // First pass: Calculate global max extent if sameSizeIcons is enabled
        if (sameSizeIcons)
        {
            foreach (GameObject prefab in prefabs)
            {
                GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                Renderer[] renderers = instance.GetComponentsInChildren<Renderer>();
                if (renderers.Length > 0)
                {
                    Bounds bounds = renderers[0].bounds;
                    foreach (Renderer r in renderers)
                        bounds.Encapsulate(r.bounds);

                    float maxExtent = Mathf.Max(bounds.extents.x, bounds.extents.y, bounds.extents.z);
                    globalMaxExtent = Mathf.Max(globalMaxExtent, maxExtent);
                }
                DestroyImmediate(instance);
            }
        }

        // Second pass: Render icons
        foreach (GameObject prefab in prefabs)
        {
            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            instance.transform.position = Vector3.zero;
            instance.transform.rotation = Quaternion.identity;

            // Store original layers and assign the 'IconRender' layer
            Dictionary<Transform, int> originalLayers = new Dictionary<Transform, int>();
            foreach (Transform child in instance.GetComponentsInChildren<Transform>())
            {
                originalLayers[child] = child.gameObject.layer;
                child.gameObject.layer = iconRenderLayer;
            }

            // Calculate bounds
            Renderer[] renderers = instance.GetComponentsInChildren<Renderer>();
            if (renderers.Length == 0)
            {
                DestroyImmediate(instance);
                continue;
            }

            Bounds bounds = renderers[0].bounds;
            foreach (Renderer r in renderers)
                bounds.Encapsulate(r.bounds);

            Vector3 boundsCenter = bounds.center;
            float maxExtent = Mathf.Max(bounds.extents.x, bounds.extents.y, bounds.extents.z);

            // Adjust camera orthographic size
            if (sameSizeIcons)
            {
                maxExtent = globalMaxExtent; // Use the global max extent for same-size icons
            }

            // Camera positioning
            Vector3 camDir = new Vector3(0, 0, -1);
            cam.orthographicSize = maxExtent * 1.2f;
            cam.transform.position = boundsCenter - camDir * (maxExtent * 3f);
            cam.transform.LookAt(boundsCenter);

            // Add directional light
            GameObject lightGO = new GameObject("TempLight");
            Light light = lightGO.AddComponent<Light>();
            light.type = LightType.Directional;
            light.transform.rotation = Quaternion.Euler(50, -30, 0);

            // Render and save
            cam.Render();

            RenderTexture.active = rt;
            Texture2D tex = new Texture2D(iconSize, iconSize, TextureFormat.ARGB32, false); // Supports transparency
            tex.ReadPixels(new Rect(0, 0, iconSize, iconSize), 0, 0);
            tex.Apply();
            RenderTexture.active = null;

            string filePath = Path.Combine(savePath, prefab.name + "_icon.png");
            File.WriteAllBytes(filePath, tex.EncodeToPNG());
            Debug.Log($"✅ Saved: {filePath}");

            AssetDatabase.ImportAsset(filePath);
            TextureImporter importer = AssetImporter.GetAtPath(filePath) as TextureImporter;
            if (importer != null)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.alphaIsTransparency = true;
                importer.SaveAndReimport();
            }

            // Restore original layers
            foreach (var kvp in originalLayers)
            {
                kvp.Key.gameObject.layer = kvp.Value;
            }

            DestroyImmediate(lightGO);
            DestroyImmediate(instance);
        }

        DestroyImmediate(cam.gameObject);
        rt.Release();
        AssetDatabase.Refresh();
        Debug.Log("🎉 Icon generation complete!");
    }
}
