using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingDataReader : MonoBehaviour {

    public bool updateHeatMap = false;
    public float GREEN,YELLOW,RED;

    public bool walkingFloorMapperEnabled;
    [SerializeField]
    private GameObject floorMapParent;
    private GameObject[] floorTiles;
    // Start is called before the first frame update
    void Start() {
        dataReader.floorMapData.Add("FloorTileID, FloorHitCount, FloorTimeCount");
        if (!walkingFloorMapperEnabled) {
            floorMapParent.SetActive(false);
        } else {
            floorMapParent.SetActive(true);
            floorTiles = GameObject.FindGameObjectsWithTag("FloorMap");
            int count = 1;
            foreach (GameObject floorTile in floorTiles) {
                floorMapElement floorMapEle = floorTile.AddComponent<floorMapElement>();
                floorMapEle.ID = count;
                count++;
                floorMapEle.colThreshold = this;
            }
        }
    }

    public void logFloorMapData() {
        foreach (GameObject floorTile in floorTiles) {
            floorMapElement floorTileEle = floorTile.GetComponent<floorMapElement>();
            dataReader.floorMapData.Add(floorTileEle.ID + "," +floorTileEle.floorHitCount+ "," + floorTileEle.floorTimeCount);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (updateHeatMap) {
            updateHeatMap = false;
            foreach (GameObject floorTile in floorTiles) {
                floorTile.GetComponent<floorMapElement>().updateCollisions();
                //floorMapElement floorTileEle = floorTile.GetComponent<floorMapElement>();
            }
           
        }
    }
}
