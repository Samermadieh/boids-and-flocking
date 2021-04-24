// Name: Samer Madieh
// File: flock.cs
// Description: script for single boid movement. This script is attached to the boid prefab.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flock : MonoBehaviour
{
    public float speed = 0.001f;
    float rotationSpeed = 4.0f;
    Vector3 averageHeading;
    Vector3 averagePostion;
    float neighborDistance = 3.0f;

    bool turn = false;

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(1, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, Vector3.zero) >= globalFlock.tankSize)
        {
            turn = true;
        }
        else
        {
            turn = false;
        }

        if(turn == true)
        {
            Vector3 direction = Vector3.zero - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
            speed = Random.Range(1, 1.5f);
        }
        else
        {
            if (Random.Range(0, 5) < 1)
            {
                ApplyRules();
            }
        }
        transform.Translate(0, 0, Time.deltaTime * speed);
    }

    // Apply Rules to boids function
    void ApplyRules()
    {
        GameObject[] gos;
        gos = globalFlock.boids;

        Vector3 vcenter = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        float groupSpeed = 0.1f;

        Vector3 goal = globalFlock.goal;

        float dist;

        int groupSize = 0;
        foreach(GameObject go in gos)
        {
            if(go != this.gameObject)
            {
                dist = Vector3.Distance(go.transform.position, this.transform.position);
                if(dist <= neighborDistance)
                {
                    vcenter += go.transform.position;
                    groupSize++;

                    if(dist < 1.0f)
                    {
                        vavoid = vavoid + (this.transform.position - go.transform.position);
                    }

                    flock anotherFlock = go.GetComponent<flock>();
                    groupSpeed = groupSpeed + anotherFlock.speed;
                }
            }
        }

        if(groupSize > 0)
        {
            vcenter = vcenter / groupSize + (goal - this.transform.position);
            speed = groupSpeed / groupSize;

            Vector3 direction = (vcenter + vavoid) - transform.position;
            if(direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
            }
        }
    }
}
