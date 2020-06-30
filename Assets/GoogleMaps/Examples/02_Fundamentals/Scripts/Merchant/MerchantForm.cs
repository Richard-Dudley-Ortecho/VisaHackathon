using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Google.Maps;

public class MerchantForm : MonoBehaviour
{
    public Button merchantBusinessFormSubmit;

    public InputField businessName;
    public InputField businessCoord;
    public InputField businessDeals;

    public MerchantIndentifier mI;

    void Start()
    {
        Button formButton = merchantBusinessFormSubmit.GetComponent<Button>();

        businessName = businessName.GetComponent<InputField>();
        businessCoord = businessCoord.GetComponent<InputField>();
        businessDeals = businessDeals.GetComponent<InputField>();

        formButton.onClick.AddListener(SubmitForm);
    }
    
    void SubmitForm()
    { 

        string bName = businessName.text;
        string bLocation = businessCoord.text;
        string bDeals = businessDeals.text;

        Debug.Log(bName);
        Debug.Log(bLocation);
        Debug.Log(bDeals);

        if(bName != "" && bLocation != "" && bDeals != "")
        {
            string[] latLongs = bLocation.Split(',');
            double lat = System.Convert.ToDouble(latLongs[0]);
            double longitude = System.Convert.ToDouble(latLongs[1]);

            Google.Maps.Coord.LatLng bLatLong = new Google.Maps.Coord.LatLng(lat, longitude);
            
            mI.AddBusiness(bName,bDeals,bLatLong);
        }

        businessName.text = "";
        businessCoord.text = "";
        businessDeals.text = "";

    }

}
