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
    [SerializeField] private GameObject popUp;
    [SerializeField] private TextMeshProUGUI labelGet;

    [SerializeField] private PickerWheel pickerWheel;
    // Start is called before the first frame update
    void Start()
    {
        uiSpinButton.onClick.AddListener(() =>
        {
            uiSpinButton.interactable = false;
            uiSpinText.text = "Spinning!";

            pickerWheel.OnSpinStart(() =>
            {
                Debug.Log("Spin Started");
            });

            pickerWheel.OnSpinEnd(wheelPiece =>
            {
                Debug.Log("Spin End: " + wheelPiece.Label + ", Amount " + wheelPiece.Amount);
                popUp.SetActive(true);
                labelGet.text = (wheelPiece.Label);
                uiSpinButton.interactable = true;
                uiSpinText.text = "Spin Again!";
            });

            pickerWheel.Spin();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
