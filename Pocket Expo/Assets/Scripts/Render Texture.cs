#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class CreateRenderTextureTool
{
    [MenuItem("Tools/Create Render Texture")]
    public static void CreateRT()
    {
        RenderTexture rt = new RenderTexture(1920, 1080, 24);
        AssetDatabase.CreateAsset(rt, "Assets/VideoRenderTexture.renderTexture");
        AssetDatabase.SaveAssets();
        Debug.Log("Render Texture created at Assets/VideoRenderTexture.renderTexture");
    }
}
#endif