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
    private string DIRET = "eyetracking/";
    private string DIRCL = "checklist/realtime/";
    private string DIRCLP = "checklist/post/";
    private bool dataInitialized = false;
    [SerializeField]
    private int heatMapCount = 0;
    private Vector3 newHeadPos;
    private Vector3 newHeadRot;
    public castray floorRay;
    [SerializeField]
    public bool runAutomatically = false;
    [Tooltip("Change the speed of the playback. (1 = realtime)")]
    [SerializeField]
    private float speedIncreased = 1f;
    [SerializeField]
    private float realtimeTimer, realtimeTimerConverted = 0f;
    public int realtimeChecklistDataCounter = 0;

    public GameObject[] elementsByID;
    [SerializeField]
    private BendCast bendcast;
    [SerializeField]
    private WalkingDataReader walkingDataReader;

    private void logData() {
        Debug.Log("ParticipantData:" + importFile + ", Time Looking At Floor:" + floorRay.timeLookingAtFloor + ", Overall Eye Tracking Time:" + overallEyeTrackingTime);
        logElementData.Add("Participant, Time Looking At Floor, Overall Eye Tracking Time");
        logElementData.Add(importFile + "," + floorRay.timeLookingAtFloor + "," + overallEyeTrackingTime);
        logElementData.Add("");
        logElementData.Add("Element ID, Total Collision Points, Time Spent Looking At Element, Dwell Timer on Elements (0.5s threshold), Dwell Count Of Elements (0.5s threshold), ERROR");
        foreach (GameObject element in elementsByID) {
            elementData ele = element.GetComponent<elementData>();
            Debug.Log("Element = " + element.name + " , " + ele);
            Debug.Log("Element ID = " + ele.elementID + " , TotalCollisionPoints = " + ele.totalCollisionPointsRaw + " , TimeSpentLookingAtElement = " + ele.timeSpentLookingAtElement + " , DwellTimer = " + ele.dwellTime);
            logElementData.Add(ele.elementID + "," + ele.totalCollisionPointsRaw + "," + ele.timeSpentLookingAtElement + "," + ele.dwellTime + "," + ele.dwellCount);
        }
        logParticipantData(logElementData, "outputa.csv");
        logParticipantData(logFloorDwellData, "outputb.csv");

        walkingDataReader.logFloorMapData();
        logParticipantData(floorMapData, "outputc.csv");
    }

    private List<string> logElementData = new List<string>();
    public static List<string> logFloorDwellData = new List<string>();
    public static List<string> floorMapData = new List<string>();
    private void logParticipantData(List<string> dataToLog, string outputFileName) {
        string dest = System.IO.Path.Combine(Application.dataPath, outputFileName);
        StreamWriter writer = null;
        int count = 1;
        bool fileExists = File.Exists(dest);
        if (fileExists) {
            dest = System.IO.Path.Combine(Application.dataPath, outputFileName);
            count++;
            Debug.Log(dest + " already exists - Failed to log data.");
            writer = new StreamWriter(dest, false) as StreamWriter;
        } else {
            print("Found path:" + dest);
            writer = new StreamWriter(dest, true) as StreamWriter;
        }
        print("File exists?" + fileExists);
        for (int i = 0; i < dataToLog.Count; i++) {
            //print(logInfo[i]);
            writer.Write(dataToLog[i]);
            writer.WriteLine();
        }
        print("Writen to file:" + dest);
        writer.Close();
    }

    private bool isARBased = false; // Determine which root gameobject to start with
    [SerializeField]
    private GameObject[] rootObjs; // AR = [0], PB = [1]
    [SerializeField]
    private Text importFileText;
    [SerializeField]
    private Text conditionText;
    public void playApp() {
        runAutomatically = true;
    }

    public void pauseApp() {
        runAutomatically = false;
    }
    public void startApp() {
        if (!isUnityEditor) {
            logElementData = new List<string>();

            importFile = importFileText.text;
            if (conditionText.text == "AR") {
                rootObjs[0].SetActive(true);
                rootObjs[1].SetActive(false);
            } else {
                rootObjs[0].SetActive(false);
                rootObjs[1].SetActive(true);
            }
            allChecklistElementsUnsrted = GameObject.FindGameObjectsWithTag("BIM");
            elementsByID = new GameObject[30];
            readPTChecklistData();
            root = FindObjectOfType<updatePos>();
            Debug.Log("Found:" + root.transform.name);
            cubeLocalPlaybackObj.SetActive(true);
            readData();
            readRTChecklistData();
        }
    }

    public void stopApp() {

    }

    public string nextEleCheckedOff = "";
    public string nextEleCheckedOffID = "";

    private void Update() {
        //Debug.Log("RT Timestamp:"+realtimeChecklistData[0].getTimeStamp());

        if (dataInitialized) {
            setCube((int)slider.value);
        } if (runAutomatically) {
            realtimeTimer += (Time.deltaTime*offset)*speedIncreased;
            realtimeTimerConverted = realtimeTimer * 0.121f;
            slider.value = realtimeTimer;
            if (realtimeTimer >= (slider.maxValue-5f)) {
                runAutomatically = false;
                logData();
                realtimeTimer = 0f;
                slider.value = 0f;
            }
        }
        if (realtimeChecklistData != null && runAutomatically) {
            //if (realtimeChecklistData[realtimeChecklistDataCounter] == null) {
            //    return;
            //}
            int nextChecklistTimerVal = (int)float.Parse(realtimeChecklistData[realtimeChecklistDataCounter].getTimeStamp());
            int currTimeStamp = (int)float.Parse(data[(int)slider.value].getTimeStamp());
            if (nextChecklistTimerVal == currTimeStamp) {
                Debug.Log("Participant checked ele:" + realtimeChecklistData[realtimeChecklistDataCounter].getEleName() + " to: " + realtimeChecklistData[realtimeChecklistDataCounter].getChecklistValue());
                string checklistVal = realtimeChecklistData[realtimeChecklistDataCounter].getChecklistValue();
                GameObject gameObjRef = realtimeChecklistData[realtimeChecklistDataCounter].getRefGameObject();
                realtimeChecklistDataCounter++;
                /*if (checklistVal == "correct") {
                    gameObjRef.GetComponent<Renderer>().material.color = Color.green;
                } else if (checklistVal == "other") {
                    gameObjRef.GetComponent<Renderer>().material.color = Color.yellow;
                } else if (checklistVal == "incorrect") {
                    gameObjRef.GetComponent<Renderer>().material.color = Color.red;
                }*/
            } else if (currTimeStamp != 0) {
                // Next Element To Be Checked Off
                nextEleCheckedOff = realtimeChecklistData[realtimeChecklistDataCounter].getEleName();
                nextEleCheckedOffID = realtimeChecklistData[realtimeChecklistDataCounter].getEleID();
                //Debug.Log("Next Ele Checked Off:"+realtimeChecklistData[realtimeChecklistDataCounter].getEleName());
                /*if (!realtimeChecklistData[realtimeChecklistDataCounter].isBendcastAssigned) {
                    bendcast.currentlyPointingAtRender = realtimeChecklistData[realtimeChecklistDataCounter].getRefGameObject().GetComponent<Renderer>();
                    bendcast.currentlyPointingAt = realtimeChecklistData[realtimeChecklistDataCounter].getRefGameObject();
                    bendcast.currentlyPointingAtRender.material.color = Color.blue;
                    realtimeChecklistData[realtimeChecklistDataCounter].isBendcastAssigned = true;
                }*/
            }
        }
    }

    public void setCube(int slider) {
        if (root.debugMode) {
            Debug.Log("At pos:" + slider + " | User Pos:" + data[slider].getPosition() + " | User Rot::" + data[slider].getEulerAngles());
        }
        
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

    private GameObject findChecklistID(string eleName) {
        //Debug.Log("Searching for:" + eleName);
        foreach (GameObject element in allChecklistElementsUnsrted) {
            //Debug.Log("Found ele:" + element.name);
            if (element.name == eleName) {
                return element;
            }
        }
        return null;
    }
    public bool isUnityEditor = false;
    // Start is called before the first frame update
    void Start() {
        //logParticipantData();
        #if !UNITY_EDITOR
        DIR = Application.persistentDataPath + "/";
        #endif
        #if UNITY_EDITOR
        DIR = "Logs/";
        #endif
        logFloorDwellData.Add("Dwell Time At Floor (>0.5f), Last Element Looked At Name, Last Element Looked At ID, Next Element Checked off list (Name), Next Element Checked off list (ID)");
        if (isUnityEditor) {
            allChecklistElementsUnsrted = GameObject.FindGameObjectsWithTag("BIM");
            elementsByID = new GameObject[30];
            readPTChecklistData();
            root = FindObjectOfType<updatePos>();
            Debug.Log("Found:" + root.transform.name);
            cubeLocalPlaybackObj.SetActive(true);
            readData();
            readRTChecklistData();
        }
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
    public storeRTChecklistData[] realtimeChecklistData = null;
    public float offset;
    private int count = 0;
    public float overallEyeTrackingTime = 0f;
    private bool exit = false;
    [SerializeField]
    private updatePos root;

    private GameObject[] allChecklistElementsUnsrted;

    public void readPTChecklistData() {
        Debug.Log("DIR = " + @DIR + DIRCLP + importFile + ".csv");
        using (var reader = new StreamReader(@DIR + DIRCLP + importFile + ".csv")) {
            int lineCount = getLines(new StreamReader(@DIR + DIRCLP + importFile + ".csv"));
            int count = 0;
            while (!reader.EndOfStream) {
                var line = reader.ReadLine();
                if (count >= 1) {
                    string[] split = line.Split(',');
                    string elementName = split[0];
                    GameObject obj = findChecklistID(elementName);
                    elementsByID[count-1] = obj;
                }
                count++;
            }
        }
    }
    public void readRTChecklistData() {
        Debug.Log("DIR = " + @DIR + DIRCL + importFile + ".csv");
        using (var reader = new StreamReader(@DIR + DIRCL + importFile + ".csv")) {
            int lineCount = getLines(new StreamReader(@DIR + DIRCL + importFile + ".csv"));
            realtimeChecklistData = new storeRTChecklistData[lineCount];
            count = 0;
            while (!reader.EndOfStream) {
                var line = reader.ReadLine();
                string[] split = line.Split(',');
                Debug.Log(line);
                if (count >= 1) {
                    Debug.Log("Setting up data.." + count);
                    storeRTChecklistData RTCheckListData = new storeRTChecklistData();
                    RTCheckListData.setTimeStamp(split[0]);
                    RTCheckListData.setEleName(split[1]);
                    RTCheckListData.setEleID(split[2]);
                    RTCheckListData.setChecklistValue(split[3]);
                    realtimeChecklistData[count - 1] = RTCheckListData;
                    //Debug.Log("Test1:"+RTCheckListData.getTimeStamp());
                    //Debug.Log("Test2:"+realtimeChecklistData[count - 1].getTimeStamp());
                    //realtimeChecklistData[count - 1] = RTCheckListData;
                    /*realtimeChecklistData[count - 1].setTimeStamp(split[0]);
                    realtimeChecklistData[count - 1].setEleName(split[1]);
                    realtimeChecklistData[count - 1].setEleID(split[2]);
                    realtimeChecklistData[count - 1].setChecklistValue(split[3]);*/
                    GameObject refGameObj = elementsByID[int.Parse(split[2])];
                    if (refGameObj.GetComponent<elementData>() == null) {
                        elementData eleData = refGameObj.AddComponent<elementData>();
                        eleData.elementID = int.Parse(split[2]);
                        realtimeChecklistData[count - 1].setRefGameObject(refGameObj);

                        //Text label = refGameObj.transform.GetChild(0).GetChild(0).GetComponent<Text>();
                        //label.text = "ID=" + split[2];
                    }
                    //Debug.Log("Name:"+);
                }
                count++;
            }
        }
    }
    public void readData() {
        Debug.Log("DIR = " + @DIR + DIRET + importFile + ".csv");
        using (var reader = new StreamReader(@DIR + DIRET + importFile + ".csv")) {
            int lineCount = getLines(new StreamReader(@DIR + DIRET + importFile + ".csv"));
            slider.maxValue = lineCount;
            Debug.Log("Lines:" + lineCount);
            data = new storeData[lineCount];
            count = 0;
            while (!reader.EndOfStream && !exit) {
                if (count == lineCount-1) {
                    Debug.Log("Exiting here..");
                    string[] splitReader = reader.ReadLine().Split(',');
                    root.rootPos = new Vector3(float.Parse(splitReader[0]), float.Parse(splitReader[1]), float.Parse(splitReader[2]));
                    root.rootRot = new Vector3(float.Parse(splitReader[3]), float.Parse(splitReader[4]), float.Parse(splitReader[5]));
                    root.initPos();
                    exit = true;
                    continue;
                }
                var line = reader.ReadLine();
                //Debug.Log(line);
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
