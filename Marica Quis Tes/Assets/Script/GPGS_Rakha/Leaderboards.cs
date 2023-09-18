using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Leaderboards : MonoBehaviour
{

    public void ShowLeaderboardUI()
    {
        Social.ShowLeaderboardUI();
    }

    public void AddScoreToLeaderboard(int score)
    {
        if (Social.localUser.authenticated)
        {
            Social.ReportScore(score, "CgkIlO33uugJEAIQBA", success =>
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
}
