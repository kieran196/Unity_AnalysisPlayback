/*
 *  BendCast - Similar to a ray-cast except it will bend towards the closest object
 *  VR Interaction technique for the HTC Vive.
 *  
 *  Copyright(C) 2018  Ian Hanan
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 * 
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.If not, see<http://www.gnu.org/licenses/>.

 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class BendCast : MonoBehaviour
{
    public GameObject trackedObj;
    public GameObject lastSelectedObject; // holds the selected object

    public GameObject currentlyPointingAt;
    public Renderer currentlyPointingAtRender;
    private Vector3 castingBezierFrom;

    // Bend in ray is built from multiple other rays
    private int numOfLasers = 10; // how many rays to use for the bend (the more the smoother) MUST BE EVEN
    public GameObject laserPrefab;
    private GameObject[] lasers;
    private Transform[] laserTransform;

    private Vector3 p1PointLocation; // used fot the bezier curve

	public UnityEvent selectedObject; // Invoked when an object is selected

	public UnityEvent hovered; // Invoked when an object is hovered by technique
	public UnityEvent unHovered; // Invoked when an object is no longer hovered by the technique


#if SteamVR_Legacy
    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }
#endif
    private GameObject laserHolderGameobject;

    // Use this for initialization
    void Start() {

        // Initalizing all the lasers
        laserHolderGameobject = new GameObject();
        laserHolderGameobject.transform.parent = this.transform;
        laserHolderGameobject.gameObject.name = trackedObj.name + " Laser Rays";

        lasers = new GameObject[numOfLasers];
        laserTransform = new Transform[numOfLasers];
        for (int i = 0; i < numOfLasers; i++)
        {
            GameObject laserPart = Instantiate(laserPrefab, new Vector3((float)i, 1, 0), Quaternion.identity) as GameObject;
            laserTransform[i] = laserPart.transform;
            lasers[i] = laserPart;
            laserPart.transform.parent = laserHolderGameobject.transform;
        }
    }

    // Update is called once per frame
    void Update() {
        checkSurroundingObjects();
        if (currentlyPointingAt != null) {
            castLaserCurve();
        }
    }

    // Using a bezier! Idea from doing flexible pointer
    Vector3 GetBezierPosition(float t)
    {
        if (currentlyPointingAt == null)
        {
            // Fix for more appropriate later
            return new Vector3(0, 0, 0);
        }


        Vector3 p0 = castingBezierFrom;
        //Vector3 p2 = currentlyPointingAt.transform.position;
        Vector3 p2 = currentlyPointingAtRender.bounds.center;
        return Mathf.Pow(1f - t, 2f) * p0 + 2f * (1f - t) * t * p1PointLocation + Mathf.Pow(t, 2) * p2;
    }

    void castLaserCurve()
    {
        float valueToSearchBezierBy = 0f;
        Vector3 positionOfLastLaserPart = castingBezierFrom;

        valueToSearchBezierBy += (1f / numOfLasers);

        for (int i = 0; i < numOfLasers; i++)
        {
            lasers[i].SetActive(true);
            Vector3 pointOnBezier = GetBezierPosition(valueToSearchBezierBy);
            Vector3 nextPart = new Vector3(pointOnBezier.x, pointOnBezier.y, pointOnBezier.z);
            float distBetweenParts = Vector3.Distance(nextPart, positionOfLastLaserPart);

            laserTransform[i].position = Vector3.Lerp(positionOfLastLaserPart, nextPart, .5f);
            laserTransform[i].LookAt(nextPart);
            laserTransform[i].localScale = new Vector3(laserTransform[i].localScale.x, laserTransform[i].localScale.y, distBetweenParts);
            positionOfLastLaserPart = nextPart;

            valueToSearchBezierBy += (1f / numOfLasers);
        }
    }

    void checkSurroundingObjects() {
        if (currentlyPointingAt != null)
        {
            Vector3 forwardVectorFromRemote = trackedObj.transform.forward;
            Vector3 positionOfRemote = trackedObj.transform.position;
            //Vector3 forwardControllerToObject = trackedObj.transform.position - currentlyPointingAt.transform.position;
            Vector3 forwardControllerToObject = trackedObj.transform.position - currentlyPointingAtRender.bounds.center;

            float distanceBetweenRayAndPoint = Vector3.Magnitude(Vector3.Cross(forwardControllerToObject,forwardVectorFromRemote))/Vector3.Magnitude(forwardVectorFromRemote);

            Vector3 newPoint = new Vector3(forwardVectorFromRemote.x * distanceBetweenRayAndPoint + positionOfRemote.x, forwardVectorFromRemote.y * distanceBetweenRayAndPoint + positionOfRemote.y, forwardVectorFromRemote.z * distanceBetweenRayAndPoint + positionOfRemote.z);
            p1PointLocation = newPoint;
            // Activiating laser gameobject in case it isnt active
            laserHolderGameobject.SetActive(true);
            
            // Invoke un-hover if object with shortest distance is now different to currently hovered
            /*if(currentlyPointingAt != objectWithShortestDistance) {
                unHovered.Invoke();
            }

            // setting the object that is being pointed at
            currentlyPointingAt = objectWithShortestDistance;*/
            
            hovered.Invoke(); // Broadcasting that object is hovered

            castingBezierFrom = trackedObj.transform.position;

        } else if (currentlyPointingAt == null) {
            // Laser didnt reach any object so will disable
            laserHolderGameobject.SetActive(false);
            currentlyPointingAt = null;
            lastSelectedObject = null;
        }
    }
}
