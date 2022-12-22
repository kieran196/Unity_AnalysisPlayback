using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisions : MonoBehaviour {

    [SerializeField] // for testing
    private int collisionCount;
    private collisionsThreshold colThreshold;
    public float distanceFromCamAtSpawn;
    [SerializeField]
    private bool enableDebugging;
    private bool despawnHeatmap = true;
    /*private void OnTriggerStay(Collider other) {
        if (enableDebugging) {
            Debug.Log("Trigger stay:"+other.transform.name);
        }
    }*/
    public List<GameObject> cols = new List<GameObject>();
    private void OnTriggerEnter(Collider other) {
        if (other.transform.name == "Collider" && !other.transform.parent.Equals(this.gameObject)) {
            collisionCount++;
            updateCollisions(colThreshold.gradientBlending);
            if (enableDebugging) {
                //Debug.Log("Trigger enter:"+other.transform.name);
            }
        } else if (other.transform.tag == "BIM") {
            elementData eleData = other.GetComponent<elementData>();
            if (eleData == null) {
                eleData = other.transform.parent.GetComponent<elementData>();
            }
            if (eleData != null) {
                eleData.totalCollisionPointsRaw++;
                eleData.timeSpentLookingAtElement = eleData.totalCollisionPointsRaw * 0.121f;
            } else {
                //Debug.Log("Ele data null for obj:" + other.transform.name);
            }
            despawnHeatmap = false;
        }
    }

    private float despawnTimer;

    private void OnTriggerExit(Collider other) {
            if (other.transform.name == "Collider" && !other.transform.parent.Equals(this.gameObject)) {
            collisionCount--;
            updateCollisions(colThreshold.gradientBlending);
            if (enableDebugging) {
                Debug.Log("Trigger exit:"+other.transform.name);
            }
        }
    }

    void Start() {
        distanceFromCamAtSpawn = Vector3.Distance(this.transform.position, Camera.main.transform.position);
        colThreshold = this.transform.parent.GetComponent<collisionsThreshold>();
    }
    private float timer = 0f;
    void Update() { // Despawn after 1s
        if (despawnHeatmap) {
            timer += Time.deltaTime;
            if (timer > 0.1f) {
                Destroy(this.gameObject);
            }
        }
    }

    private float getGradientVal(float threshMin, float threshMax, bool invertCalc) {
        // Give us a range between 0 to 255 based on the threshold vals
        float x = threshMax - threshMin;
        float newThreshHold = 255f / threshMax;
        float calcGrad = x * newThreshHold;
        if (invertCalc) {
            calcGrad = 255 - calcGrad;
        }
        return calcGrad;
    }
    public float gradientBlendVal;
    public void updateCollisions(bool gradientBlend) {
        if (collisionCount >= colThreshold.GREEN && collisionCount < colThreshold.YELLOW) {
            gradientBlendVal = getGradientVal(colThreshold.GREEN, colThreshold.YELLOW, false);
            if (!gradientBlend) {
                changeMaterial(Color.green);
            } else {
                //0, 1, 0 = Green
                //1, 1, 0 = Yellow
                Color col = new Color((gradientBlendVal/255f), 1, 0, 1);
                changeMaterial(col);
            }
        } else if (collisionCount >= colThreshold.YELLOW && collisionCount < colThreshold.RED) {
            gradientBlendVal = getGradientVal(colThreshold.YELLOW, colThreshold.RED, true);
            if (!gradientBlend) {
                changeMaterial(Color.yellow);
            } else {
                //1, 1, 0 = Yellow
                //1, 0, 0 = Red
                Color col = new Color(1, (gradientBlendVal / 255f), 0, 1);
                changeMaterial(col);
            }
        } else if (collisionCount >= colThreshold.RED) {
            gradientBlendVal = 255f;
            changeMaterial(Color.red);
        }
    }

    /*public void updateCollisions(bool gradientBlend) {
        if (!gradientBlend) {
            if (collisionCount >= colThreshold.GREEN && collisionCount < (colThreshold.YELLOW - colThreshold.GREEN)) {
                changeMaterial(Color.green);
            } else if (collisionCount >= (colThreshold.YELLOW - colThreshold.GREEN) && collisionCount < colThreshold.YELLOW) {
                changeMaterial(new Color(0.5f, 1f, 0f, 1f));
                getGradientVal(colThreshold.GREEN, colThreshold.YELLOW);
            } else if (collisionCount >= colThreshold.YELLOW && collisionCount < (colThreshold.RED - colThreshold.YELLOW)) {
                changeMaterial(Color.yellow);
            } else if (collisionCount >= (colThreshold.RED - colThreshold.YELLOW) && collisionCount < colThreshold.RED) {
                changeMaterial(new Color(1f, 0.5f, 0f, 1f));
            } else if (collisionCount >= colThreshold.RED) {
                changeMaterial(Color.red);
            }
        } else { // Gradient Blending Mode Enabled..

        }
    }*/

    private void changeMaterial(Color col) {
        Material mat = GetComponent<Renderer>().material;
        mat.color = col;
        GetComponent<Renderer>().material = mat;
    }
}
