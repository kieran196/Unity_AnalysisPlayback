using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cursorDwellCapturer : MonoBehaviour {
    public bool calculatePredictedHitpoint;
    [SerializeField]
    private dataReader dataReader;
    public float DISTANCE_VALUE;
    public float DWELL_THRESHALD;
    public float dwellTimer;
    public float dwellTimerWThreshald;

    //private float getLastDwellDistance = 0f;
    private Vector3 getLastDwellPosition;
    private bool isInitialized = false;

    public GameObject collisionObj;
    public elementData collisionObjElementData;

    public GameObject lastLookedAtGameObj;
    public string lastLookedAtGameObjID;
    private void OnTriggerStay(Collider other) {
        if (other.tag == "BIM" && other.gameObject != collisionObj) {
            collisionObj = other.gameObject;
            if (collisionObj.GetComponent<elementData>() != null) {
                collisionObjElementData = collisionObj.GetComponent<elementData>();
            } else {
                collisionObjElementData = collisionObj.transform.parent.GetComponent<elementData>();
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "BIM") {
            lastLookedAtGameObj = other.gameObject;
            if (other.GetComponent<elementData>() != null) {
                lastLookedAtGameObjID = other.GetComponent<elementData>().elementID.ToString();
            } else {
                //Debug.Log(other.transform.name);
                lastLookedAtGameObjID = other.transform.parent.GetComponent<elementData>().elementID.ToString();
            }
            
        }
    }

    private void OnTriggerExit(Collider other) {
        collisionObj = null;
        collisionObjElementData = null;
    }
    public float lastDwellDistGreaterThanThresh;
    void Update() {
        //Debug.DrawRay(this.transform.position, this.transform.forward, Color.red);
        //Debug.DrawRay(this.transform.position, -this.transform.forward, Color.blue);
        if (dataReader.runAutomatically || dataReader.debuggingInEdit) {
            if (!isInitialized) {
                isInitialized = true;
                getLastDwellPosition = this.transform.position;
            }
            lastDwellDistGreaterThanThresh = Vector3.Distance(this.transform.position, getLastDwellPosition);
            if (Vector3.Distance(this.transform.position, getLastDwellPosition) <= DISTANCE_VALUE) {
                //lastDwellDistGreaterThanThresh = 0f;
                dwellTimer += Time.deltaTime;
                if (dwellTimer >= DWELL_THRESHALD) {
                    if (dwellTimerWThreshald == 0) {
                        // Here..
                        if (collisionObjElementData != null) {
                            collisionObjElementData.dwellCount++;
                            //Debug.Log("Increased dwell count:" + collisionObjElementData.dwellCount);
                        }
                    }
                    dwellTimerWThreshald += Time.deltaTime;
                    // Start logging dwell
                    if (collisionObjElementData != null) {
                        collisionObjElementData.dwellTime += Time.deltaTime;
                        collisionObjElementData.dwellCountsGreaterThanThreshold++;
                    }
                }
            } else {
                dwellTimer = 0f; // Reset just to make it eaiser for now.
                dwellTimerWThreshald = 0f;
                getLastDwellPosition = this.transform.position;
            }
        }
    }
}
