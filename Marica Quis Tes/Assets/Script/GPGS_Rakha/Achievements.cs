using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Achievements : MonoBehaviour
{
    const string AIdFinansial = "CgkIlO33uugJEAIQBw";
    const string AIdBacaTulis = "CgkIlO33uugJEAIQCA";
    const string AIdNumerasi = "CgkIlO33uugJEAIQCQ";
    const string AIdSains = "CgkIlO33uugJEAIQCg";
    const string AIdDigital = "CgkIlO33uugJEAIQCw";
    const string AIdAllLiteracy = "CgkIlO33uugJEAIQDA";

    const string AIdSignInSuccess = "CgkIlO33uugJEAIQAg";
    const string AIdTestLOL = "CgkIlO33uugJEAIQAw";

    public void ShowAchievementUI()
    {
        Social.ShowAchievementsUI();
    }

    // unlock achievement success login
    public void UnlockSuccessLoginAchievement()
    {
        ReportProgressAchievement(AIdSignInSuccess, 100f);
    }

    // unlock achievement ZETAAA UWEEEEEEEEEEEEEEEEEEE
    public void UnlockAchievementLOL(){
        ReportProgressAchievement(AIdTestLOL, 100f);
    }

    // unlock achievement for each scene
    public void UnlockAchievement()
    {
        var sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        switch (sceneName)
        {
            case "kuis1":
                ReportProgressAchievement(AIdFinansial, 100f);
                break;

            case "kuis2":
                ReportProgressAchievement(AIdBacaTulis, 100f);
                break;

            case "kuis3":
                ReportProgressAchievement(AIdNumerasi, 100f);
                break;

            case "kuis4":
                ReportProgressAchievement(AIdSains, 100f);
                break;

            case "kuis5":
                ReportProgressAchievement(AIdDigital, 100f);
                break;
        }
    }

    public void CheckAllLiteracyAchievement(){
        string[] requiredAchievement = {AIdFinansial, AIdBacaTulis, AIdNumerasi, AIdSains, AIdDigital};

        // check if all literacy achievement is unlocked then unlock AIdAllLiteracy
        Social.LoadAchievements(achievements =>
        {
            if (achievements.Length > 0)
            {
                foreach (IAchievement achievement in achievements)
                {
                    if (requiredAchievement.Contains(achievement.id) && achievement.completed)
                    {
                        Debug.Log("Achievement " + achievement.id + " is completed");
                    }
                    else
                    {
                        Debug.Log("Achievement " + achievement.id + " is not completed");
                        return;
                    }
                }
                // if all achievement is completed
                ReportProgressAchievement(AIdAllLiteracy, 100f);
            }
            else
            {
                Debug.Log("No achievement found");
            }
        });
    }

    public void ReportProgressAchievement(string achievementId, float progress){
        Social.ReportProgress(achievementId, progress, success => {
            if (success)
            {
                Debug.Log("Achievement Unlocked");
            }
            else
            {
                Debug.Log("Achievement Failed");
            }
        });
    }

}
