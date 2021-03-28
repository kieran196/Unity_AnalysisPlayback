using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class castray : MonoBehaviour {
    public float timeLookingAtFloor = 0f;
    [SerializeField]
    private dataReader datareader;
    [SerializeField]
    private GameObject locationTrackerRay;
    // Update is called once per frame
    void Update() {
        locationTrackerRay.transform.position = this.transform.position;
        //Debug.DrawRay(this.transform.position, this.transform.forward, Color.red);
        //Debug.DrawRay(locationTrackerRay.transform.position, -locationTrackerRay.transform.up, Color.green);
        RaycastHit hitt;
        if (Physics.Raycast(locationTrackerRay.transform.position, -locationTrackerRay.transform.up, out hitt)) {
            
        }
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, this.transform.forward, out hit)) {
            if (hit.transform.name.StartsWith("Floor") && datareader.runAutomatically) {
                timeLookingAtFloor += Time.deltaTime;
            }
        }
    }
}
