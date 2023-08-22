using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Jawaban
{
    [SerializeField] private string _info;
    public string Info { get { return _info; } }

    [SerializeField] private bool _jawabanBenar;
    public bool JawabanBenar { get { return _jawabanBenar; } }
}
[CreateAssetMenu(fileName = "Pertanyaan Baru", menuName = "Quiz/Pertanyaan Baru")]
public class Pertanyaan : ScriptableObject
{
    public enum TypeJawaban { Multi, Single }

    [SerializeField] private string _info = string.Empty;
    public string info { get { return _info; } }

    [SerializeField] Jawaban[] _jawaban = null;
    public Jawaban[] Jawabans { get { return _jawaban; } }

    //parameters

    [SerializeField] private bool _useTimer = false;
    public bool UseTimer { get { return _useTimer; } }

    [SerializeField] private int _timer = 0;
    public int Timer { get { return _timer; } }

    [SerializeField] private TypeJawaban _typeJawaban = TypeJawaban.Multi;
    public TypeJawaban GetTypeJawaban { get { return _typeJawaban; } }

    [SerializeField] private int _addScore = 10;
    public int AddScore { get { return _addScore; } }

    public List<int> GetCorrectJawaban()
    {
        List<int> CorrectJawaban = new List<int>();
        for (int i = 0; i > Jawabans.Length; i++)
        {
            if (Jawabans[i].JawabanBenar)
            {
                CorrectJawaban.Add(i);
            }
        }
        return CorrectJawaban;
    }
}
