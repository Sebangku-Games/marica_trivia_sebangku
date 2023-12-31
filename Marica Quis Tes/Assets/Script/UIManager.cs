using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;


[System.Serializable]
public struct UIManagerParameter
{
    [Header("Answers Options")]
    [SerializeField] float margins;
    public float Margins { get { return margins; } }

    [Header("Resolution Screen Options")]
    [SerializeField] Color correctBGColor;
    public Color CorrectBGColor { get { return correctBGColor; } }
    [SerializeField] Color incorrectBGColor;
    public Color IncorrectBGColor { get { return incorrectBGColor; } }
    [SerializeField] Color finalBGColor;
    public Color FinalBGColor { get { return finalBGColor; } }
}

[System.Serializable]
public struct UIElements
{
    [SerializeField] RectTransform answersContentArea;
    public RectTransform AnswersContentArea { get { return answersContentArea; } }

    [SerializeField] TextMeshProUGUI questionInfoTextObject;
    public TextMeshProUGUI QuestionInfoTextObject { get { return questionInfoTextObject; } }

    [SerializeField] TextMeshProUGUI scoreText;
    public TextMeshProUGUI ScoreText { get { return scoreText; } }

    [Space]

    [SerializeField] Animator resolutionScreenAnimator;
    public Animator ResolutionScreenAnimator { get { return resolutionScreenAnimator; } }

    [SerializeField] Image resolutionBG;
    public Image ResolutionBG { get { return resolutionBG; } }

    [SerializeField] TextMeshProUGUI resolutionStateInfoText;
    public TextMeshProUGUI ResolutionStateInfoText { get { return resolutionStateInfoText; } }

    [SerializeField] TextMeshProUGUI resolutionScoreText;
    public TextMeshProUGUI ResolutionScoreText { get { return resolutionScoreText; } }

    [Space]

    [SerializeField] TextMeshProUGUI highScoreText;
    public TextMeshProUGUI HighScoreText { get { return highScoreText; } }

    [SerializeField] CanvasGroup mainCanvasGroup;
    public CanvasGroup MainCanvasGroup { get { return mainCanvasGroup; } }

    [SerializeField] RectTransform finishUIElements;
    public RectTransform FinishUIElements { get { return finishUIElements; } }

    [SerializeField] RectTransform hintButton;
    public RectTransform HintButton { get { return hintButton; } }

    [SerializeField] RectTransform hintButton2;
    public RectTransform HintButton2 { get { return hintButton2; } }

    [SerializeField] RectTransform popupTidak;
    public RectTransform PopUpTidak { get { return popupTidak; } }
}





public class UIManager : MonoBehaviour
{


    #region Variables


    public GameManager gameManager;

    public enum ResolutionScreenType { Correct, Incorrect, Finish }

    [Header("References")]
    [SerializeField] GameEvent events = null;

    [Header("UI Elements (Prefabs)")]
    [SerializeField] DataJawaban answerPrefab = null;

    [SerializeField] UIElements uIElements = new UIElements();
    [SerializeField] ParticleSystem correctAnswerParticles;

    [Space]
    [SerializeField] UIManagerParameter parameters = new UIManagerParameter();

    private List<DataJawaban> currentAnswers = new List<DataJawaban>();
    private List<DataJawaban> dataJawaban = new List<DataJawaban>();
    private int resStateParaHash = 0;

    private IEnumerator IE_DisplayTimedResolution = null;

    #endregion

    #region Default Unity methods

    /// <summary>
    /// Function that is called when the object becomes enabled and active
    /// </summary>
    void OnEnable()
    {
        events.UpdateQuestionUI += UpdateQuestionUI;
        events.DisplayResolutionScreen += DisplayResolution;
        events.ScoreUpdated += UpdateScoreUI;
    }
    /// <summary>
    /// Function that is called when the behaviour becomes disabled
    /// </summary>
    void OnDisable()
    {
        events.UpdateQuestionUI -= UpdateQuestionUI;
        events.DisplayResolutionScreen -= DisplayResolution;
        events.ScoreUpdated -= UpdateScoreUI;
    }

    /// <summary>
    /// Function that is called when the script instance is being loaded.
    /// </summary>
    void Start()
    {
        UpdateScoreUI();
        resStateParaHash = Animator.StringToHash("ScreenState");

    }

    #endregion

