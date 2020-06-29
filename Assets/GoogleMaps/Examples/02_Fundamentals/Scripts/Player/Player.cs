using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public popup popup;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Handle collisions
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "marked merchant") {
            popup.Show((int)(Random.value * 10));
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "marked merchant")
        {
            popup.Hide();
        }
    }
}
