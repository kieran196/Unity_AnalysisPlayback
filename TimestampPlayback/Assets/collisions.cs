using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisions : MonoBehaviour {

    [SerializeField] // for testing
    private int collisionCount;
    [SerializeField]
    private bool enableDebugging;
    /*private void OnTriggerStay(Collider other) {
        if (enableDebugging) {
            Debug.Log("Trigger stay:"+other.transform.name);
        }
    }*/
    private void OnTriggerEnter(Collider other) {
        if (other.transform.name == "Collider" && !other.transform.parent.Equals(this.gameObject)) {
            collisionCount++;
            updateCollisions();
            if (enableDebugging) {
                Debug.Log("Trigger enter:"+other.transform.name);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
            if (other.transform.name == "Collider" && !other.transform.parent.Equals(this.gameObject)) {
            collisionCount--;
            updateCollisions();
            if (enableDebugging) {
                Debug.Log("Trigger exit:"+other.transform.name);
            }
        }
    }

    private void updateCollisions() {
        if (collisionCount == 1) {
            changeMaterial(Color.green);
        } else if (collisionCount == 2) {
            changeMaterial(new Color(0.5f, 1f, 0f, 1f));
        } else if (collisionCount == 3) {
            changeMaterial(Color.yellow);
        } else if (collisionCount >= 4) {
            changeMaterial(Color.red);
        }
    }

    private void changeMaterial(Color col) {
        Material mat = GetComponent<Renderer>().material;
        mat.color = col;
        GetComponent<Renderer>().material = mat;
    }
}
