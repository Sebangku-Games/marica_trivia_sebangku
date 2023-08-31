using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DataJawaban = DataJawaban2;

public class DataJawaban2 : MonoBehaviour
{
    #region Variables

    [Header("UI Elements")]
    [SerializeField] TextMeshProUGUI infoTextObject = null;
    [SerializeField] Image toggle = null;

    [Header("Textures")]
    [SerializeField] Sprite uncheckedToggle = null;
    [SerializeField] Sprite checkedToggle = null;

    [Header("References")]
    [SerializeField] GameEvent2 events = null; // Update to use GameEvent2


    private RectTransform _rect = null;
    public RectTransform Rect
    {
        get
        {
            if (_rect == null)
            {
                _rect = GetComponent<RectTransform>() ?? gameObject.AddComponent<RectTransform>();
            }
            return _rect;
        }
    }

    private int _answerIndex = -1;
    public int AnswerIndex { get { return _answerIndex; } }

    private bool Checked = false;
    internal bool IsCorrect;

    #endregion


    /// <summary>
    /// Function that is called to update the answer data.
    /// </summary>
    public void UpdateData2(string info, int index)
    {
        infoTextObject.text = info;
        _answerIndex = index;
    }

    public void Reset()
    {
        Checked = false;
        UpdateUI();
    }

    
    public void SwitchState()
    {
        Checked = !Checked;
        UpdateUI();

        if (events.UpdateQuestionAnswer != null)
        {
            events.UpdateQuestionAnswer(this as DataJawaban2);
        }
    }
    
    void UpdateUI()
    {
        if (toggle == null) return;

        toggle.sprite = (Checked) ? checkedToggle : uncheckedToggle;
    }

   
}
