using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionsThreshold : MonoBehaviour {

    public float GREEN, YELLOW, RED;
    public bool applyHeatmapChanges;
    void Update() {
        if (applyHeatmapChanges) {
            applyHeatmapChanges = false;
            foreach (Transform child in this.transform) {
                child.GetComponent<collisions>().updateCollisions();
            }
        }
    }

}
