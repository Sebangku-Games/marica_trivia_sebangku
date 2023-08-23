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
    [SerializeField] DataJawaban jawabanPrefab;

    [SerializeField] UIElements uIElements = new UIElements();

    [Space]
    [SerializeField] UIManagerParameter parameters = new UIManagerParameter();

    List<DataJawaban> currentJawaban = new List<DataJawaban>();

    void OnEnable()
    {
        events.UpdatePertanyaanUI += UpdatePertanyaanUI;
    }

    void OnDisable()
    {
        events.UpdatePertanyaanUI -= UpdatePertanyaanUI;
    }

    void UpdatePertanyaanUI(Pertanyaan pertanyaan)
    {
        uIElements.QuestionInfoTextObject.text = pertanyaan.info;
        //membuat jawaban
        CreateJawaban(pertanyaan);

    }

    void CreateJawaban(Pertanyaan pertanyaan)
    {
        EraseJawaban();

        float offset = 0 - parameters.Margins;
        for (int i = 0; i < pertanyaan.Jawabans.Length; i++)
        {
            DataJawaban newJawaban = (DataJawaban)Instantiate(jawabanPrefab, uIElements.JawabanContentArea);
            newJawaban.UpdateData(pertanyaan.Jawabans[i].Info, i);

            newJawaban.Rect.anchoredPosition = new Vector2(0, offset);

            offset -= (newJawaban.Rect.sizeDelta.y + parameters.Margins);
            uIElements.JawabanContentArea.sizeDelta = new Vector2(uIElements.JawabanContentArea.sizeDelta.x, offset * -1);

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
