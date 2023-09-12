using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using TMPro;

public class GoogleSignIn : MonoBehaviour
{
    [SerializeField] private TMP_Text debugText;
    // create script to auto sign in user to google play games
    void Start()
    {
        PlayGamesPlatform.Activate();
        SignIn();
    }

    void SignIn()
    {
        Social.localUser.Authenticate(success =>
        {
            if (success)
            {
                Debug.Log("Login Success");
                debugText.text = "Login Success";
            }
            else
            {
                Debug.Log("Login Failed");
                debugText.text = "Login Failed";
            }
        });
    }

    public void ShowLeaderboard()
    {
        Social.ShowLeaderboardUI();
    }

    
}
