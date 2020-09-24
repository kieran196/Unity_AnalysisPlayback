using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class storeData : MonoBehaviour {

    private string timeStamp;
    private Vector3 position;
    private Vector3 eulerAngles;
    private Vector3 Headposition;
    private Vector3 HeadeulerAngles;
    private bool heatmapCreated = false;

    private GameObject heatmapRef;

    public void setHeatMapRef(GameObject heatmapRef) {
        this.heatmapRef = heatmapRef;
    }

    public GameObject getHeatMapRef() {
        return heatmapRef;
    }

    public bool isHeatMapCreated() {
        return heatmapCreated;
    }

    public void setHeatMapCreated(bool created) {
        heatmapCreated = created;
    }

    public void setTimeStamp(string timeStamp) {
        this.timeStamp = timeStamp;
    }

    public void setHeadPosition(Vector3 Headposition) {
        this.Headposition = Headposition;
    }

    public void setHeadEulerAngles(Vector3 HeadeulerAngles) {
        this.HeadeulerAngles = HeadeulerAngles;
    }

    public Vector3 getHeadPosition() {
        return Headposition;
    }

    public Vector3 getHeadEulerAngles() {
        return HeadeulerAngles;
    }
    
    public void setPosition(Vector3 position) {
        this.position = position;
    }

    public void setEulerAngles(Vector3 eulerAngles) {
        this.eulerAngles = eulerAngles;
    }

    public string getTimeStamp() {
        return timeStamp;
    }

    public Vector3 getPosition() {
        return position;
    }

    public Vector3 getEulerAngles() {
        return eulerAngles;
    }
}
