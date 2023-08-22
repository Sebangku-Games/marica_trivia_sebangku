using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class input_handler : MonoBehaviour
{
    public InputField inputField;

    public void OnTextSubmitted()
    {
        string inputText = inputField.text;
        text_manager.instance.StoreText(inputText);
        Debug.Log("inputText");
    }


}
