using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[ExecuteInEditMode]
public class getDist : MonoBehaviour
{
    public float dist;
    // Update is called once per frame
    void Update() {
        if (this.transform.parent != null) {
            dist = Vector3.Distance(this.transform.position, this.transform.parent.position);
        }
    }
}
