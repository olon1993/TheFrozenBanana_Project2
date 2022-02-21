using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public GameObject carPrefab;
    public int carsToSpawn;
    public int variance = 5;

    private void Start()
    {
        carsToSpawn = carsToSpawn + Random.Range(-variance, variance);
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        int count = 0;
        while (count < carsToSpawn)
        {
            GameObject obj = Instantiate(carPrefab);
            Transform child = transform.GetChild(Random.Range(0, transform.childCount - 1));
            obj.GetComponent<WaypointNavigator>().currentWaypoint = child.GetComponent<Waypoint>();
            obj.transform.position = child.position;
            obj.transform.up = child.transform.right;

            yield return new WaitForEndOfFrame();

            count++;
        }
    }
}
