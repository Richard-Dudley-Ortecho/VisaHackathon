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
    public void UpdateRating(string merchantName, string latValue, string lonValue) {
        /*
             * Additions by Luke Joyce
             * 6/27/2020
             */
        // Make Google Places API call
        // Maps API key: AIzaSyADWTa8YQtzzCzWLH1dZv17Ad8gCRNcDQ0
        Text text = gameObject.GetComponent<Text>();

        GoogleAPIClient gpObject = new GoogleAPIClient();

        // Make a place search API call to place_id
        string basePlaceURL = "https://maps.googleapis.com/maps/api/place/findplacefromtext/json?";
        string APIkey = "AIzaSyADWTa8YQtzzCzWLH1dZv17Ad8gCRNcDQ0";
        string requestPlaceURL = basePlaceURL + "inputtype=textquery&key=" + APIkey
            + "&input=" + merchantName + "&locationbias=circle:1600@" + latValue + "," + lonValue;
        Tuple<string, string> gpPlaceAuthCall = gpObject.DoMutualAuthCall(requestPlaceURL, "GET", "Google Places API Test", "");
        string gpPlaceStatus = gpPlaceAuthCall.Item1;
        string gpPlaceResponse = gpPlaceAuthCall.Item2;
        if (gpPlaceStatus != "OK") {
            Debug.Log("Invalid place status: " + gpPlaceStatus);
            return;
        }
        var gpPlaceResponseJSON = JSON.Parse(gpPlaceResponse);
        if (gpPlaceResponseJSON["candidates"].AsArray.Count == 0) {
            Debug.Log("Find place returned no results.");
            return;
        }
        string place_id = "place_id=" + gpPlaceResponseJSON["candidates"][0]["place_id"];
        Debug.Log("Place search response: " + gpPlaceResponse);


        // Make a place detail API call
        string baseDetailURL = "https://maps.googleapis.com/maps/api/place/details/json?";
        //string place_id = "place_id=ChIJXZiBbDDsa4cRobxsl8GF2Is";
        string requestDetailURL = baseDetailURL + place_id + "&fields=name,rating,reviews,formatted_phone_number,formatted_address,business_status,price_level&key=" + APIkey;
        Tuple<string, string> gpDetailAuthCall = gpObject.DoMutualAuthCall(requestDetailURL, "GET", "Google Places API Test", "");

        string gpDetailStatus = gpDetailAuthCall.Item1;
        string gpDetailResponse = gpDetailAuthCall.Item2;

        if (gpDetailStatus != "OK") {
            Debug.Log("Invalid detail status: " + gpDetailStatus);
            return;
        }

        var gpDetailResponseJSON = JSON.Parse(gpDetailResponse);

        // Change text
        string rating = gpDetailResponseJSON["result"]["rating"].Value;
        string phone_number = gpDetailResponseJSON["result"]["formatted_phone_number"];
        string address = gpDetailResponseJSON["result"]["formatted_address"];
        string price_level = gpDetailResponseJSON["result"]["price_level"];
        string business_status = gpDetailResponseJSON["result"]["business_status"];

        //string desiredText = gpDetailResponseJSON.ToString();
        text.text = "Rating: " + rating
            + "\nPhone number: " + phone_number
            + "\nAddress: " + address
            + "\nPrice level: " + price_level
            + "\nBusiness status: " + business_status;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
