using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Volume : MonoBehaviour
{

    public Toggle musicToggle;

    public UnityAction<bool> ToggleMusic { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        // Tambahkan listener untuk tombol toggle
        musicToggle.onValueChanged.AddListener(ToggleMusic);
    }
}
