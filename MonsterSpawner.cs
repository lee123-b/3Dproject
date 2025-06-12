using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider))]
public class MonsterSpawner : MonoBehaviour
{
    [Header("===== Drag & Drop Settings =====")]
    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private BoxCollider spawnArea;

    private List<GameObject> spawnedMonsters = new List<GameObject>();

    [Header("===== Spawn Parameters =====")]
    [SerializeField] private int spawnCount = 10;
    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private float spawnDistance = 0.5f;

    private void Start()
    {
        if (spawnArea == null)
            spawnArea = GetComponent<BoxCollider>();

        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            SpawnOneAtFront();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnOneAtFront()
    {
        Vector3 localCenter = spawnArea.center;
        Vector3 size = spawnArea.size;
        float halfX = size.x * 0.5f;
        float halfY = size.y * 0.5f;
        float halfZ = size.z * 0.5f;

        Vector3 localPos = localCenter
                         + Vector3.forward * (halfZ + spawnDistance)
                         + new Vector3(Random.Range(-halfX, halfX), Random.Range(-halfY, halfY), 0);

        Vector3 worldPos = spawnArea.transform.TransformPoint(localPos);

        GameObject monster = Instantiate(monsterPrefab, worldPos, spawnArea.transform.rotation);
        spawnedMonsters.Add(monster);

        Rigidbody rb = monster.GetComponent<Rigidbody>();
        if (rb != null) rb.detectCollisions = true;
    }

    public IEnumerator SpawnWave(int count, float interval)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnOneAtFront();
            yield return new WaitForSeconds(interval);
        }
    }

    public void DestroyAllMonsters()
    {
        foreach (GameObject monster in spawnedMonsters)
        {
            if (monster != null) Destroy(monster);
        }

        spawnedMonsters.Clear();
    }

    public void ResetSpawner()
    {
        DestroyAllMonsters();
    }

    public void OnMonsterKilled()
    {
        WaveManager.Instance?.OnEnemyKilled();
    }
}
