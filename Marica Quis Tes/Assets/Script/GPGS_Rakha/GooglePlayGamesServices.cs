using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using TMPro;

public class GooglePlayGamesServices : MonoBehaviour
{
    [SerializeField] private GameObject popupLoginFailed;
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
                // unlock success login achievement

                GetComponent<Achievements>().UnlockSuccessLoginAchievement();
            }
            else
            {
                Debug.Log("Login Failed");
                popupLoginFailed.SetActive(true);
            }
        });
    }

    public void SignInButton()
    {
        SignIn();
    }
}
