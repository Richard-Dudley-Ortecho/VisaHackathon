using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vdp;
using System;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    	Debug.Log("Debugging start");
        VisaAPIClient visaAPIClient = new VisaAPIClient();

        string baseUri = "vmorc/offers";
        string resourcePath = "/v1/all";
        string queryParameters = "?max_offers=5";
        string status = visaAPIClient.DoMutualAuthCall(baseUri + resourcePath + queryParameters, "GET", "Merchant Offer API Test", "");

        Debug.Log(status);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
