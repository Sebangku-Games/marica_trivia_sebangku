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

    public void ShowLeaderboardUI()
    {
        Social.ShowLeaderboardUI();
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
                    break;

                case "kuis2" :
                    ReportScore(score, IdBacaTulis);
                    break;

                case "kuis3" :
                    ReportScore(score, IdNumerasi);
                    break;
                
                case "kuis4" :
                    ReportScore(score, IdSains);
                    break;

                case "kuis5" :
                    ReportScore(score, IdDigital);
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
