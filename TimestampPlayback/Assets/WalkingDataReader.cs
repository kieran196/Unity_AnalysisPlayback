using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WalkingDataReader : MonoBehaviour {

    public bool updateHeatMap = false;
    public bool updateTextVis = false;
    public float GREEN,YELLOW,RED;

    public bool walkingFloorMapperEnabled;
    [SerializeField]
    private GameObject floorMapParent;
    private GameObject[] floorTiles;
    // Start is called before the first frame update
    void Start() {
        importFloorData(); // Comment me out
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
                floorMapEle.floorTimeCount = (float)decimal.Round(decimal.Parse(floorDatArr[count-1]), 2);
                floorTile.transform.name = count.ToString();
                count++;
                floorMapEle.colThreshold = this;
            }
        }
    }
    [SerializeField]
    private string[] floorDatArr;
    public void importFloorData() {
        floorDatArr = new string[75];
        int count = 0;
        string dir = @"D:\GitHub\Unity_AnalysisPlayback\TimestampPlayback\Logs\floorDat.txt";
        foreach (string line in System.IO.File.ReadLines(dir)) {
            floorDatArr[count] = line;
            count++;
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
        if (updateTextVis) {
            updateHeatMap = false;
            foreach (GameObject floorTile in floorTiles) {
                floorTile.GetComponent<floorMapElement>().changeTextVisibility();
            }
        }
        if (updateHeatMap) {
            updateHeatMap = false;
            foreach (GameObject floorTile in floorTiles) {
                floorTile.GetComponent<floorMapElement>().updateCollisions();
                //floorMapElement floorTileEle = floorTile.GetComponent<floorMapElement>();
            }
           
        }
    }
}
