using UnityEngine;

public class CloudBuilder : MonoBehaviour
{
    public string spritePath = "cloud"; 
    public string cloudLayerName = "Clouds";

    [HideInInspector] public GameObject cloudPrefab;

    void Awake()
    {
        // Check if the layer exists
        int cloudLayer = LayerMask.NameToLayer(cloudLayerName);
        if (cloudLayer == -1)
        {
            Debug.LogError($"Layer '{cloudLayerName}' not found. Please add it under Edit > Project Settings > Tags and Layers.");
            return;
        }

        // Load sprite from Resources
        Sprite sprite = Resources.Load<Sprite>(spritePath);
        if (sprite == null)
        {
            Debug.LogError($"Could not load sprite at Resources/{spritePath}");
            return;
        }
        Debug.Log($"Sprite '{sprite.name}' loaded successfully.");

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

        // Scale up the cloud so it's visible
        quad.transform.localScale = new Vector3(3f, 2f, 0.5f); // Width x Height

        // Remove collider
        DestroyImmediate(quad.GetComponent<Collider>());

        // Add billboard script
        quad.AddComponent<Billboard>();

        // Store as prefab in memory
        cloudPrefab = quad;

        // Hide original prefab
        quad.SetActive(false);

        Debug.Log("Cloud prefab created and stored.");
    }
}
// This script builds a cloud prefab from a sprite and prepares it for instantiation in the scene.
// It creates a quad with a transparent material, applies the sprite texture, and adds a billboard effect.