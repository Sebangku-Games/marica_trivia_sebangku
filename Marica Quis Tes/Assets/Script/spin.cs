using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyUI.PickerWheelUI;
using TMPro;

public class spin : MonoBehaviour
{
    [SerializeField] private Button uiSpinButton;
    [SerializeField] private TextMeshProUGUI uiSpinText;

    [SerializeField] private PickerWheel pickerWheel;
    // Start is called before the first frame update
    void Start()
    {
        uiSpinButton.onClick.AddListener(() =>
        {
            uiSpinText.text = "Spinning!";
            pickerWheel.Spin();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
