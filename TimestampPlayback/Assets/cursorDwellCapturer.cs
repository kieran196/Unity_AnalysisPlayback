using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cursorDwellCapturer : MonoBehaviour {
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
    private void OnTriggerStay(Collider other) {
        if (other.tag == "BIM" && other.gameObject != collisionObj) {
            collisionObj = other.gameObject;
            if (collisionObj.GetComponent<elementData>() != null) {
                collisionObjElementData = collisionObj.GetComponent<elementData>();
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        collisionObj = null;
        collisionObjElementData = null;
    }
    void Update() {
        if (dataReader.runAutomatically) {
            if (!isInitialized) {
                isInitialized = true;
                getLastDwellPosition = this.transform.position;
            }
            if (Vector3.Distance(this.transform.position, getLastDwellPosition) <= DISTANCE_VALUE) {
                dwellTimer += Time.deltaTime;
                if (dwellTimer >= DWELL_THRESHALD) {
                    dwellTimerWThreshald += Time.deltaTime;
                    // Start logging dwell
                    if (collisionObjElementData != null) {
                        collisionObjElementData.dwellTime += Time.deltaTime;
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
