using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class load_Scene : MonoBehaviour
{
    public void SceneNama()
    {
        SceneManager.LoadScene("nama");
    }

    public void SceneHome()
    {
        SceneManager.LoadScene("home");
    }
}
