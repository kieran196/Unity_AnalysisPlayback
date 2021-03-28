using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class checklistParser : MonoBehaviour {

    public string importARFile = "";
    public string importPAPERFile = "";
    private string DIR = "Checklist/";

    public string[] AR_RESULTS, PAPER_RESULTS;

    public string[] approximationVals;

    private void parseAllARData(string fileType) {
        Debug.Log("Parsing all AR data");
        using (var reader = new StreamReader(@DIR + fileType + ".csv")) {
            int count = -1;
            while (!reader.EndOfStream) {
                var line = reader.ReadLine();
                //Debug.Log(line);
                if (count >= 0) {
                    string[] split = line.Split(',');
                    string approx = split[3];
                    if (approx == "") {
                        approx = "0cm";
                    }
                    Debug.Log("ID:" + count +  " | Approx:" + approx);
                    approximationVals[count] += approx+",";
                }
                count++;
            }
        }
    }

    private void parseDataAR(string fileType) {
        using (var reader = new StreamReader(@DIR + fileType + ".csv")) {
            int count = -1;
            float correctCount = 0;
            float incorrectCount = 0;
            while (!reader.EndOfStream) {
                var line = reader.ReadLine();
                if (count >= 0) {
                    string[] split = line.Split(',');
                    string result = "";
                    if (split[2] == AR_RESULTS[int.Parse(split[1])]) {
                        result = "CORRECT";
                        correctCount++;
                    } else {
                        incorrectIDs += split[2] + ",";
                        incorrectIDVal += count + ",";
                        result = "INCORRECT";
                        incorrectCount++;
                    }
                    //Debug.Log("ID:"+ split[1] + " | Value:" + split[2] + " | Result:" + result);
                }
                count++;
            }
            Debug.Log("AR | Correct:" + (correctCount/30f) + " , Incorrect:" + (incorrectCount/30f) + " | Correct Raw:" + correctCount);
            Debug.Log("IDs:" + incorrectIDVal + " , Values:" + incorrectIDs);
        }
    }

    private string incorrectIDs = "";
    private string incorrectIDVal = "";

    private void parseDataPaper(string fileType) {
        using (var reader = new StreamReader(@DIR + fileType + ".csv")) {
            int count = -1;
            float correctCount = 0;
            float incorrectCount = 0;
            while (!reader.EndOfStream) {
                var line = reader.ReadLine();
                if (count >= 0) {
                    string[] split = line.Split(',');
                    string result = "";
                    if (split[2] == PAPER_RESULTS[int.Parse(split[1])]) {
                        result = "CORRECT";
                        correctCount++;
                    } else {
                        incorrectIDs += split[2] + ",";
                        incorrectIDVal += count + ",";
                        result = "INCORRECT";
                        incorrectCount++;
                    }
                    //Debug.Log("ID:"+ split[1] + " | Value:" + split[2] + " | Result:" + result);
                }
                count++;
            }
            Debug.Log("Paper | Correct:" + (correctCount/30f) + " , Incorrect:" + (incorrectCount/30f)  + " | Correct Raw:" + correctCount);
            Debug.Log("IDs:" + incorrectIDVal + " , Values:" + incorrectIDs);
        }
    }

    void Start() {
        approximationVals = new string[30];
    }

    [SerializeField]
    private bool parseData;
    [SerializeField]
    private bool parseAllARApproxData;
    void Update() {
        if (parseAllARApproxData) {
            parseAllARData("p1PB");
            parseAllARData("p2PB");
            parseAllARData("p3PB");
            parseAllARData("p4PB");
            parseAllARData("p5PB");
            parseAllARData("p6PB");
            parseAllARData("p7PB");
            parseAllARData("p8PB");
            parseAllARData("p9PB");
            parseAllARData("p10PB");
            parseAllARData("p11PB");
            parseAllARApproxData = false;
        }
        if (parseData) {
            Debug.Log("~~~ NEW DATASET ~~~");
            incorrectIDs = "";
            incorrectIDVal = "";
            parseDataPaper(importPAPERFile);
            incorrectIDs = "";
            incorrectIDVal = "";
            parseDataAR(importARFile);
            parseData = false;
        }
    }

}
