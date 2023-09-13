using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using TMPro;

public class GooglePlayGamesServices : MonoBehaviour
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
                // unlock success login achievement
                UnlockSuccessLoginAchievement();
            }
            else
            {
                Debug.Log("Login Failed");
                debugText.text = "Login Failed";
            }
        });
    }

    public void SignInButton()
    {
        SignIn();
    }

    public void ShowAchievementUI()
    {
        Social.ShowAchievementsUI();
    }

    // unlock achievement success login
    public void UnlockSuccessLoginAchievement()
    {
        Social.ReportProgress("CgkIlO33uugJEAIQAg", 100f, success =>
        {
            if (success)
            {
                Debug.Log("Achievement Unlocked");
                debugText.text = "Achievement Unlocked";
            }
            else
            {
                Debug.Log("Achievement Failed");
                debugText.text = "Achievement Failed";
            }
        });
    }

    // unlock achievement ZETAAA UWEEEEEEEEEEEEEEEEEEE
    public void UnlockAchievementLOL(){
        Social.ReportProgress("CgkIlO33uugJEAIQAw", 100f, success => {
            if (success)
            {
                Debug.Log("Achievement Unlocked");
                debugText.text = "Achievement Unlocked";
            }
            else
            {
                Debug.Log("Achievement Failed");
                debugText.text = "Achievement Failed";
            }
        });
    }

    
}
