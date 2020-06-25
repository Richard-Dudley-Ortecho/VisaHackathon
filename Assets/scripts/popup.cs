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
    public void UpdateText(int offerStartIndex)
    {
        //offer_text ofText = gameObject.GetComponentInChildren<offer_text>();
        //rating_text rText = gameObject.GetComponentInChildren<rating_text>();

        ofText.UpdateOffer(offerStartIndex);
        rText.UpdateRating();
    }

    // Show gameobject
    public void Show(int offerStartIndex) {
        gameObject.SetActive(true);
        UpdateText(offerStartIndex);
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
