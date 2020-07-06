using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logdata : MonoBehaviour {
    public csvWriter writer;

    public float time_elapsed = 0f;

    public void logData() {
        float x = GetComponent<castray>().hitPoint.x;
        float y= GetComponent<castray>().hitPoint.y;
        float z= GetComponent<castray>().hitPoint.z;
        writer.WriteLine(time_elapsed + ","+x+","+y+","+z);
    }

    // Update is called once per frame
    void Update()
    {
        time_elapsed += Time.deltaTime; // Get the overall time the app is running.
        logData(); // Logged once per frame.
    }
}
