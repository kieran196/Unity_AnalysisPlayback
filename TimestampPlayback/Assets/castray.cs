using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class castray : MonoBehaviour {
    [SerializeField]
    private floorDrawer drawer;
    [SerializeField]
    private cursorDwellCapturer dwellCapturer;
    public float timeLookingAtFloor = 0f;
    private float tempTimeLookingAtFloorThresh, tempTimeLookingAtFloor = 0f;
    [SerializeField]
    private dataReader datareader;
    [SerializeField]
    private GameObject locationTrackerRay;
    private bool lookingAtFloorReset = false;
    private string lastGameObjName = "";
    private string lastGameObjID = "";
    private string nextGameObjCheckedOff = "";
    //private string nextGameObjCheckedOffID = "";
    private GameObject lastFloorMapObj;
    private floorMapElement lastFloorMapEle;
    // Update is called once per frame
    //logFloorDwellData
    void Update() {
        locationTrackerRay.transform.position = this.transform.position;
        //Debug.DrawRay(this.transform.position, this.transform.forward, Color.red);
        Debug.DrawRay(locationTrackerRay.transform.position, -locationTrackerRay.transform.up, Color.green);
        RaycastHit hitt;
        if (Physics.Raycast(locationTrackerRay.transform.position, -locationTrackerRay.transform.up, out hitt)) {
            
            if ((hitt.transform.tag == "FloorMap" || hitt.transform.name.StartsWith("Floor")) && datareader.runAutomatically) {
                Debug.Log("Hitting:" + hitt.transform.name);
                Vector3 pos = hitt.point;
                if (!drawer.isInit) {
                    drawer.CreateBrush(new Vector3(pos.x, pos.y, pos.z));
                }
                drawer.AddPoint(new Vector3(pos.x, pos.y, pos.z));
            }
            if (hitt.transform.tag == "FloorMap" && datareader.runAutomatically) {
                if (hitt.transform.gameObject != lastFloorMapObj) {
                    lastFloorMapObj = hitt.transform.gameObject;
                    if (lastFloorMapObj.GetComponent<floorMapElement>() != null) {
                        lastFloorMapEle = lastFloorMapObj.GetComponent<floorMapElement>();
                    }
                }
                if (lastFloorMapEle != null) { // Start Logging..
                    lastFloorMapEle.floorHitCount++;
                    lastFloorMapEle.floorTimeCount += Time.deltaTime;
                }
            }
        }
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, this.transform.forward, out hit)) {
            if ((hit.transform.name.StartsWith("Floor") || hit.transform.tag == "FloorMap") && datareader.runAutomatically) {
                timeLookingAtFloor += Time.deltaTime;
                tempTimeLookingAtFloorThresh += Time.deltaTime;
                if (tempTimeLookingAtFloorThresh >= 0.5f) { // Greater than threshold
                    tempTimeLookingAtFloor += Time.deltaTime;
                    // Check the last looked at obj..
                    if (dwellCapturer.lastLookedAtGameObj != null && lookingAtFloorReset) {
                        lastGameObjName = dwellCapturer.lastLookedAtGameObj.name;
                        lastGameObjID = dwellCapturer.lastLookedAtGameObjID;
                        Debug.Log("Last looked at obj:" + dwellCapturer.lastLookedAtGameObj.name);
                        //nextGameObjCheckedOff = datareader.realtimeChecklistData[datareader.realtimeChecklistDataCounter].getEleName();
                        //datareader.realtimeChecklistData[datareader.realtimeChecklistDataCounter].getRefGameObject();
                        lookingAtFloorReset = false;
                    }
                }
            } else {
                if (!lookingAtFloorReset && lastGameObjName != "") { // Log data..
                    // Obj Name & Time Spent > 0.5
                    dataReader.logFloorDwellData.Add(tempTimeLookingAtFloor + "," + lastGameObjName + "," + lastGameObjID + "," + datareader.nextEleCheckedOff + "," + datareader.nextEleCheckedOffID);
                    //Debug.Log("Floor Dwelling Time:" + tempTimeLookingAtFloor + ", Last Obj:" + lastGameObjName);
                }
                tempTimeLookingAtFloorThresh = 0f;
                tempTimeLookingAtFloor = 0f;
                lookingAtFloorReset = true;
            }
        }
    }
}
