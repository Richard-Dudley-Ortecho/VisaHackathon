using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class name_text : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update name
    public void UpdateName(string name)
    {
        // Change text
        string desiredText = name;
        Text text = gameObject.GetComponent<Text>();
        text.text = desiredText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
