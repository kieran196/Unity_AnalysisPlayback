using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class castray : MonoBehaviour {

    public GameObject cubePrefab;
    private GameObject hitObj;
    public Vector3 hitPoint;
    // Start is called before the first frame update
    void Start()
    {
        hitObj = Instantiate(cubePrefab);
    }

    // Update is called once per frame
    void Update() {
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, this.transform.forward, out hit)) {
            hitObj.transform.position = hit.point;
            hitPoint = hit.point;
        } else {
            hitPoint = Vector3.zero; //Not hitting anything..
        }
        
    }
}
