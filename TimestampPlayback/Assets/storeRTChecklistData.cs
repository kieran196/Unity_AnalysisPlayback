using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class storeRTChecklistData : MonoBehaviour {

    private string timeStamp, eleName, eleID, checklistValue;

    private GameObject refGameObject;

    public bool isBendcastAssigned, isAssigned;

    public void setRefGameObject(GameObject refGameObject) {
        this.refGameObject = refGameObject;
    }

    public GameObject getRefGameObject() {
        return refGameObject;
    }

    public void setTimeStamp(string timeStamp) {
        this.timeStamp = timeStamp;
    }

    public string getTimeStamp() {
        return timeStamp;
    }

    public void setEleName(string eleName) {
        this.eleName = eleName;
    }

    public string getEleName() {
        return eleName;
    }

    public void setEleID(string eleID) {
        this.eleID = eleID;
    }

    public string getEleID() {
        return eleID;
    }

    public void setChecklistValue(string checklistValue) {
        this.checklistValue = checklistValue;
    }

    public string getChecklistValue() {
        return checklistValue;
    }
}
