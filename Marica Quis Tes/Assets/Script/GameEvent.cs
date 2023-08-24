using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvents", menuName = "Quiz/new GameEvents")]

public class GameEvent : ScriptableObject
{
    public delegate void UpdateQuestionUICallback(Pertanyaan pertanyaan);
    public UpdateQuestionUICallback UpdatePertanyaanUI;

    public delegate void UpdateQuestionAnswerCallback(DataJawaban dataJawaban);
    public UpdateQuestionAnswerCallback UpdateQuestionAnswer;

    public delegate void DisplayResolutionScreenCallback(UIManager.ResolutionScreenType type, int score);
    public DisplayResolutionScreenCallback displayResolutionScreen;

    public delegate void ScoreUpdatedCallback();
    public ScoreUpdatedCallback ScoreUpdated;

    [HideInInspector]
    public int CurrentFinalScore;

    [HideInInspector]
    public int StartupHighscore;
}
