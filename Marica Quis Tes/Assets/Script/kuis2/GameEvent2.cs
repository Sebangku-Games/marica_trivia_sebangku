using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvents2", menuName = "Quiz/new GameEvents2")]

public class GameEvent2 : ScriptableObject
{
    public delegate void UpdateQuestionUICallback(Pertanyaan2 pertanyaan2);
    public UpdateQuestionUICallback UpdateQuestionUI = null;

    public delegate void UpdateQuestionAnswerCallback(DataJawaban2 pickedAnswer);
    public UpdateQuestionAnswerCallback UpdateQuestionAnswer = null;

    public delegate void DisplayResolutionScreenCallback(UIManager2.ResolutionScreenType type, int score);
    public DisplayResolutionScreenCallback DisplayResolutionScreen = null;

    public delegate void ScoreUpdatedCallback();
    public ScoreUpdatedCallback ScoreUpdated = null;

    [HideInInspector]
    public int CurrentFinalScore = 0;
    [HideInInspector]
    public int StartupHighscore = 0;
}
