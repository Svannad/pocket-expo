using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    public CloudBuilder builder;
    public int cloudCount = 5;
    public Vector3 spawnCenter = new Vector3(0, 5, 10);
    public Vector3 spawnRange = new Vector3(5, 2, 1);

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

            // Add CloudBehavior script to control size, drifting, and fade-in
            cloud.AddComponent<CloudBehavior>();

            Debug.Log($"Spawned cloud at {cloud.transform.position}");
        }
    }
}
// This script spawns a specified number of clouds at random positions within a defined range around a center point.