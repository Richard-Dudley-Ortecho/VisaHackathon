using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class box : MonoBehaviour
{
    public GameObject cam;
    public GameObject popup;
    public int offerStartIndex;
    public int rightDist;

    // Start is called before the first frame update
    void Start()
    {
        // Position cube
        Transform camTransform = cam.GetComponent<Transform>();
        Transform thisTransform = gameObject.GetComponent<Transform>();
        thisTransform.position = camTransform.position + camTransform.transform.forward * 5 + camTransform.transform.right * rightDist;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.name == gameObject.name)
                {
                    print("My object is clicked by mouse");
                    popup.GetComponent<popup>().Show(offerStartIndex);
                }
            }
        }
    }
}
