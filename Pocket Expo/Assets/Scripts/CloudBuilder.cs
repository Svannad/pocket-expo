using UnityEngine;

public class CloudBuilder : MonoBehaviour
{
    public string spritePath = "cloud"; // Inside Resources folder
    public string cloudLayerName = "Clouds";

    [HideInInspector] public GameObject cloudPrefab;
    

    void Awake()
    {
        // Check if the layer exists
        int cloudLayer = LayerMask.NameToLayer(cloudLayerName);
        if (cloudLayer == -1)
        {
            Debug.LogError($"Layer '{cloudLayerName}' not found. Please add it manually under 'Edit > Project Settings > Tags and Layers'.");
            return;
        }

        // Load sprite from Resources
        Sprite sprite = Resources.Load<Sprite>(spritePath);
        if (sprite == null)
        {
            Debug.LogError($"Could not load sprite at Resources/{spritePath}");
            return;
        }

        // Extract texture
        Texture2D texture = sprite.texture;

        // Create transparent material
        Material mat = new Material(Shader.Find("Unlit/Transparent"));
        mat.mainTexture = texture;

        // Create a quad and apply the material
        GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        quad.name = "Cloud";
        quad.layer = cloudLayer;
        quad.GetComponent<MeshRenderer>().material = mat;

        // Remove unnecessary collider
        DestroyImmediate(quad.GetComponent<Collider>());

        // Add billboard behavior
        quad.AddComponent<Billboard>();

        // Store as prefab in memory
        cloudPrefab = quad;

        // Hide the original quad
        quad.SetActive(false);
    }
}
// This script creates a cloud prefab with a transparent material and billboard behavior.