using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vdp;
using System;
using System.Net;
using SimpleJSON;
using System.IO;
using System.Text;
using UnityEngine.UI;

public class offer_text : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Finds the offer information and updates the text
    public void UpdateOffer(int startIndex) {

        // Create client and get components
        VisaAPIClient visaAPIClient = new VisaAPIClient();
        Text text = gameObject.GetComponent<Text>();

        // Make API call
        string baseUri = "vmorc/offers";
        string resourcePath = "/v1/all";
        string queryParameters = "?max_offers=5&start_index=" + startIndex.ToString();
        Tuple<string, string> t = visaAPIClient.DoMutualAuthCall(baseUri + resourcePath + queryParameters, "GET", "Merchant Offer API Test", "");
        string status = t.Item1;
        string response = t.Item2;
        Debug.Log("Returned response from mutual auth call: " + response);

        // Decode JSON returned
        var responseJSON = JSON.Parse(response);
        string firstProgram = responseJSON["Offers"][1]["programName"].Value;
        string firstOffer = responseJSON["Offers"][1]["offerTitle"].Value;
        string firstFrom = responseJSON["Offers"][1]["validityFromDate"].Value;
        string firstTo = responseJSON["Offers"][1]["validityToDate"].Value;

        // Change text
        string desiredText = firstProgram + "\n" + firstOffer + "\n" + firstFrom + " to " + firstTo;
        text.text = desiredText;

        // Close response
        Debug.Log(status);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
