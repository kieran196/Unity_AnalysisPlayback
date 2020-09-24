using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class dataReader : MonoBehaviour
{
    public GameObject cubeLocalPlaybackObj;
    public GameObject head;
    public Slider slider;
    public Text timeStampText;
    public string importFile = "";
    private string DIR = "Logs/";
    private bool dataInitialized = false;
    [SerializeField]
    private int heatMapCount = 0;
    private Vector3 newHeadPos;
    private Vector3 newHeadRot;
    [SerializeField]
    private bool runAutomatically = false;
    [Tooltip("Change the speed of the playback. (1 = realtime)")]
    [SerializeField]
    private float speedIncreased = 1f;
    private float realtimeTimer = 0f;
    private void Update() {
        if (dataInitialized) {
            setCube((int)slider.value);
        } if (runAutomatically) {
            realtimeTimer += (Time.deltaTime*offset)*speedIncreased;
            slider.value = realtimeTimer;
            if (realtimeTimer >= (slider.maxValue-1f)) {
                runAutomatically = false;
                realtimeTimer = 0f;
                slider.value = 0f;
            }
        }
    }

    public void setCube(int slider) {
        Debug.Log("At pos:" + slider + " | User Pos:" + data[slider].getPosition() + " | User Rot::" + data[slider].getEulerAngles());
        timeStampText.text = "Timestamp = "+ data[slider].getTimeStamp();
        cubeLocalPlaybackObj.transform.position = data[slider].getPosition();
        cubeLocalPlaybackObj.transform.eulerAngles = data[slider].getEulerAngles();
        head.transform.position = data[slider].getHeadPosition();
        head.transform.localEulerAngles = data[slider].getEulerAngles();
        if (data[slider].isHeatMapCreated()) {
            // Do something here?
        } else {
            data[slider].setHeatMapCreated(true);
            heatmapGenerator(data[slider].getPosition(), data[slider].getEulerAngles(), data[slider].getHeatMapRef());
        }
    }

    // Start is called before the first frame update
    void Start() {
        cubeLocalPlaybackObj.SetActive(true);
        readData();
    }

    
    public GameObject heatmapPrefab;
    public Transform heatmapParent;
    public void heatmapGenerator(Vector3 pos, Vector3 rot, GameObject heatmapRef) {
        heatmapRef = Instantiate(heatmapPrefab);
        heatmapRef.transform.position = pos;
        heatmapRef.transform.eulerAngles = rot;
        heatmapRef.transform.SetParent(heatmapParent.transform);
        heatMapCount++;
    }

    public int getLines(StreamReader reader) {
        int count = 0;
        while (reader.ReadLine() != null) {
            count++;
        }
        return count;
    }
    storeData[] data = null;
    public float offset;
    private int count = 0;
    public float overallEyeTrackingTime = 0f;
    public void readData() {
        using (var reader = new StreamReader(@DIR + importFile + ".csv")) {
            int lineCount = getLines(new StreamReader(@DIR + importFile + ".csv"));
            slider.maxValue = lineCount;
            Debug.Log("Lines:" + lineCount);
            data = new storeData[lineCount];
            count = 0;
            while (!reader.EndOfStream) {
                var line = reader.ReadLine();
                string[] split = line.Split(',');
                if (count >= 1) {
                    //Debug.Log(data[count-1]);
                    if (split.Length > 3) {
                        data[count - 1] = new storeData();
                        data[count-1].setTimeStamp(split[0]);
                        data[count-1].setPosition(new Vector3(float.Parse(split[1]), float.Parse(split[2]), float.Parse(split[3])));
                        data[count-1].setEulerAngles(new Vector3(float.Parse(split[4]), float.Parse(split[5]), float.Parse(split[6])));
                        data[count-1].setHeadPosition(new Vector3(float.Parse(split[7]), float.Parse(split[8]), float.Parse(split[9])));
                        data[count-1].setHeadEulerAngles(new Vector3(float.Parse(split[10]), float.Parse(split[11]), float.Parse(split[12])));
                        //Debug.Log("Timestamp:" + split[0] + ", xL:" + split[1] + ", yL:" + split[2] + ", zL" + split[3] + ", xR:" + split[4] + ", yR:" + split[5] + ", zR" + split[6]);
                    }
                }
                count++;
            }
        }
        overallEyeTrackingTime = float.Parse(data[count-2].getTimeStamp());
        offset = count / overallEyeTrackingTime;
        dataInitialized = true;
    }

}
