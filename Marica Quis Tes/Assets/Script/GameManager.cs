using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    Pertanyaan[] _pertanyaans = null;
    public Pertanyaan[] Pertanyaans { get { return _pertanyaans;} }


    [SerializeField] GameEvent events = null;

    private List<DataJawaban> JawabanPicked = new List<DataJawaban>();
    private List<int> JawabansSelesai = new List<int>();
    private int currentQuestion = 0;

    private IEnumerator IE_WaitTillNextRound = null;

    void Start()
    {
        LoadQuestiion();
        var seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        UnityEngine.Random.InitState(seed);
        foreach (var pertanyaans in Pertanyaans)
        {
            Debug.Log(pertanyaans.info);
        }
        Display();
    }

    public void UpdateJawaban(DataJawaban newJawaban)
    {
        if (Pertanyaans[currentQuestion].GetTypeJawaban == Pertanyaan.TypeJawaban.Single)
        {
            foreach (var jawaban in JawabanPicked)
            {
                if(jawaban != newJawaban)
                {
                    jawaban.Reset();
                }
                JawabanPicked.Clear();
                JawabanPicked.Add(newJawaban);
            }
           
        }
        else
        {
            bool alreadyPicked = JawabanPicked.Exists(x => x == newJawaban);
            if (alreadyPicked)
            {
                JawabanPicked.Remove(newJawaban);
            }
            else
            {
                JawabanPicked.Add(newJawaban);
            }
        }
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
        var pertanyaan = GetRandomPertanyaan();
        if (events.UpdatePertanyaanUI != null)
        {
            events.UpdatePertanyaanUI(pertanyaan);
        }
        else { Debug.LogWarning("Ups! Something went wrong while trying to display new Question UI Data. GameEvents.UpdateQuestionUI is null. Issue occured in GameManager.Display() method."); }

        

    }

    public void Accept()
    {
        bool isCorrect = CheckJawaban();
        JawabansSelesai.Add(currentQuestion);

        UpdateScore((isCorrect) ? Pertanyaans[currentQuestion].AddScore : -Pertanyaans[currentQuestion].AddScore);

        if(IE_WaitTillNextRound != null)
        {
            StopCoroutine(IE_WaitTillNextRound);
        }
        IE_WaitTillNextRound = WaitTillNextRound();
        StartCoroutine(IE_WaitTillNextRound);
    }

    IEnumerator WaitTillNextRound()
    {
        yield return new WaitForSeconds(GameUtility.ResolutionDelayTime);
        Display();
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

    bool CheckJawaban()
    {
        if (!CompareJawaban())
        {
            return false;
        }
        return true;
    }

    bool CompareJawaban()
    {
        if(JawabanPicked.Count > 0)
        {
            List<int> c = Pertanyaans[currentQuestion].GetCorrectJawaban();
            List<int> p = JawabanPicked.Select(x => x.AnswerIndex).ToList();

            var f = c.Except(p).ToList();
            var s = p.Except(c).ToList();

            return !f.Any() && !s.Any();
        }
        return false;
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

    public void UpdateScore(int add)
    {
        events.CurrentFinalScore += add;

        if(events.ScoreUpdated != null)
        {
            events.ScoreUpdated();
        }
    }
}
