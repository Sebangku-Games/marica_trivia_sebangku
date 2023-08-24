using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class text_manager : MonoBehaviour
{
    public static text_manager instance; // Membuat instance yang dapat diakses dari semua scene
    public string storedText; // Variabel untuk menyimpan teks

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Tetapkan objek ini ketika berpindah scene
        }
        else
        {
            Destroy(gameObject); // Hancurkan objek jika instance sudah ada
        }
    }

    public void StoreText(string text)
    {
        storedText = text; // Simpan teks dari InputField
    }

    public string GetStoredText()
    {
        return storedText; // Ambil teks yang disimpan
    }
}
