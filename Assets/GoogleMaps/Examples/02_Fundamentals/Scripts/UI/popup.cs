using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class popup : MonoBehaviour
{

    public string merchantName;
    public string merchantID; //Visa's id
    public string latitude;
    public string longitude;

    public offer_text ofText;
    public rating_text rText;

    // Start is called before the first frame update
    void Start()
    {
        merchantName = "Placeholder";
        merchantID = "NA";
        latitude = "1";
        longitude = "1";

        gameObject.SetActive(false);
    }

    // Update underlying text
    public void UpdateText(int offerStartIndex, string merchantName, string latValue, string lonValue)
    {
        ofText.UpdateOffer(offerStartIndex);
        rText.UpdateRating(merchantName, latValue, lonValue);
    }

    // Show gameobject
    public void Show(int offerStartIndex, string merchantName, string latValue, string lonValue) {
        gameObject.SetActive(true);
        UpdateText(offerStartIndex, merchantName, latValue, lonValue);
    }

    // Hide gameobject
    public void Hide() {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
