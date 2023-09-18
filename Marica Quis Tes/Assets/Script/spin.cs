using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyUI.PickerWheelUI;
using TMPro;

public class spin : MonoBehaviour
{
    [SerializeField] private Image labelbuku;
    [SerializeField] private Button uiSpinButton;
    [SerializeField] private Button play;
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
                Debug.Log("Spin End: " + wheelPiece.Label + ", Amount ");
                popUp.SetActive(true);
                labelGet.text = (wheelPiece.Label);
                labelbuku.sprite = (wheelPiece.Icon);
                
                uiSpinButton.interactable = true;
                uiSpinText.text = "Spin Again!";

                // Aktifkan tombol pada popup dan tambahkan listener untuk tombol
                
                play = popUp.GetComponentInChildren<Button>();
                play.onClick.AddListener(() =>
                {
                    // Pindah ke scene yang sesuai dengan WheelPiece
                    string sceneName = wheelPiece.SceneName;
                    if (!string.IsNullOrEmpty(sceneName))
                    {
                        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
                    }
                });
            });

            pickerWheel.Spin();
        });
    }

    // Update is called once per frame
    void pindah()
    {
        
    }
}
