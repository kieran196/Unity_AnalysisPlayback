using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class csvParser : MonoBehaviour {
    private string fileType;
    // Start is called before the first frame update
    void Start() {
        if (isCsvFile) {
            fileType = ".csv";
        } else {
            fileType = ".xlsx";
        }
        readData();
        calculate();
    }

    [SerializeField]
    private bool readAndUpdateData;
    void Update() {
        if (readAndUpdateData) { 
            readAndUpdateData = false;
            readData();
            calculate();
        }
    }
    private string badElements, poorElements, trueElementErrors;

    private float calculatedFTPRating, calculatedGTPRating;
    [SerializeField]
    private float totalPRating, totalFTPRating, totalGTPRating;
    [SerializeField]
    private float[] EPR, FTP, GTP;
    void calculate() {
        EPR = new float[30];
        FTP = new float[30];
        GTP = new float[30];
        calculatedFTPRating = 0f;
        calculatedGTPRating = 0f;
        totalFTPRating = 0f;
        totalGTPRating = 0f;
        totalPRating = 0f;
        badElements = "";
        poorElements = "";
        Debug.Log("Performance Ratings..");
        // Calculations below
        for (int i=0; i<storedGT.Length; i++) {
            float getThresholdReqFT = performanceCalculator.THRESHOLD_REQUIREMENTS_FT(storedSize[i]);
            float getThresholdReqGT = performanceCalculator.THRESHOLD_REQUIREMENTS_GT(storedSize[i]);
            calculatedFTPRating = performanceCalculator.getPRating(float.Parse(storedGT[i]), getThresholdReqFT);
            calculatedGTPRating = performanceCalculator.getLookingPRating(float.Parse(storedLT[i]), getThresholdReqGT);
            float pRating = calculatedFTPRating + calculatedGTPRating;
            FTP[i] = calculatedFTPRating;
            GTP[i] = calculatedGTPRating;
            EPR[i] = pRating;
            if (pRating <= 1.5f && pRating > 0.5f) {
                badElements += i.ToString() + ",";
            } else if (pRating <= 0.5f) {
                poorElements += i.ToString() + ",";
            }
            totalFTPRating += calculatedFTPRating;
            totalGTPRating += calculatedGTPRating;
            totalPRating += pRating;
        }
        Debug.Log("Total P Rating:" + totalPRating);
        Debug.Log("Calculated Perecentage: " + totalPRating / 180f); // (6*30) = Max score
        Debug.Log("True Err Count:" + (30-trueErrorCount) / 30f);
        Debug.Log("True Errors:" + trueElementErrors);
        Debug.Log("Predicted bad elements:" + badElements);
        Debug.Log("Predicted poor elements:" + poorElements);
    }

    public string importFile = "";
    private string DIR = "Logs/stats/";
    public string[] storedLT, storedGT, storedSize, storedErrs;
    private int count = 0;
    public bool isCsvFile = false;

    public int getLines(StreamReader reader) {
        int count = 0;
        while (reader.ReadLine() != null) {
            count++;
        }
        return count;
    }
    public int trueErrorCount = 0;

    public void readData() {
        trueElementErrors = "";
        trueErrorCount = 0;
        Debug.Log("DIR = " + @DIR + importFile + fileType);
        using (var reader = new StreamReader(@DIR  + importFile + fileType)) {
            int lineCount = getLines(new StreamReader(@DIR + importFile + fileType));
            Debug.Log("Lines:" + lineCount);
            storedLT = new string[lineCount-4];
            storedGT = new string[lineCount-4];
            storedErrs = new string[lineCount-4];
            storedSize = new string[lineCount-4];
            count = 0;
            int subCount = 0;
            while (!reader.EndOfStream) {
                var line = reader.ReadLine();
                //Debug.Log(line);
                string[] split = line.Split(',');
                if (count >= 4) {
                    // Start here..
                    storedLT[subCount] = split[1];
                    storedGT[subCount] = split[2]; // 2 = Actual Time, 3 = Gaze (w/500ms threshold)
                    storedErrs[subCount] = split[5];
                    if (split[5] == "TRUE") {
                        trueErrorCount++;
                        trueElementErrors += subCount + ",";
                    }
                    storedSize[subCount] = split[6];
                    subCount++;
                }
                count++;
            }
        }
    }


}
