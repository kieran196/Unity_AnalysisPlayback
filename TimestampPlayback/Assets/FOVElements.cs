using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVElements : MonoBehaviour
{
    public dataReader dataReader;
    [SerializeField]
    private List<GameObject> objsWithinFov = new List<GameObject>();
    public void getFOVObjs() {
        objsWithinFov = new List<GameObject>();
        //List<GameObject> renderedObjs = new List<GameObject>();
        for (int i = 0; i < dataReader.eleDataList.Count; i++) {
            Renderer objRenderer = dataReader.eleDataList[i].GetComponent<Renderer>();
            if (objRenderer == null) {
                objRenderer = dataReader.eleDataList[i].transform.parent.GetComponent<Renderer>();
            }
            bool withinFov = withinFOV(objRenderer.bounds.center);
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 toOther = objRenderer.bounds.center - transform.position;
            float distance = Vector3.Distance(objRenderer.bounds.center, transform.position);
            if (withinFov) {
                // Within FOV..
                objsWithinFov.Add(objRenderer.gameObject);
                dataReader.eleDataList[i].withinFOVTimer += Time.deltaTime;
            }
        }
    }
    [SerializeField]
    private float FOV_VARIABLE;
    private bool withinFOV(Vector3 other) {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 toOther = other - transform.position;
        float dot = Vector3.Dot(forward, toOther);
        float distance = Vector3.Distance(forward, toOther);
        //Debug.Log(Vector3.Dot(forward, toOther));
        if ((distance-dot) < FOV_VARIABLE) {
            return true;
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        getFOVObjs();
    }
}
