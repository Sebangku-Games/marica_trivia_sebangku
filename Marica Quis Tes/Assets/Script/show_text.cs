using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class show_text : MonoBehaviour
{
    public Text showText;
    // Start is called before the first frame update
    void Start()
    {
        string storedText = text_manager.instance.GetStoredText();
        showText.text = storedText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
