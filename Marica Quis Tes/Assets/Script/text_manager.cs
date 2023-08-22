using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class text_manager : MonoBehaviour
{
    public static text_manager instance; // Membuat instance yang dapat diakses dari semua scene
    public string storedText; // Variabel untuk menyimpan teks

    public InputField input_field;
                              

    public void StoreText(string text)
    {
        string inputText = input_field.text;
        PlayerPrefs.SetString("StoredText", inputText);
    }

    public string GetStoredText()
    {
        return PlayerPrefs.GetString("StoredText", "");
    }
}
