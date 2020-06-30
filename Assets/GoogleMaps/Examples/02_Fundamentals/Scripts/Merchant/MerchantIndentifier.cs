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

    public List<Google.Maps.Coord.LatLng> businessesToLoad = new List<Google.Maps.Coord.LatLng>();
    
    // private void Awake() {
    //    MapsService = GetComponent<Google.Maps.MapsService>();
    // }

    private Vector3 GetBuildingPosition(int index) {
      Google.Maps.Coord.LatLng latLng = businessesToLoad[index];
      return MapsService.Coords.FromLatLngToVector3(latLng);
    }


    private void MarkStructures(int index) {
      Vector3 center = GetBuildingPosition(index);

      //Make a collider with the buildings
      Collider[] mercBuilding = Physics.OverlapSphere(center, 20.0f);
      
      foreach (Collider potentialBuilding in mercBuilding)
      {
        GameObject building = potentialBuilding.gameObject;
        //testB.Add(building);
        if (IsStructure(building)) {
          //testB = building;
          SetMercMarker(building);
        }
      }
    }

    private void OnClickFunction()
    {
        Debug.Log("Merchant locator button pressed");

        //businessesToLoad.Clear();

        //Here we would populate the business locations in address or LatLong
        //Here i will add dummy values based off of the coordinates 40.00044, -105.25184
        //in this case its a mcdonalds up the street
        Google.Maps.Coord.LatLng fakeBusinessLoc = new Google.Maps.Coord.LatLng(39.999423, -105.255135);
        AddBusiness("Micky Ds", "Free Fries", fakeBusinessLoc);
        

        //Now we set all the buildings to be marked accordingly
        for (int i = 0; i < businessesToLoad.Count; i++) {
          MarkStructures(i);
        }
    }

    public void AddBusiness(string name, string deals, Google.Maps.Coord.LatLng location) 
    {
      if( !businessesToLoad.Contains(location) ) {
        businessesToLoad.Add(location);
      }
    }

    private void SetMercMarker(GameObject gameObject) {
      //Change the game object so that it is now marked, in this case it will be ?blinking? blue
      Renderer renderer = gameObject.GetComponent<Renderer>();
      renderer.material.color = Color.blue;
      gameObject.tag = "marked merchant";
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


    // void Awake() {
    //     Labeller = GetComponent<Google.Maps.Examples.MapLabeller>();
    //     //I dont know if we need this labller tbh
    // }

    // void OnEnable() {
    //     Labeller.BaseMapLoader.MapsService.Events.ExtrudedStructureEvents.DidCreate.AddListener(
    //       OnExtrudedStructureCreated);

    //     Labeller.BaseMapLoader.MapsService.Events.ModeledStructureEvents.DidCreate.AddListener(
    //       OnModeledStructureCreated);
    // }

    // void OnDisable() {
    //   Labeller.BaseMapLoader.MapsService.Events.ExtrudedStructureEvents.DidCreate.RemoveListener(
    //       OnExtrudedStructureCreated);
          
    //   Labeller.BaseMapLoader.MapsService.Events.ModeledStructureEvents.DidCreate.RemoveListener(
    //       OnModeledStructureCreated);
    // }

    // void OnExtrudedStructureCreated(DidCreateExtrudedStructureArgs args) {
    //   CreateLabel(args.GameObject, args.MapFeature.Metadata.PlaceId, args.MapFeature.Metadata.Name);
    // }

    // void OnModeledStructureCreated(DidCreateModeledStructureArgs args) {
    //   CreateLabel(args.GameObject, args.MapFeature.Metadata.PlaceId, args.MapFeature.Metadata.Name);
    // }

    // void CreateLabel(GameObject buildingGameObject, string placeId, string displayName) {
    //   if (!Labeller.enabled)
    //     return;

    //   // Ignore uninteresting names.
    //   if (displayName.Equals("ExtrudedStructure") || displayName.Equals("ModeledStructure")) {
    //     return;
    //   }

    //   Label label = Labeller.NameObject(buildingGameObject, placeId, displayName);
    //   // if (label != null) {
    //   //   MapsGamingExamplesUtils.PlaceUIMarker(buildingGameObject, label.transform);
    //   // }
    // }

}
