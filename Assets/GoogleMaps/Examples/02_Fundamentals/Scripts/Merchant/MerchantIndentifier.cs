using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Google.Maps.Event;
using Google.Maps.Examples.Shared;
using System.Collections.Generic;
using Google.Maps.Coord;
using Google.Maps.Unity;

public class MerchantIndentifier : MonoBehaviour
{
    //Summary:
    // During the creation buildings, and with the click of the Merc Locator button
    // Several (?) Buildings will be identified as a small merchant location
    // This script will identify these buildings and make sure their GameObject
    // Is portrayed differently, in this example they will be blinking blue

    //TODO see if we can use this labeller to display info on the merchant
    //private Google.Maps.Examples.MapLabeller Labeller;

    public Google.Maps.MapsService MapsService;

    //private HashSet<GameObject> UncheckedStructures = new HashSet<GameObject>();

    public List<Merchant> businessesToLoad = new List<Merchant>();
    
    // private void Awake() {
    //    MapsService = GetComponent<Google.Maps.MapsService>();
    // }

    private Vector3 GetBuildingPosition(int index) {
      Google.Maps.Coord.LatLng latLng = businessesToLoad[index].merchantLocation;
      return MapsService.Coords.FromLatLngToVector3(latLng);
    }


    private void MarkStructures(int index) {
        Vector3 center = GetBuildingPosition(index);
        string name = businessesToLoad[index].merchantName;

        //Make a collider with the buildings
        Collider[] mercBuilding = Physics.OverlapSphere(center, 20.0f);
      
        foreach (Collider potentialBuilding in mercBuilding)
        {
            GameObject building = potentialBuilding.gameObject;
            if (IsStructure(building)) {
                SetMercMarker(building, name);
            }
        }
    }

    private void OnClickFunction()
    {
        //businessesToLoad.Clear();

        //Here we would populate the business locations in address or LatLong
        //Here i will add dummy values based off of the coordinates 40.0188, -105.27818
        //in this case its a small local toy store (turn 180 from starting position) really close
        Google.Maps.Coord.LatLng fakeBusinessLoc = new Google.Maps.Coord.LatLng(40.0182, -105.2769);
        AddBusiness("McDonalds", "Free Fries", fakeBusinessLoc);
        

        //Now we set all the buildings to be marked accordingly
        for (int i = 0; i < businessesToLoad.Count; i++) {
          MarkStructures(i);
        }
    }

    public void AddBusiness(string name, string deals, Google.Maps.Coord.LatLng location) 
    {
        Merchant m = new Merchant(name, location);
        businessesToLoad.Add(m);
    }

    private void SetMercMarker(GameObject gameObject, string merchantName) {
        //Change the game object so that it is now marked, in this case it will be ?blinking? blue
        Renderer renderer = gameObject.GetComponent<Renderer>();
        renderer.material.color = Color.blue;
        gameObject.tag = "marked merchant";
        gameObject.name = merchantName;
    }

    private bool IsStructure(GameObject gameObject) {
      ExtrudedStructureComponent extrudedComponent =
          gameObject.GetComponent<ExtrudedStructureComponent>();

      ModeledStructureComponent modeledComponent =
          gameObject.GetComponent<ModeledStructureComponent>();

      return ((extrudedComponent != null) || (modeledComponent != null));
    }
    
    private void HandleStructureSpawn(GameObject gameObject) {
      //Make sure each building has a collider to "Find" the building
      gameObject.AddComponent<BoxCollider>();
      gameObject.tag = "structure";
    }

    void Start()
    {

        //Find the visa canvas where the button exists
        GameObject visaButton = GameObject.Find("VisaButton");

        Button mercLocatorButton = visaButton.GetComponentInChildren<Button>();

        //mercLocatorButton.onClick.AddListener(() => onClickFunction());

        mercLocatorButton.onClick.AddListener(() => OnClickFunction());

        mercLocatorButton.GetComponentInChildren<Text>().text = "Merc Locator";

        //Make sure the created buildings have colliders
        MapsService.Events.ExtrudedStructureEvents.DidCreate.AddListener(
            args => { HandleStructureSpawn(args.GameObject); });

        MapsService.Events.ModeledStructureEvents.DidCreate.AddListener(
            args => { HandleStructureSpawn(args.GameObject); });

    }

}

public class Merchant {
    public string merchantName;
    public Google.Maps.Coord.LatLng merchantLocation;

    public Merchant(string n, Google.Maps.Coord.LatLng l) {
        this.merchantLocation = l;
        this.merchantName = n;
    }
}
