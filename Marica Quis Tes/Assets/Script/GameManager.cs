using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Pertanyaan[] _pertanyaans = null;
    public Pertanyaan[] Pertanyaans { get { return _pertanyaans;} }


    [SerializeField] GameEvent events = null;

    private List<DataJawaban> JawabanPicked = new List<DataJawaban>();
    private List<int> JawabansSelesai = new List<int>();
    private int currentQuestion = 0;

    void Start()
    {
        LoadQuestiion();

        foreach (var pertanyaans in Pertanyaans)
        {
            Debug.Log(pertanyaans.info);
        }
        Display();
    }

    public void HapusJawaban()
    {
        JawabanPicked = new List<DataJawaban>();
        var pertanyaan = GetRandomPertanyaan();

        if(events.UpdatePertanyaanUI != null)
        {
            events.UpdatePertanyaanUI(pertanyaan);
        }
        else
        {
            Debug.LogWarning("Ups! Something went wrong while trying to display new Question UI Data. GameEvents.UpdateQuestionUI is null. Issue occured in GameManager.Display() method.");
        }
    }

    void Display()
    {
        HapusJawaban();

    }

    Pertanyaan GetRandomPertanyaan()
    {
        var randomIndex = GetRandomPertanyaanIndex();
        currentQuestion = randomIndex;

        return Pertanyaans[currentQuestion];

    }
    int GetRandomPertanyaanIndex()
    {
        var random = 0;
        if (JawabansSelesai.Count < Pertanyaans.Length)
        {
            do
            {
                random = UnityEngine.Random.Range(0, Pertanyaans.Length);
            } while (JawabansSelesai.Contains(random) || random == currentQuestion);
        }
        return random;
    }

    void LoadQuestiion()
    {
        Object[] objs = Resources.LoadAll("Pertanyaans", typeof (Pertanyaan));
        _pertanyaans = new Pertanyaan[objs.Length];
        for (int i = 0; i < objs.Length; i++)
        {
            _pertanyaans[i] = (Pertanyaan)objs[i];
        }
    }
}
