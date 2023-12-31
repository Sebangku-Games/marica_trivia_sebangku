using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Leaderboards : MonoBehaviour
{
    const string IdFinansial = "CgkIlO33uugJEAIQDQ";
    const string IdBacaTulis = "CgkIlO33uugJEAIQDg";
    const string IdNumerasi = "CgkIlO33uugJEAIQDw";
    const string IdSains = "CgkIlO33uugJEAIQEA";
    const string IdDigital = "CgkIlO33uugJEAIQEQ";
    const string IdLeaderboardsOverall = "CgkIlO33uugJEAIQBA";

    [SerializeField] private GameObject loginPanel;

    public void ShowLeaderboardUI()
    {
        // check if player is authenticated
        if (Social.localUser.authenticated) // player has login
        {
            // show leaderboard
            Social.ShowLeaderboardUI();
        } else { 
            // player hasnt login, try authenticate
            Social.localUser.Authenticate(success =>
            {
                if (success)
                {
                    Social.ShowLeaderboardUI();
                }
                else
                { 
                    loginPanel.SetActive(true);
                }
            });
        }
    }

    // public void AddScoreToLeaderboard(int score)
    // {
    //     if (Social.localUser.authenticated)
    //     {
    //         Social.ReportScore(score, "CgkIlO33uugJEAIQBA", success =>
    //         {
    //             if (success)
    //             {
    //                 Debug.Log("Update Score Success");
    //             }
    //             else
    //             {
    //                 Debug.Log("Update Score Failed");
    //             }
    //         });
    //     }
    // }

    // Add score to leaderboard for each scene
    public void AddScoreToLeaderboard(int score)
    {
        if (Social.localUser.authenticated)
        {
            var sceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

            switch(sceneIndex){
                case "kuis1" :
                    ReportScore(score, IdFinansial);
                    ReportScore(score, IdLeaderboardsOverall);
                    break;

                case "kuis2" :
                    ReportScore(score, IdBacaTulis);
                    ReportScore(score, IdLeaderboardsOverall);
                    break;

                case "kuis3" :
                    ReportScore(score, IdNumerasi);
                    ReportScore(score, IdLeaderboardsOverall);
                    break;
                
                case "kuis4" :
                    ReportScore(score, IdDigital);
                    ReportScore(score, IdLeaderboardsOverall);
                    break;

                case "kuis5" :
                    ReportScore(score, IdSains);
                    ReportScore(score, IdLeaderboardsOverall);
                    break;
            }
        }
    }

    public void ReportScore(int score, string leaderboardId)
    {
        Social.ReportScore(score, leaderboardId, success =>
        {
            if (success)
            {
                Debug.Log("Update Score Success");
            }
            else
            {
                Debug.Log("Update Score Failed");
            }
        });
    }


}
