using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.IO;
using System.Text;
using UnityEngine.UI;
using GP;
using SimpleJSON;

public class LoadImage : MonoBehaviour
{

    public RawImage img;
    // Start is called before the first frame update
    void Start()
    {
    
    }

    public string photosHTTPRequest(string merchantName, string latValue, string lonValue)
    {
        GoogleAPIClient gpObject = new GoogleAPIClient();

        // Make a place search API call to place_id
        string basePlaceURL = "https://maps.googleapis.com/maps/api/place/findplacefromtext/json?";
        string APIkey = "AIzaSyADWTa8YQtzzCzWLH1dZv17Ad8gCRNcDQ0";
        string requestPlaceURL = basePlaceURL + "inputtype=textquery&key=" + APIkey
            + "&input=" + merchantName + "&locationbias=circle:1600@" + latValue + "," + lonValue;
        Tuple<string, string> gpPlaceAuthCall = gpObject.DoMutualAuthCall(requestPlaceURL, "GET", "Google Places API Test", "");
        string gpPlaceStatus = gpPlaceAuthCall.Item1;
        string gpPlaceResponse = gpPlaceAuthCall.Item2;

        var gpPlaceResponseJSON = JSON.Parse(gpPlaceResponse);

        string place_id = "place_id=" + gpPlaceResponseJSON["candidates"][0]["place_id"];
        Debug.Log("Place search response: " + gpPlaceResponse);

        string baseDetailURL = "https://maps.googleapis.com/maps/api/place/details/json?";
        string requestDetailURL = baseDetailURL + place_id + "&fields=photos&key=" + APIkey;

        Tuple<string, string> gpDetailAuthCall = gpObject.DoMutualAuthCall(requestDetailURL, "GET", "Google Places API Test", "");

        string gpDetailStatus = gpDetailAuthCall.Item1;
        string gpDetailResponse = gpDetailAuthCall.Item2;

        var gpDetailResponseJSON = JSON.Parse(gpDetailResponse);

        // Get photo info
        string width = gpDetailResponseJSON["result"]["photos"][0]["width"];
        string height = gpDetailResponseJSON["result"]["photos"][0]["height"];
        string photo_reference = gpDetailResponseJSON["result"]["photos"][0]["photo_reference"];

        string photoUrl = "https://maps.googleapis.com/maps/api/place/photo?" + "maxwidth=" + width + "&photo_reference=" + photo_reference + "&key=" + APIkey;

        return photoUrl;


    }

    // Update is called once per frame
    public IEnumerator LoadImageToUnity(string merchantName, string latValue, string lonValue)
    {
        string url = photosHTTPRequest(merchantName, latValue, lonValue);
        WWW w = new WWW(url);
        yield return w;
        Texture2D te = w.texture;
        img.texture = te;
    }
}
