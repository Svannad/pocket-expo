using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    public CloudBuilder builder;
    public int cloudCount = 5;
    public Vector3 spawnCenter = new Vector3(0, 5, 10);  // Position in front of camera
    public Vector3 spawnRange = new Vector3(5, 2, 1);     // Random offset range

    void Start()
    {
        if (builder == null || builder.cloudPrefab == null)
        {
            Debug.LogError("CloudBuilder not set or prefab missing.");
            return;
        }

        for (int i = 0; i < cloudCount; i++)
        {
            Vector3 offset = new Vector3(
                Random.Range(-spawnRange.x, spawnRange.x),
                Random.Range(-spawnRange.y, spawnRange.y),
                Random.Range(-spawnRange.z, spawnRange.z)
            );

            GameObject cloud = Instantiate(builder.cloudPrefab, spawnCenter + offset, Quaternion.identity);
            cloud.SetActive(true);
            cloud.transform.localScale *= Random.Range(0.8f, 1.3f);
        }
    }
}
// This script spawns multiple cloud prefabs at random positions around a specified center point in front of the camera.
