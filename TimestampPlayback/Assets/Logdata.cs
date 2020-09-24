using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logdata : MonoBehaviour {
    public csvWriter writer;
    public float time_elapsed = 0f;
    public castray[] cursorObjs;

    public void logData() {
        float xL = cursorObjs[0].hitPoint.x;
        float yL= cursorObjs[0].hitPoint.y;
        float zL= cursorObjs[0].hitPoint.z;
        float xR = cursorObjs[1].hitPoint.x;
        float yR = cursorObjs[1].hitPoint.y;
        float zR = cursorObjs[1].hitPoint.z;
        writer.WriteLine(time_elapsed + ","+xL+","+yL+","+zL + "," + xR + "," + yR + "," + zR);
    }

    // Update is called once per frame
    void Update() {
        Debug.Log("Cubes:" + GameObject.FindGameObjectsWithTag("Player").Length);
        if (cursorObjs.Length >= 2) {
            time_elapsed += Time.deltaTime; // Get the overall time the app is running.
            logData(); // Logged once per frame.
        } else {
            cursorObjs = FindObjectsOfType<castray>();
        }
    }
}
