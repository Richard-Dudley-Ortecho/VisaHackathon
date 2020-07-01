using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using SimpleJSON;
using System.IO;
using System.Text;
using UnityEngine.UI;
using GP;

public class Reviews : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void UpdateReviews(string merchantName, string latValue, string lonValue)
    {
        Debug.Log("enter update reviews!!!!!!!!!!!!!!!!!!!");
        Text text = gameObject.GetComponent<Text>();
        var text_height = gameObject.GetComponent<RectTransform>();

        GoogleAPIClient gpObject = new GoogleAPIClient();

        string basePlaceURL = "https://maps.googleapis.com/maps/api/place/findplacefromtext/json?";
        string APIkey = "AIzaSyADWTa8YQtzzCzWLH1dZv17Ad8gCRNcDQ0";
        string requestPlaceURL = basePlaceURL + "inputtype=textquery&key=" + APIkey
            + "&input=" + merchantName + "&locationbias=circle:1600@" + latValue + "," + lonValue;
        Tuple<string, string> gpPlaceAuthCall = gpObject.DoMutualAuthCall(requestPlaceURL, "GET", "Google Places API Test", "");
        string gpPlaceStatus = gpPlaceAuthCall.Item1;
        string gpPlaceResponse = gpPlaceAuthCall.Item2;
        if (gpPlaceStatus != "OK")
        {
            Debug.Log("Invalid place status: " + gpPlaceStatus);
            return;
        }
        var gpPlaceResponseJSON = JSON.Parse(gpPlaceResponse);
        if (gpPlaceResponseJSON["candidates"].AsArray.Count == 0)
        {
            Debug.Log("Find place returned no results.");
            return;
        }
        string place_id = "place_id=" + gpPlaceResponseJSON["candidates"][0]["place_id"];
        Debug.Log("Place search response: " + gpPlaceResponse);


        //Detail API request
        string baseDetailURL = "https://maps.googleapis.com/maps/api/place/details/json?";
        //string place_id = "place_id=ChIJXZiBbDDsa4cRobxsl8GF2Is";
        string requestDetailURL = baseDetailURL + place_id + "&fields=reviews&key=" + APIkey;
        Tuple<string, string> gpDetailAuthCall = gpObject.DoMutualAuthCall(requestDetailURL, "GET", "Google Places API Test", "");

        string gpDetailStatus = gpDetailAuthCall.Item1;
        string gpDetailResponse = gpDetailAuthCall.Item2;

        if (gpDetailStatus != "OK")
        {
            Debug.Log("Invalid detail status: " + gpDetailStatus);
            return;
        }

        var gpDetailResponseJSON = JSON.Parse(gpDetailResponse);
        Debug.Log(gpDetailResponseJSON.ToString());

        string user, rating, timestamp, review;
        string display = "";

        for(int i = 0; i < 5; i++)
        {
            user = "<b>" + gpDetailResponseJSON["result"]["reviews"][i]["author_name"] + "</b>";
            rating = gpDetailResponseJSON["result"]["reviews"][i]["rating"];
            timestamp = "<i>" + gpDetailResponseJSON["result"]["reviews"][i]["relative_time_description"] + "</i>";
            review = gpDetailResponseJSON["result"]["reviews"][i]["text"];

            display += "―――――――――――――\n―――――――――――――\n" + user
            + "\n" + stars_graphic(float.Parse(rating))
            + "\n<i> Posted </i>" + timestamp
            + "\n―――――――――――――"
            + "\n" + review + "\n";
        }

        text.text = display;

        float pref_height = text.preferredHeight;

        text_height.sizeDelta = new Vector2(275, pref_height);

        text_height.localPosition = new Vector2(0,-1*text_height.sizeDelta.y/2);


        //text_height.pos

        /*
        string user1 = "<b>" + gpDetailResponseJSON["result"]["reviews"][0]["author_name"] + "</b>";
        string rating1 = gpDetailResponseJSON["result"]["reviews"][0]["rating"];
        string timestamp = "<i>" + gpDetailResponseJSON["result"]["reviews"][0]["relative_time_description"] + "</i>";
        string review1 = gpDetailResponseJSON["result"]["reviews"][0]["text"];
        
        text.text = user1
            + "\n" + rating1 + " out of 5 stars"
            + "\n<i> Posted </i>" + timestamp
            + "\n―――――――――――――――――――"
            + "\n" + review1;
        */


    }

    string stars_graphic(float rating)
    {
        string stars;
        double whole_rating = Math.Round(rating);
        switch(whole_rating)
        {
            case 0:
                stars = "☆☆☆☆☆";
                break;
            case 1:
                stars = "★☆☆☆☆";
                break;
            case 2:
                stars = "★★☆☆☆";
                break;
            case 3:
                stars = "★★★☆☆";
                break;
            case 4:
                stars = "★★★★☆";
                break;
            default:
                stars = "★★★★★";
                break;
        }

        return stars;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
