using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class redeem : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI buttonText;
    public GameObject prizePanel;
    public GameObject notEnough;


    private void Start()
    {
        UpdateScoreDisplay();
    }
    void UpdateScoreDisplay()
    {
        // Mengecek apakah highscore tersimpan di PlayerPrefs
        if (PlayerPrefs.HasKey(GameUtility.SavePrefKey))
        {
            int highscore = PlayerPrefs.GetInt(GameUtility.SavePrefKey);
            scoreText.text = highscore.ToString();
        }
        else
        {
            scoreText.text = "Tidak Ada Coin";
        }
    }

    public void RedeemVoucher()
    {
        if (PlayerPrefs.HasKey(GameUtility.SavePrefKey))
        {
            int highscore = PlayerPrefs.GetInt(GameUtility.SavePrefKey);
            highscore -= 50; //harga voucher
            PlayerPrefs.SetInt(GameUtility.SavePrefKey, highscore);
            PlayerPrefs.Save(); // Save the updated highscore
            UpdateScoreDisplay(); // Update the displayed highscore
            prizePanel.SetActive(true);
            
        }
    }

    public void tesRedeem()
    {
        if (PlayerPrefs.HasKey(GameUtility.SavePrefKey))
        {
            int highscore = PlayerPrefs.GetInt(GameUtility.SavePrefKey);

            if (highscore >= 50)
            {
                highscore -= 50;
                PlayerPrefs.SetInt(GameUtility.SavePrefKey, highscore);
                PlayerPrefs.Save(); // Save the updated highscore
                UpdateScoreDisplay(); // Update the displayed highscore
            }
            else
            {
                notEnough.SetActive(true);
                Debug.Log("Not enough coins!"); // Print debug message if coins are not enough
            }
        }
    }

    public void Back()
    {
        prizePanel.SetActive(false);
    }

    
}
