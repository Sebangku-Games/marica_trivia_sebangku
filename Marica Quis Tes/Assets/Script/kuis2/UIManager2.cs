using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager2 : MonoBehaviour
{
    #region Variables

    public GameManager2 gameManager;

    public enum ResolutionScreenType { Correct, Incorrect, Finish }

    [Header("References")]
    [SerializeField] GameEvent2 events = null;

    [Header("UI Elements (Prefabs)")]
    [SerializeField] DataJawaban2 answerPrefab = null;

    [SerializeField] UIElements uIElements = new UIElements();

    [Space]
    [SerializeField] UIManagerParameter parameters = new UIManagerParameter();

    private List<DataJawaban2> currentAnswers = new List<DataJawaban2>();
    private int resStateParaHash = 0;

    private IEnumerator IE_DisplayTimedResolution = null;

    #endregion

    #region Default Unity methods

    void OnEnable()
    {
        events.UpdateQuestionUI += UpdateQuestionUI;
        events.DisplayResolutionScreen += DisplayResolution;
        events.ScoreUpdated += UpdateScoreUI;
    }

    void OnDisable()
    {
        events.UpdateQuestionUI -= UpdateQuestionUI;
        events.DisplayResolutionScreen -= DisplayResolution;
        events.ScoreUpdated -= UpdateScoreUI;
    }

    void Start()
    {
        UpdateScoreUI();
        resStateParaHash = Animator.StringToHash("ScreenState");
    }

    #endregion
    /// <summary>
    /// Function that is used to update new question UI information.
    /// </summary>
    void UpdateQuestionUI(Pertanyaan2 pertanyaann)
    {

        uIElements.QuestionInfoTextObject.text = pertanyaann.Info;
        CreateAnswers(pertanyaann);
    }
    /// <summary>
    /// Function that is used to display resolution screen.
    /// </summary>
    void DisplayResolution(ResolutionScreenType type, int score)
    {
        UpdateResUI(type, score);
        uIElements.ResolutionScreenAnimator.SetInteger(resStateParaHash, 2);
        uIElements.MainCanvasGroup.blocksRaycasts = false;

        if (type != ResolutionScreenType.Finish)
        {
            if (IE_DisplayTimedResolution != null)
            {
                StopCoroutine(IE_DisplayTimedResolution);
            }
            IE_DisplayTimedResolution = DisplayTimedResolution();
            StartCoroutine(IE_DisplayTimedResolution);
        }
    }
    IEnumerator DisplayTimedResolution()
    {
        yield return new WaitForSeconds(GameUtility.ResolutionDelayTime);
        uIElements.ResolutionScreenAnimator.SetInteger(resStateParaHash, 1);
        uIElements.MainCanvasGroup.blocksRaycasts = true;
    }

    /// <summary>
    /// Function that is used to display resolution UI information.
    /// </summary>
    void UpdateResUI(ResolutionScreenType type, int score)
    {
        var highscore = PlayerPrefs.GetInt(GameUtility.SavePrefKey);

        switch (type)
        {
            case ResolutionScreenType.Correct:
                uIElements.ResolutionBG.color = parameters.CorrectBGColor;
                uIElements.ResolutionStateInfoText.text = "CORRECT!";
                uIElements.ResolutionScoreText.text = "+" + score;
                break;
            case ResolutionScreenType.Incorrect:
                uIElements.ResolutionBG.color = parameters.IncorrectBGColor;
                uIElements.ResolutionStateInfoText.text = "WRONG!";
                uIElements.ResolutionScoreText.text = "-" + score;
                break;
            case ResolutionScreenType.Finish:
                uIElements.ResolutionBG.color = parameters.FinalBGColor;
                uIElements.ResolutionStateInfoText.text = "FINAL SCORE";

                StartCoroutine(CalculateScore());
                uIElements.FinishUIElements.gameObject.SetActive(true);
                uIElements.HighScoreText.gameObject.SetActive(true);
                uIElements.HighScoreText.text = ((highscore > events.StartupHighscore) ? "<color=yellow>new </color>" : string.Empty) + "Highscore: " + highscore;
                break;
        }
    }

    /// <summary>
    /// Function that is used to calculate and display the score.
    /// </summary>
    IEnumerator CalculateScore()
    {
        var scoreValue = 0;
        while (scoreValue < events.CurrentFinalScore)
        {
            scoreValue++;
            uIElements.ResolutionScoreText.text = scoreValue.ToString();

            yield return null;
        }
    }

    /// <summary>
    /// Function that is used to create new question answers.
    /// </summary>
    void CreateAnswers(Pertanyaan2 pertanyaann)
    {
        EraseAnswers();

        float offset = 0 - parameters.Margins;
        for (int i = 0; i < pertanyaann.Answers.Length; i++)
        {
            DataJawaban2 newAnswer = (DataJawaban2)Instantiate(answerPrefab, uIElements.AnswersContentArea);
            newAnswer.UpdateData2(pertanyaann.Answers[i].Info, i);

            newAnswer.Rect.anchoredPosition = new Vector2(0, offset);

            offset -= (newAnswer.Rect.sizeDelta.y + parameters.Margins);
            uIElements.AnswersContentArea.sizeDelta = new Vector2(uIElements.AnswersContentArea.sizeDelta.x, offset * -1);

            

            currentAnswers.Add(newAnswer);
        }
    }
    /// <summary>
    /// Function that is used to erase current created answers.
    /// </summary>
    void EraseAnswers()
    {
        foreach (var answer in currentAnswers)
        {
            Destroy(answer.gameObject);
        }
        currentAnswers.Clear();
    }

    /// <summary>
    /// Function that is used to update score text UI.
    /// </summary>
    void UpdateScoreUI()
    {
        uIElements.ScoreText.text = "Score: " + events.CurrentFinalScore;
    }

    public void AddTimeAndReduceScore()
    {
        float additionalTime = 30f;
        gameManager.UpdateTimer(false);
        // Tambah waktu sebanyak 30 detik
        gameManager.UpdateTimer(true, additionalTime);

        // Tambah waktu sebanyak 30 detik
        gameManager.UpdateTimer(true, additionalTime);

        // Kurangi skor sebanyak 10
        events.CurrentFinalScore -= 10;

        // Update UI skor
        UpdateScoreUI();
    }

    
}









