using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class performanceCalculator {
    // PREV 8, 4, 2 - 8.58, 6.45, 3.82
    public static float LARGE_FT = 8.58f;
    public static float MEDIUM_FT = 6.45f;
    public static float SMALL_FT = 3.82f;
    
    
    public static float LARGE_GT = 31.84f;
    public static float MEDIUM_GT = 22.50f;
    public static float SMALL_GT = 7.43f;

    public static float THRESHOLD_REQUIREMENTS_FT(string size) {
        switch(size) {
            case "L":
            return LARGE_FT;
            case "M":
            return MEDIUM_FT;
            case "S":
            return SMALL_FT;
        }
        throw new InvalidOperationException("ERROR - SHOULD NEVER REACH HERE (THRESHOLD_REQUIREMENTS method)");
    }

    public static float THRESHOLD_REQUIREMENTS_GT(string size) {
        switch(size) {
            case "L":
            return LARGE_GT;
            case "M":
            return MEDIUM_GT;
            case "S":
            return SMALL_GT;
        }
        throw new InvalidOperationException("ERROR - SHOULD NEVER REACH HERE (THRESHOLD_REQUIREMENTS method)");
    }

    // Params GT = Gaze Fixation Time, TREQ = Threshold Requirement
    public static float getPRating(float FT, float TREQ) {
        float TREQ1 = TREQ/2f;
        float TREQ2 = TREQ1/2f;
        float TREQ3 = TREQ2/2f;
        
        if (FT >= TREQ) { // Excellent
            return 4;
        } else if (FT < TREQ && FT >= TREQ1) { // Good
            return 3;
        } else if (FT < TREQ1 && FT >= TREQ2) { // Moderate
            return 2;
        } else if (FT < TREQ2 && FT >= TREQ3) { // Bad
            return 1;
        } else if (FT < TREQ3) { // Poor
            return 0;
        }
        throw new InvalidOperationException("ERROR - SHOULD NEVER REACH HERE (getPRating method)");
    }

    //Factoring in Time Spent Looking At Element Params GT = Gaze Time
    public static float getLookingPRating(float GT, float TREQ) {
        float TREQ1 = TREQ/2f;
        float TREQ2 = TREQ1/2f;
        float TREQ3 = TREQ2/2f;
        
        if (GT >= TREQ) { // Excellent
            return 2;
        } else if (GT < TREQ && GT >= TREQ1) { // Good
            return 1.5f;
        } else if (GT < TREQ1 && GT >= TREQ2) { // Moderate
            return 1;
        } else if (GT < TREQ2 && GT >= TREQ3) { // Bad
            return 0.5f;
        } else if (GT < TREQ3) { // Poor
            return 0;
        }
        throw new InvalidOperationException("ERROR - SHOULD NEVER REACH HERE (getLookingPRating method)");
    }
}
