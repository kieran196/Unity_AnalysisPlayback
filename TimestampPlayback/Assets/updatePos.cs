using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class updatePos : MonoBehaviour
{
    public Vector3 rootPos, rootRot;
    public bool initializePos;
    public bool debugMode = false;
    // Start is called before the first frame update
    void Start() {
        //initPos();
    }

    public void initPos() {
        if (initializePos) {
            this.transform.position = rootPos;
            this.transform.eulerAngles = rootRot;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
