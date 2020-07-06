using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class storeData : MonoBehaviour {

    private string timeStamp;
    private Vector3 position;

    public void setTimeStamp(string timeStamp) {
        this.timeStamp = timeStamp;
    }

    public void setPosition(Vector3 position) {
        this.position = position;
    }

    public string getTimeStamp() {
        return timeStamp;
    }

    public Vector3 getPosition() {
        return position;
    }

}
