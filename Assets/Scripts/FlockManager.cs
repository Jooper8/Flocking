using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public GameObject birdPrefab;
    public int numBirds = 20;
    public GameObject[] allFlock;
    public Vector3 swimLimits = new Vector3(5, 5, 5);
    public Vector3 goalPosition;
    public GameObject goal;

    [Header("Bird Configuration")]
    [Range(0f, 5f)]
    public float minSpeed;
    [Range(0f, 5f)]
    public float maxSpeed;
    [Range(1f, 10f)]
    public float neighborDistance;
    [Range(5f, 5f)]
    public float rotationSpeed;

    void Start()
    {
        allFlock = new GameObject[numBirds];
        for (int i = 0; i < numBirds; i++)
        {
            Vector3 position = transform.position + new Vector3(
                Random.Range(-swimLimits.x, swimLimits.x),
                Random.Range(-swimLimits.y, swimLimits.y),
                Random.Range(-swimLimits.z, swimLimits.z));
            allFlock[i] = Instantiate(birdPrefab, position, Quaternion.identity);
            allFlock[i].GetComponent<Flock>().flockManager = this;
        }
        goalPosition = transform.position;
    }
    void Update()
    {
        goalPosition = transform.position;
        if (Random.Range(0, 100) < 10)
        {
            goalPosition = transform.position + new Vector3(
                Random.Range(-swimLimits.x, swimLimits.x),
                Random.Range(-swimLimits.y, swimLimits.y),
                Random.Range(-swimLimits.z, swimLimits.z));
        }
    }
}
