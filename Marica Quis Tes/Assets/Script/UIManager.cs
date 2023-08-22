using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

[SerializeField()]
public struct UIManagerParameter
{
    [Header("Answers Options")]
    [SerializeField] float margins;
    public float Margins { get { return margins; } }
}

[SerializeField()]
public struct UIElements
{
    [SerializeField] RectTransform jawabanContentArea;
    public RectTransform JawabanContentArea { get { return jawabanContentArea; } }

    [SerializeField] TextMeshProUGUI questionInfoTextObject;
    public TextMeshProUGUI QuestionInfoTextObject { get { return questionInfoTextObject; } }

    [SerializeField] TextMeshProUGUI scoreText;
    public TextMeshProUGUI ScoreText { get { return scoreText; } }

    [Space]
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

}


    public class UIManager : MonoBehaviour
{
    public enum ResolutionScreenType { Correct, Incorrect, Finish }

    [Header("Reference")]
    [SerializeField] GameEvent events;

    [Header("UI Elements (Prefabs)")]
    [SerializeField] DataJawaban jawabans;

    [SerializeField] UIElements uIElements;

    [Space]
    [SerializeField] UIManagerParameter parameters;

    List<DataJawaban> currentJawaban = new List<DataJawaban>();

    void OnEnable()
    {
        
    }

    void OnDisable()
    {
        
    }

    void UpdatePertanyaanUI(Pertanyaan pertanyaan)
    {
        uIElements.QuestionInfoTextObject.text = pertanyaan.info;
        //membuat jawaban

    }

    void CreateJawaban()
    {

    }

}
