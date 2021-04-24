// Name: Samer Madieh
// File: globalFlock.cs
// Description: script for the whole flock and the BoidManager object. It also controls the UI elements and provides functions for them.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class globalFlock : MonoBehaviour
{
    public GameObject boidPrefab;
    public GameObject goalPrefab;
    public static int tankSize = 5;
    public Button lazyBtn;
    public Button circleBtn;

    int isRunning = 1;
    int numSeconds = 4;
    bool lazy = true;

    static int numBoids = 20;
    public static GameObject[] boids = new GameObject[numBoids];
    Coroutine co;

    public static Vector3 goal = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < numBoids; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-tankSize, tankSize), Random.Range(-tankSize, tankSize), Random.Range(-tankSize, tankSize));
            boids[i] = (GameObject)Instantiate(boidPrefab, pos, Quaternion.identity);
            lazyBtn.onClick.AddListener(lazyClicked);
            circleBtn.onClick.AddListener(userClicked);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check Which Mode is active
        if(lazy)
        {
            if (isRunning == 1)
            {
                co = StartCoroutine(LazyWait());
            }
        }
        else
        {
            // Check if buttons are pressed to move target
            if (goalPrefab.transform.position.y < 5)
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    goalPrefab.transform.Translate(Vector3.up * Time.deltaTime * 2.5f);
                    goal = goalPrefab.transform.position;
                }
            }
            if (goalPrefab.transform.position.y > -5)
            {
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    goalPrefab.transform.Translate(Vector3.down * Time.deltaTime * 2.5f);
                    goal = goalPrefab.transform.position;
                }
            }
            if (goalPrefab.transform.position.x < 5)
            {
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    goalPrefab.transform.Translate(Vector3.right * Time.deltaTime * 2.5f);
                    goal = goalPrefab.transform.position;
                }
            }
            if (goalPrefab.transform.position.x > -5)
            {
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    goalPrefab.transform.Translate(Vector3.left * Time.deltaTime * 2.5f);
                    goal = goalPrefab.transform.position;
                }
            }
        }
    }

    // Function for Lazy target position switching
    IEnumerator LazyWait()
    {
        isRunning = 0;
        yield return new WaitForSeconds(numSeconds);
        goal = new Vector3(Random.Range(-tankSize, tankSize), Random.Range(-tankSize, tankSize), Random.Range(-tankSize, tankSize));
        goalPrefab.transform.position = goal;
        isRunning = 1;
    }

    // Functions for UI buttons clicked
    void lazyClicked()
    {
        lazy = true;
    }

    void userClicked()
    {
        lazy = false;
        StopCoroutine(co);
        goal = new Vector3(0, 0, 0);
        goalPrefab.transform.position = goal;
    }
}
