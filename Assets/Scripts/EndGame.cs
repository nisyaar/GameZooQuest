using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    //text yang menampilkan skor akhir dan skor tertinggi
    public Text TeksScore, TeksTotalScore;

    public void Start()
    {
        //mengecek apakah skor saat ini lebih besar atau sama dengan skor tertinggi yang tersimpan
        if(Data.DataScore >= PlayerPrefs.GetInt("score"))
        {
            //jika benar, perbarui skor tertinggi yang disimpan di PlayerPrefs
            PlayerPrefs.SetInt("score", Data.DataScore);
        }

        //menampilkan skor akhir dari sesi permainan
        TeksScore.text = Data.DataScore.ToString();
        
        //menampilkan skor tertinggi yang tersimpan di PlayerPrefs
        TeksTotalScore.text = PlayerPrefs.GetInt("score").ToString();
    }
}
