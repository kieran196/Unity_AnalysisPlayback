using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredictedHitpoint : MonoBehaviour{
    [SerializeField]
    private dataReader dataReader;
    [SerializeField]
    private GameObject fHitObj;
    private Vector3 hitPoint;

    [SerializeField]
    private Transform cursor, userHead;
    [SerializeField]
    private Transform newCursor;
    void Start() {
        cursor = this.transform.parent;
        this.transform.SetParent(null);
        newCursor.transform.SetParent(null);
    }

    [SerializeField]
    private LayerMask ignoredLayer;

    private GameObject castRay(Vector3 direction) {
        GameObject hitObj = null;
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, direction, out hit, Mathf.Infinity, ignoredLayer)) {
            //Debug.Log("Hit:" + hit.transform.name + ", tag: " + hit.transform.gameObject.layer);
            if (hit.transform.tag == "BIM") {
                hitObj = hit.transform.gameObject;
                hitPoint = hit.point;
                if (!dataReader.data[(int)dataReader.slider.value].isHeatMapCreated()) {
                    dataReader.heatmapGenerator(hitPoint, cursor.transform.eulerAngles, hitObj);
                    dataReader.data[(int)dataReader.slider.value].setHeatMapCreated(true);
                }
            }
        }
        return hitObj;
    }
    // Update is called once per frame
    void Update() {
        //this.transform.position = userHead.transform.position;
        this.transform.position = cursor.transform.position + cursor.transform.forward;
        this.transform.eulerAngles = cursor.transform.eulerAngles;
        Debug.DrawRay(this.transform.position, -cursor.transform.forward, Color.blue);
        fHitObj = castRay(-cursor.transform.forward); // Forwards
        if (fHitObj == null) {
            newCursor.transform.position = cursor.transform.position;
        } else {
            newCursor.transform.position = hitPoint;
        }
        newCursor.transform.eulerAngles = cursor.transform.eulerAngles;
        //Debug.DrawRay(cursor.transform.position, cursor.transform.forward, Color.cyan);
        //bHitObj = castRay(cursor.transform.forward); // Backwards
        //dataReader.heatmapGenerator();
    }
}
