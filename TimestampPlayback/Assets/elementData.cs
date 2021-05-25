using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class elementData : MonoBehaviour {

    public int elementID = -1;

    //Total collision points * 0.121f; - Should give an approx on the gaze time.
    public float totalCollisionPointsRaw, timeSpentLookingAtElement, dwellTime;
    public int dwellCountsGreaterThanThreshold, dwellCount;



}
