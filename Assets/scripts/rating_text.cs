using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using SimpleJSON;
using System.IO;
using System.Text;
using GP;
using UnityEngine.UI;

public class rating_text : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update ratings
    public void UpdateRating() {
        /*
             * Additions by Luke Joyce
             * 6/27/2020
             */
        // Make Google Places API call
        // Maps API key: AIzaSyADWTa8YQtzzCzWLH1dZv17Ad8gCRNcDQ0
        Text text = gameObject.GetComponent<Text>();

        GoogleAPIClient gpObject = new GoogleAPIClient();

        string baseURL = "https://maps.googleapis.com/maps/api/place/details/json?";
        string place_id = "place_id=ChIJXZiBbDDsa4cRobxsl8GF2Is";
        string APIkey = "AIzaSyADWTa8YQtzzCzWLH1dZv17Ad8gCRNcDQ0";
        string requestURL = baseURL + place_id + "&fields=name,rating,reviews,formatted_phone_number,business_status&key=" + APIkey;
        Tuple<string, string> gpAuthCall = gpObject.DoMutualAuthCall(requestURL, "GET", "Google Places API Test", "");
        string gpStatus = gpAuthCall.Item1;
        string gpResponse = gpAuthCall.Item2;

        var gpResponseJSON = JSON.Parse(gpResponse);

        Debug.Log("Parsed JSON: " + gpResponseJSON.ToString());

        // Change text
        string desiredText = gpResponseJSON.ToString();
        text.text = desiredText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
