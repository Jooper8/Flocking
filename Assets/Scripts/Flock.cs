using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockManager flockManager;
    float flockSpeed;
    bool isTurning = false;
    void Start()
    {
        flockSpeed = Random.Range(flockManager.minSpeed, flockManager.maxSpeed);
    }
    void Update()
    {
        Bounds bounds = new Bounds(flockManager.transform.position, flockManager.swimLimits * 2);
        RaycastHit hit;
        Vector3 flockDirection = flockManager.transform.position - transform.position;
        if (!bounds.Contains(transform.position))
        {
            isTurning = true;
            flockDirection = flockManager.transform.position - transform.position;
        }
        else if (Physics.Raycast(transform.position, transform.forward * 50, out hit))
        {
            isTurning = true;
            flockDirection = Vector3.Reflect(transform.forward, hit.normal);
        }
        else
        {
            isTurning = false;
        }
        if (isTurning)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(flockDirection),
                flockManager.rotationSpeed * Time.deltaTime);
        }
        else
        {
            if (Random.Range(0, 100) < 10)
            {
                flockSpeed = Random.Range(flockManager.minSpeed, flockManager.maxSpeed);
            }
            if (Random.Range(0, 100) < 20)
            {
                ApplyFlockRules();
            }
        }
        transform.Translate(0, 0, Time.deltaTime * flockSpeed);
    }

    void ApplyFlockRules()
    {
        GameObject[] allFlocks;
        allFlocks = flockManager.allFlock;
        Vector3 flockCenter = Vector3.zero;
        Vector3 flockAvoidance = Vector3.zero;
        float flockAvgSpeed = 0.01f;
        float neighborDistance;
        int flockSize = 0;

        foreach (GameObject flock in allFlocks)
        {
            if (flock != this.gameObject)
            {
                neighborDistance = Vector3.Distance(flock.transform.position, transform.position);

                if (neighborDistance <= flockManager.neighborDistance)
                {
                    flockCenter += flock.transform.position;
                    flockSize++;

                    if (neighborDistance < 1f)
                    {
                        flockAvoidance += (transform.position - flock.transform.position);
                    }
                    Flock anotherFlock = flock.GetComponent<Flock>();
                    flockAvgSpeed += anotherFlock.flockSpeed;
                }
            }
        }
        if (flockSize > 0)
        {
            flockCenter /= flockSize;
            flockSpeed = flockAvgSpeed / flockSize;
            Vector3 direction = (flockCenter - flockAvoidance) - transform.position;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), flockManager.rotationSpeed * Time.deltaTime);
            }
        }
    }
}
