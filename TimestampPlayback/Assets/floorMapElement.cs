using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floorMapElement : MonoBehaviour {
    public int ID;
    public int floorHitCount = 0; // Represents the amount of ticks the user stood on this floor tile.
    public float floorTimeCount = 0; // Represents the total time the user stood on this floor tile.
    public WalkingDataReader colThreshold;

    public void updateCollisions() {
        if (floorHitCount >= colThreshold.GREEN && floorHitCount < (colThreshold.YELLOW - colThreshold.GREEN)) {
            changeMaterial(Color.green);
        } else if (floorHitCount >= (colThreshold.YELLOW - colThreshold.GREEN) && floorHitCount < colThreshold.YELLOW) {
            changeMaterial(new Color(0.5f, 1f, 0f, 1f));
        } else if (floorHitCount >= colThreshold.YELLOW && floorHitCount < (colThreshold.RED - colThreshold.YELLOW)) {
            changeMaterial(Color.yellow);
        } else if (floorHitCount >= (colThreshold.RED - colThreshold.YELLOW) && floorHitCount < colThreshold.RED) {
            changeMaterial(new Color(1f, 0.5f, 0f, 1f));
        } else if (floorHitCount >= colThreshold.RED) {
            changeMaterial(Color.red);
        }
    }

    private void changeMaterial(Color col) {
        Material mat = GetComponent<Renderer>().material;
        mat.color = new Color(col.r, col.g, col.b, 1f);
        GetComponent<Renderer>().material = mat;
    }

}