    /// <summary>
    /// Function that is used to update new question UI information.
    /// </summary>
    void UpdateQuestionUI(Pertanyaan pertanyaan)
    {

        uIElements.QuestionInfoTextObject.text = pertanyaan.Info;
        CreateAnswers(pertanyaan);
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
                uIElements.ResolutionStateInfoText.text = "Jawaban Kamu Benar!";
                uIElements.ResolutionScoreText.text = "+" + score;

                // Mengaktifkan Particle System saat jawaban benar
                if (correctAnswerParticles != null)
                {
                    correctAnswerParticles.gameObject.SetActive(true);
                    correctAnswerParticles.Play();
                }
                break;
            case ResolutionScreenType.Incorrect:
                uIElements.ResolutionBG.color = parameters.IncorrectBGColor;
                uIElements.ResolutionStateInfoText.text = "Maaf, Jawaban Kamu Salah!";
                uIElements.ResolutionScoreText.text = "-" + score;
                break;
            case ResolutionScreenType.Finish:
                uIElements.ResolutionBG.color = parameters.FinalBGColor;
                uIElements.ResolutionStateInfoText.text = "Skor Akhir Kamu adalah";

                StartCoroutine(CalculateScore());
                uIElements.FinishUIElements.gameObject.SetActive(true);
                uIElements.HighScoreText.gameObject.SetActive(true);
                uIElements.HighScoreText.text = ((highscore > events.StartupHighscore) ? "<color=yellow>new </color>" : string.Empty) + "Highscore: " + highscore;
                
                GetComponent<Leaderboards>().AddScoreToLeaderboard(events.CurrentFinalScore);
                break;
        }
    }

    /// <summary>
    /// Function that is used to calculate and display the score.
    /// </summary>
    IEnumerator CalculateScore()
    {
        if (events.CurrentFinalScore == 0) { uIElements.ResolutionScoreText.text = 0.ToString(); yield break; }

        // Get Achievement in score 100
        if (events.CurrentFinalScore == 100)
        {
            GetComponent<Achievements>().UnlockAchievement();
        }

        var scoreValue = 0;
        var scoreMoreThanZero = events.CurrentFinalScore > 0;
        while (scoreMoreThanZero ? scoreValue < events.CurrentFinalScore : scoreValue > events.CurrentFinalScore)
        {
            scoreValue += scoreMoreThanZero ? 1 : -1;
            uIElements.ResolutionScoreText.text = scoreValue.ToString();

            yield return null;
        }

    }

    /// <summary>
    /// Function that is used to create new question answers.
    /// </summary>
    void CreateAnswers(Pertanyaan pertanyaan)
{
    EraseAnswers();

    List<DataJawaban> shuffledAnswers = new List<DataJawaban>();
    List<int> shuffledIndices = new List<int>();

    for (int i = 0; i < pertanyaan.Answers.Length; i++)
    {
        shuffledIndices.Add(i);
    }

    // Shuffle the indices
    System.Random rng = new System.Random();
    int n = shuffledIndices.Count;
    while (n > 1)
    {
        n--;
        int k = rng.Next(n + 1);
        int value = shuffledIndices[k];
        shuffledIndices[k] = shuffledIndices[n];
        shuffledIndices[n] = value;
    }

    // Create and position the answers based on the shuffled indices
    float offset = 0 - parameters.Margins;
    foreach (int index in shuffledIndices)
    {
        DataJawaban newAnswer = (DataJawaban)Instantiate(answerPrefab, uIElements.AnswersContentArea);
        newAnswer.UpdateData(pertanyaan.Answers[index].Info, index);

        newAnswer.Rect.anchoredPosition = new Vector2(0, offset);

        offset -= (newAnswer.Rect.sizeDelta.y + parameters.Margins);
        uIElements.AnswersContentArea.sizeDelta = new Vector2(uIElements.AnswersContentArea.sizeDelta.x, offset * -1);

        dataJawaban.Add(newAnswer);
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
        Debug.Log(uIElements.ScoreText.text = "Score: " + events.CurrentFinalScore);
        uIElements.ScoreText.text = "Score: " + events.CurrentFinalScore;
    }

    public void AddTimeAndReduceScore()
    {
        float additionalTime = 30f;
        if (events.CurrentFinalScore >= 10)
        {
            gameManager.UpdateTimer(false);
            // Tambah waktu sebanyak 30 detik
            gameManager.UpdateTimer(true, additionalTime);


            // Kurangi skor sebanyak 20
            events.CurrentFinalScore -= 10;

            // Update UI skor
            UpdateScoreUI();
            uIElements.HintButton2.gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine(ShowPopUp());
            Debug.Log("Skor Tidak Cukup");
        }
        

    }

    public void RemoveIncorrectAnswer(){
        if(events.CurrentFinalScore >= 10)
        {
            events.CurrentFinalScore -= 10;
            // Update UI skor
            UpdateScoreUI();
            if (gameManager.data.pertanyaans.Length > 0 && gameManager.currentQuestion >= 0 && gameManager.currentQuestion < gameManager.data.pertanyaans.Length)
            {
                Pertanyaan currentQuestion = gameManager.data.pertanyaans[gameManager.currentQuestion];
                List<int> correctAnswers = currentQuestion.GetCorrectAnswers();

                int incorrectRemovedCount = 0; // Counter for removed incorrect options

                // Create a new list to store active DataJawaban objects
                List<DataJawaban> activeDataJawaban = new List<DataJawaban>();

                foreach (var answer in dataJawaban)
                {
                    if (answer != null && answer.gameObject != null)
                    {
                        activeDataJawaban.Add(answer); // Add active objects to the new list
                    }
                }

                foreach (var answer in activeDataJawaban)
                {
                    if (!correctAnswers.Contains(answer.AnswerIndex) && incorrectRemovedCount < 2)
                    {
                        answer.gameObject.SetActive(false);
                        incorrectRemovedCount++; // Increment the counter
                    }
                }
            }
            uIElements.HintButton.gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine(ShowPopUp());
            Debug.Log("Skor Tidak Cukup");
        }
        
    }

    IEnumerator ShowPopUp()
    {
        uIElements.PopUpTidak.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        uIElements.PopUpTidak.gameObject.SetActive(false);
    }

    
}









