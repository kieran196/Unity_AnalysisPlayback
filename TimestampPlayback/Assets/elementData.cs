using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class elementData : MonoBehaviour {

    public int elementID = -1;

    //Total collision points * 0.121f; - Should give an approx on the gaze time.
    public float totalCollisionPointsRaw, timeSpentLookingAtElement, dwellTime;

    public float dwellTimeNoThreshold; // For this dwell is considered for elements that are continually looked at for a period of time..
    public int dwellCountsGreaterThanThreshold, dwellCount;

    public float withinFOVTimer;


}
