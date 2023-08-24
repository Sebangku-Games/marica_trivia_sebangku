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
    [SerializeField] RectTransform jawabanContentArea;
    public RectTransform JawabanContentArea { get { return jawabanContentArea; } }

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


    [SerializeField] TextMeshProUGUI highScoreText;
    public TextMeshProUGUI HighScoreText { get { return highScoreText; } }

    [SerializeField] CanvasGroup mainCanvasGroup;
    public CanvasGroup MainCanvasGroup { get { return mainCanvasGroup; } }

    [SerializeField] RectTransform finishUIElements;
    public RectTransform FinishUIElements { get { return finishUIElements; } }
}





public class UIManager : MonoBehaviour
{
    public enum ResolutionScreenType { Correct, Incorrect, Finish }

    [Header("Reference")]
    [SerializeField] GameEvent events;

    [Header("UI Elements (Prefabs)")]
    [SerializeField] DataJawaban jawabanPrefab;

    [Space]
    [SerializeField] private UIElements elements = new UIElements();
    [SerializeField] private UIManagerParameter parameters = new UIManagerParameter();

    List<DataJawaban> currentJawaban = new List<DataJawaban>();

    private int resStateParaHash = 0;
    private IEnumerator IE_DisplayTimedResolution;

    void OnEnable()
    {
        events.UpdatePertanyaanUI += UpdatePertanyaanUI;
        events.displayResolutionScreen += DisplayResolution;
    }

    void OnDisable()
    {
        events.UpdatePertanyaanUI -= UpdatePertanyaanUI;
        events.displayResolutionScreen -= DisplayResolution;
    }

    void Start()
    {
        resStateParaHash = Animator.StringToHash("ScreenState");
    }

    void UpdatePertanyaanUI(Pertanyaan pertanyaan)
    {
        elements.QuestionInfoTextObject.text = pertanyaan.info;
        //membuat jawaban
        CreateJawaban(pertanyaan);

    }

    void DisplayResolution(ResolutionScreenType type, int score)
    {
        UpdateResUI(type, score);
        elements.ResolutionScreenAnimator.SetInteger(resStateParaHash, 2);
        elements.MainCanvasGroup.blocksRaycasts = false;

        if(type != ResolutionScreenType.Finish)
        {
            if(IE_DisplayTimedResolution != null)
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
        elements.ResolutionScreenAnimator.SetInteger(resStateParaHash, 1);
        elements.MainCanvasGroup.blocksRaycasts = true;
    }

    void UpdateResUI(ResolutionScreenType type, int score)
    {
        var highscore = PlayerPrefs.GetInt(GameUtility.SavePrefKey);
        switch (type)
        {
            case ResolutionScreenType.Correct:
                elements.ResolutionBG.color = parameters.CorrectBGColor;
                elements.ResolutionStateInfoText.text = "JAWABAN BENAR!";
                elements.ResolutionScoreText.text = "+" + score;
                break;
            case ResolutionScreenType.Incorrect:
                elements.ResolutionBG.color = parameters.IncorrectBGColor;
                elements.ResolutionStateInfoText.text = "JAWABAN SALAH";
                elements.ResolutionScoreText.text = "-" + score;
                break;
            case ResolutionScreenType.Finish:
                elements.ResolutionBG.color = parameters.FinalBGColor;
                elements.ResolutionStateInfoText.text = "SCORE AKHIR";

                StartCoroutine(CalculateScore());
                elements.FinishUIElements.gameObject.SetActive(true);
                elements.HighScoreText.gameObject.SetActive(true);
                //display highscore

                elements.HighScoreText.text = ((highscore > events.StartupHighscore) ? "<color=yellow>new </color>" : string.Empty) + "Highscore: " + highscore;
                break;
            default:
                break;
        }
    }

    IEnumerator CalculateScore()
    {
        var scoreValue = 0;

        while (scoreValue < events.CurrentFinalScore)
        {
            scoreValue++;
            elements.ResolutionScoreText.text = scoreValue.ToString();

            yield return null;
        }
    }

    void CreateJawaban(Pertanyaan pertanyaan)
    {
        EraseJawaban();

        float offset = 0 - parameters.Margins;
        for (int i = 0; i < pertanyaan.Jawabans.Length; i++)
        {
            DataJawaban newJawaban = (DataJawaban)Instantiate(jawabanPrefab, elements.JawabanContentArea);
            newJawaban.UpdateData(pertanyaan.Jawabans[i].Info, i);

            newJawaban.Rect.anchoredPosition = new Vector2(0, offset);

            offset -= (newJawaban.Rect.sizeDelta.y + parameters.Margins);
            elements.JawabanContentArea.sizeDelta = new Vector2(elements.JawabanContentArea.sizeDelta.x, offset * -1);

            currentJawaban.Add(newJawaban);
        }
    }

    void EraseJawaban()
    {
        foreach (var jawaban in currentJawaban)
        {
            Destroy(jawaban.gameObject);
        }
        currentJawaban.Clear();
    }



}
