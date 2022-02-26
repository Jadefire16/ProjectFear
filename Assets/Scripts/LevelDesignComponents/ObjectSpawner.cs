using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject prefab;

    [SerializeField] private bool spawnOnStart = false;
    [SerializeField] private float spawnOnStartDelay = 1f;


    [SerializeField] private List<GameObject> instantiatedPrefabs = new List<GameObject>(16);

    private IEnumerator Start()
    {
        if (prefab == null)
            yield break;
        if (spawnOnStart)
        {
            yield return new WaitForSeconds(spawnOnStartDelay);
            SpawnPrefab(transform.position, Quaternion.identity);
        }
        else
            yield break;
    }

    public void SpawnPrefab(Vector3 position, Quaternion rotation)
    {
        var obj = Instantiate(prefab, position, rotation);
        instantiatedPrefabs.Add(obj);
    }

    public void SpawnPrefab() => SpawnPrefab(transform.position, transform.rotation);

}
