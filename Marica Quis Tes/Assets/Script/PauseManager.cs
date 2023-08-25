using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;
    private float savedTimeScale;

    public Button pauseButton;
    //public Text buttonText;
    // Start is called before the first frame update
    void Start()
    {
        // Menghubungkan fungsi ke tombol saat awal permainan
        pauseButton.onClick.AddListener(TogglePause);
        UpdateButtonText();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
    }
    public void TogglePause()
    {
        if (isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }

        
    }

    void UpdateButtonText()
    {
        //buttonText.text = isPaused ? "Resume" : "Pause";
    }

    void Pause()
    {
        isPaused = true;
        savedTimeScale = Time.timeScale;
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = savedTimeScale;
    }
}
