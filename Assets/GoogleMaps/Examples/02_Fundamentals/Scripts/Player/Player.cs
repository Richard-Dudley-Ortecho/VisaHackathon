using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public popup popup;
    public Text points;
    public Text latValue;
    public Text lonValue;

    private int score;
    private List<int> visitedMerchants;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        visitedMerchants = new List<int>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Handle collisions
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "marked merchant") {
            int id = collision.collider.GetInstanceID();
            if (!visitedMerchants.Contains(id)) {
                score += 1;
                points.text = "Points: " + score;
                visitedMerchants.Add(id);
            }
            string merchantString = collision.collider.name;
            popup.Show((int)(Random.value * 10), merchantString, latValue.text, lonValue.text);
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
