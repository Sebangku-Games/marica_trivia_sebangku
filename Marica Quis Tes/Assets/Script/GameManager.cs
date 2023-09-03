using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class GameManager : MonoBehaviour
{
    #region Variables

    public UIElements uIElemets; // Reference to the UIManager script

    private float previousTime = 0.0f;
    private bool isTimerRunning = false;

    
    private bool isPaused = false;

    public Data data = new Data();
    private List<DataJawaban> currentAnswers = new List<DataJawaban>();

    [SerializeField] GameEvent events = null;

    [SerializeField] Animator timerAnimtor = null;
    [SerializeField] TextMeshProUGUI timerText = null;
    [SerializeField] Color timerHalfWayOutColor = Color.yellow;
    [SerializeField] Color timerAlmostOutColor = Color.red;
    private Color timerDefaultColor = Color.white;

    private List<DataJawaban> PickedAnswers = new List<DataJawaban>();
    private List<int> FinishedQuestions = new List<int>();
    public int currentQuestion = 0;

    private int timerStateParaHash = 0;
    public bool isCorrect;

    private IEnumerator IE_WaitTillNextRound = null;
    private IEnumerator IE_StartTimer = null;
    public event Action <bool> GamePaused;

    //private List<DataJawaban> currentAnswers = new List<DataJawaban>();

    private bool IsFinished
    {
        get
        {
            return (FinishedQuestions.Count < data.pertanyaans.Length) ? false : true;
        }
    }

    #endregion

    #region Default Unity methods

    /// <summary>
    /// Function that is called when the object becomes enabled and active
    /// </summary>
    void OnEnable()
    {
        events.UpdateQuestionAnswer += UpdateAnswers;
    }
    /// <summary>
    /// Function that is called when the behaviour becomes disabled
    /// </summary>
    void OnDisable()
    {
        events.UpdateQuestionAnswer -= UpdateAnswers;
    }

    /// <summary>
    /// Function that is called on the frame when a script is enabled just before any of the Update methods are called the first time.
    /// </summary>
    void Awake()
    {
        events.CurrentFinalScore = 0;
    }
    /// <summary>
    /// Function that is called when the script instance is being loaded.
    /// </summary>
    void Start()
    {
        //uIElemets = GetComponent<UIElements>();
        GamePaused += TogglePause;
        events.StartupHighscore = PlayerPrefs.GetInt(GameUtility.SavePrefKey);

        timerDefaultColor = timerText.color;
        LoadData();

        timerStateParaHash = Animator.StringToHash("TimerState");

        var seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        UnityEngine.Random.InitState(seed);

        Display();
    }

    #endregion

    /// <summary>
    /// Function that is called to update new selected answer.
    /// </summary>
    public void UpdateAnswers(DataJawaban newAnswer)
    {
        if (data.pertanyaans[currentQuestion].Type == AnswerType.Single)
        {
            foreach (var answer in PickedAnswers)
            {
                if (answer != newAnswer)
                {
                    answer.Reset();
                }
            }
            PickedAnswers.Clear();
            PickedAnswers.Add(newAnswer);
        }
        else
        {
            bool alreadyPicked = PickedAnswers.Exists(x => x == newAnswer);
            if (alreadyPicked)
            {
                PickedAnswers.Remove(newAnswer);
            }
            else
            {
                PickedAnswers.Add(newAnswer);
            }
        }
    }

    private void TogglePause(bool pause)
    {
        isPaused = pause;
        if (isPaused)
        {
            // Jika permainan dijeda, hentikan timer
            UpdateTimer(false);
            // Mungkin Anda ingin menampilkan UI jeda di sini
        }
        else
        {
            // Jika permainan dilanjutkan, aktifkan kembali timer
            UpdateTimer(true);
            // Mungkin Anda ingin menyembunyikan UI jeda di sini
        }
    }

    public void TogglePauseButton()
    {
        if (GamePaused != null)
        {
            // Jika tombol pause ditekan, kirim status jeda terbalik
            GamePaused(!isPaused);

            if (isPaused)
            {
                if (isTimerRunning)
                {
                    UpdateTimer(false);
                    previousTime = float.Parse(timerText.text);
                }
            }
            else
            {
                if (isTimerRunning)
                {
                    UpdateTimer(true);
                    StartCoroutine(IE_StartTimer); // Lanjutkan timer yang dihentikan sebelumnya
                }
            }
        }

    }

    public void ResumeGame()
    {
        if (GamePaused != null)
        {
            // Jika tombol resume ditekan, kirim status jeda sebagai false
            GamePaused(false);
        }
    }

    /// <summary>
    /// Function that is called to clear PickedAnswers list.
    /// </summary>
    public void EraseAnswers()
    {
        PickedAnswers = new List<DataJawaban>();
    }

    /// <summary>
    /// Function that is called to display new question.
    /// </summary>
    void Display()
    {
        EraseAnswers();
        var question = GetRandomQuestion();

        if (events.UpdateQuestionUI != null)
        {
            events.UpdateQuestionUI(question);
        }
        else { Debug.LogWarning("Ups! Something went wrong while trying to display new Question UI Data. GameEvents.UpdateQuestionUI is null. Issue occured in GameManager.Display() method."); }

        // Panggil ShowHintButton setelah menampilkan pertanyaan
        //uIElemets.HintButton.GetComponent<hintButton>().ShowHintButton();

        if (question.UseTimer)
        {
            UpdateTimer(question.UseTimer);
        }
    }

    /// <summary>
    /// Function that is called to accept picked answers and check/display the result.
    /// </summary>
    public void Accept()
    {
        UpdateTimer(false);
        isCorrect = CheckAnswers();
        FinishedQuestions.Add(currentQuestion);

        UpdateScore((isCorrect) ? data.pertanyaans[currentQuestion].AddScore : -data.pertanyaans[currentQuestion].AddScore);

        if (IsFinished)
        {
            //events.level++; //nambah level
            if(events.level > GameEvent.maxLevel)
            {
                events.level = 1;
            }
            SetHighscore();
        }

        var type
            = (IsFinished)
            ? UIManager.ResolutionScreenType.Finish
            : (isCorrect) ? UIManager.ResolutionScreenType.Correct
            : UIManager.ResolutionScreenType.Incorrect;

        if (events.DisplayResolutionScreen != null)
        {
            events.DisplayResolutionScreen(type, data.pertanyaans[currentQuestion].AddScore);
        }

        AudioManager.Instance.PlaySound((isCorrect) ? "CorrectSFX" : "IncorrectSFX");

        if (type != UIManager.ResolutionScreenType.Finish)
        {
            if (IE_WaitTillNextRound != null)
            {
                StopCoroutine(IE_WaitTillNextRound);
            }
            IE_WaitTillNextRound = WaitTillNextRound();
            StartCoroutine(IE_WaitTillNextRound);
        }
    }

    #region Timer Methods

    public void UpdateTimer(bool state, float timeToAdd = 0)
    {
        switch (state)
        {
            case true:
                IE_StartTimer = StartTimer(timeToAdd);
                StartCoroutine(IE_StartTimer);

                timerAnimtor.SetInteger(timerStateParaHash, 2);
                break;
            case false:
                if (IE_StartTimer != null)
                {
                    StopCoroutine(IE_StartTimer);
                }

                timerAnimtor.SetInteger(timerStateParaHash, 1);
                break;
        }

    }
    IEnumerator StartTimer(float timeToAdd)
    {
        isTimerRunning = true;
        // Simpan waktu awal
        //var timeLeft = originalTime;
        var totalTime = data.pertanyaans[currentQuestion].Timer;
        //var timeLeft = totalTime;
        var timeLeft = (previousTime > 0.0f) ? previousTime : totalTime;

        timerText.color = timerDefaultColor;
        while (timeLeft > 0)
        {
            timeLeft--;

            AudioManager.Instance.PlaySound("CountdownSFX");

            if (timeLeft < totalTime / 2 && timeLeft > totalTime / 4)
            {
                timerText.color = timerHalfWayOutColor;
            }
            if (timeLeft < totalTime / 4)
            {
                timerText.color = timerAlmostOutColor;
            }
            // Debug.Log("Time Left : "+ timeLeft);

            timerText.text = timeLeft.ToString();
            yield return new WaitForSeconds(1.0f);
        }
        isTimerRunning = false;
        Accept();
    }

    internal Pertanyaan GetCurrentQuestion()
    {
        throw new NotImplementedException();
    }

    IEnumerator WaitTillNextRound()
    {
        yield return new WaitForSeconds(GameUtility.ResolutionDelayTime);
        Display();
    }

    #endregion

    /// <summary>
    /// Function that is called to check currently picked answers and return the result.
    /// </summary>
    public bool CheckAnswers()
    {
        if (!CompareAnswers())
        {
            return false;
        }
        return true;
    }
    /// <summary>
    /// Function that is called to compare picked answers with question correct answers.
    /// </summary>
    bool CompareAnswers()
    {
        if (PickedAnswers.Count > 0)
        {
            List<int> c = data.pertanyaans[currentQuestion].GetCorrectAnswers();
            List<int> p = PickedAnswers.Select(x => x.AnswerIndex).ToList();

            var f = c.Except(p).ToList();
            var s = p.Except(c).ToList();

            return !f.Any() && !s.Any();
        }
        return false;
    }

    /// <summary>
    /// Function that is called to load all questions from the Resource folder.
    /// </summary>
    void LoadData()
    {
        var path = Path.Combine(GameUtility.fileDir, GameUtility.fileName + events.level + ".xml");
        data = Data.Fetch(path);
    }

    /// <summary>
    /// Function that is called restart the game.
    /// </summary>
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    /// <summary>
    /// Function that is called to quit the application.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Function that is called to set new highscore if game score is higher.
    /// </summary>
    private void SetHighscore()
    {
        var highscore = PlayerPrefs.GetInt(GameUtility.SavePrefKey);
        if (highscore < events.CurrentFinalScore)
        {
            PlayerPrefs.SetInt(GameUtility.SavePrefKey, events.CurrentFinalScore);
        }
    }
    /// <summary>
    /// Function that is called update the score and update the UI.
    /// </summary>
    private void UpdateScore(int add)
    {
        events.CurrentFinalScore += add;
        if(events.CurrentFinalScore > 0)
        {
            events.CurrentFinalScore = 0;
        }

        if (events.ScoreUpdated != null)
        {
            events.ScoreUpdated();
        }
    }

    #region Getters

    Pertanyaan GetRandomQuestion()
    {
        var randomIndex = GetRandomQuestionIndex();
        currentQuestion = randomIndex;

        return data.pertanyaans[currentQuestion];
    }
    int GetRandomQuestionIndex()
    {
        var random = 0;
        if (FinishedQuestions.Count < data.pertanyaans.Length)
        {
            do
            {
                random = UnityEngine.Random.Range(0, data.pertanyaans.Length);
            } while (FinishedQuestions.Contains(random) || random == currentQuestion);
        }
        return random;

    }

    




}





    #endregion

