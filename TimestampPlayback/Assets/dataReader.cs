using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class dataReader : MonoBehaviour
{
    public GameObject cubePlaybackObj;
    public Slider slider;
    public Text timeStampText;
    public string importFile = "";
    private const string DIR = "Logs/";
    private bool dataInitialized = false;

    private void Update() {
        if (dataInitialized) {
            setCube((int)slider.value);
        }
    }

    public void setCube(int slider) {
        Debug.Log("At pos:" + slider + " | data:" + data[slider] + " , Length:" + data.Length);
        timeStampText.text = "Timestamp = "+ data[slider].getTimeStamp();
        cubePlaybackObj.transform.position = data[slider].getPosition();
    }

    // Start is called before the first frame update
    void Start() {
        readData();
    }

    public int getLines(StreamReader reader) {
        int count = 0;
        while (reader.ReadLine() != null) {
            count++;
        }
        return count;
    }
    storeData[] data = null;
    public void readData() {
        using (var reader = new StreamReader(@DIR + importFile + ".csv")) {
            int lineCount = getLines(new StreamReader(@DIR + importFile + ".csv"));
            slider.maxValue = lineCount;
            Debug.Log("Lines:" + lineCount);
            data = new storeData[lineCount];
            int count = 0;
            while (!reader.EndOfStream) {
                var line = reader.ReadLine();
                string[] split = line.Split(',');
                if (count >= 1) {
                    //Debug.Log(data[count-1]);
                    if (split.Length > 3) {
                        data[count - 1] = new storeData();
                        data[count-1].setTimeStamp(split[0]);
                        data[count-1].setPosition(new Vector3(float.Parse(split[1]), float.Parse(split[2]), float.Parse(split[3])));
                        Debug.Log("Timestamp:" + split[0] + ", x:" + split[1] + ", y:" + split[2] + ", z" + split[3]);
                    }
                }
                count++;
            }
        }
        dataInitialized = true;
    }

}
