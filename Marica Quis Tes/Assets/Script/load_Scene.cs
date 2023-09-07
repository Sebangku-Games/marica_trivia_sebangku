using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class load_Scene : MonoBehaviour
{
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    Animator animator;
    public void SceneNama()
    {
        SceneManager.LoadScene("nama");
    }

    public void SceneHome()
    {
        SceneManager.LoadScene("home");
    }

    public void scale(float scale)
    {
        transform.localScale = new Vector2(1 / scale, 1 * scale);
    }

    [System.Obsolete]
    public void scene(string scene)
    {
        Application.LoadLevel(scene);
    }

    

}
