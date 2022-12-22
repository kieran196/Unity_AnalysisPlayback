using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floorDrawer : MonoBehaviour
{
    public float Y_OFFSET = 0.05f;
    private Vector3 lastPos;
    public bool isInit = false;
    private LineRenderer currentLineRenderer;
    public void CreateBrush(Vector3 pos) {
        currentLineRenderer = GetComponent<LineRenderer>();
        currentLineRenderer.SetPosition(0, pos);
        currentLineRenderer.SetPosition(1, pos);
        isInit = true;
    }
    public void AddPoint(Vector3 point) {
        if (isInit) {
            Debug.Log("Drawing at point:" + point);
            if (point != lastPos) {
                lastPos = point;
                currentLineRenderer.positionCount++;
                int index = currentLineRenderer.positionCount - 1;
                currentLineRenderer.SetPosition(index, new Vector3(point.x, point.y+Y_OFFSET, point.z));
            }
        }
    }
}
