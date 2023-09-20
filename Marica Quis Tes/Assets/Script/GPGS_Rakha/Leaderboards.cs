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
            var sceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;

            switch(sceneIndex){
                case 1 :
                    ReportScore(score, IdFinansial);
                    break;

                case 2 :
                    ReportScore(score, IdBacaTulis);
                    break;

                case 3 :
                    ReportScore(score, IdNumerasi);
                    break;
                
                case 4 :
                    ReportScore(score, IdSains);
                    break;

                case 5 :
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
