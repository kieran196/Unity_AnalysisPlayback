using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class floorMapElement : MonoBehaviour {
    public int ID;
    public int floorHitCount = 0; // Represents the amount of ticks the user stood on this floor tile.
    public float floorTimeCount = 0; // Represents the total time the user stood on this floor tile.
    public WalkingDataReader colThreshold;
    public string textVal;
    [SerializeField]
    private Text label;
    public void updateCollisions() {

        if (floorTimeCount >= colThreshold.GREEN && floorTimeCount < colThreshold.YELLOW) {
            changeMaterial(Color.red);
        } else if (floorTimeCount >= colThreshold.YELLOW && floorTimeCount < colThreshold.RED) {
            //changeMaterial(new Color(0.5f, 1f, 0f, 1f));
            changeMaterial(Color.yellow);
        } else if (floorTimeCount >= colThreshold.RED) {
            changeMaterial(Color.green);
        }

        /*if (floorHitCount >= colThreshold.GREEN && floorHitCount < (colThreshold.GREEN - colThreshold.YELLOW)) {
            changeMaterial(Color.green);
            textVal = "Very Good";
            return;
        } else if (floorHitCount >= (colThreshold.GREEN - colThreshold.YELLOW) && floorHitCount < colThreshold.YELLOW) {
            changeMaterial(new Color(0.5f, 1f, 0f, 1f));
            textVal = "Good";
            return;
        } else if (floorHitCount >= colThreshold.YELLOW && floorHitCount < (colThreshold.YELLOW - colThreshold.RED)) {
            changeMaterial(Color.yellow);
            textVal = "Neutral";
            return;
        } else if (floorHitCount >= (colThreshold.YELLOW - colThreshold.RED) && floorHitCount < colThreshold.RED) {
            changeMaterial(new Color(1f, 0.5f, 0f, 1f));
            textVal = "Bad";
            return;
        } else if (floorHitCount >= colThreshold.RED) {
            changeMaterial(Color.red);
            textVal = "Very Bad";
            return;
        }*/
    }

    private void changeMaterial(Color col) {
        Material mat = GetComponent<Renderer>().material;
        mat.color = new Color(col.r, col.g, col.b, 1f);
        GetComponent<Renderer>().material = mat;
        GetComponent<Renderer>().material.SetColor("_Color", col);
    }

    private void Start() {
        label = this.transform.GetChild(0).GetComponentInChildren<Text>();
        label.fontSize = 300;
    }

    public void changeTextVisibility() {
        label.gameObject.SetActive(!label.gameObject.activeInHierarchy);
    }

    private void Update() {
        label.text = floorTimeCount.ToString();
        updateCollisions(); // Temp.. comment me out later
    }

}
