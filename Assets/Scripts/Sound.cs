using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    //untuk mengakses script Sound di seluruh game
    public static Sound instance;

    //array untuk menyimpan kumpulan AudioClip
    public AudioClip[] Clip;

    //audioSource untuk memutar efek suara (SFX)
    public AudioSource source_sfx;

    //audioSource untuk memutar musik latar belakang (BGM)
    public AudioSource source_bgm;

    private void Awake()
    {
        //mengecek apakah instance Sound sudah ada
        if (instance == null)
        {
            instance = this; //meenyimpan instance ke variabel statis
            DontDestroyOnLoad(this.gameObject); //membuat objek tidak hancur saat ganti scene
        }
        else
        {
            Destroy(gameObject); //menghapus instance baru jika sudah ada instance lain
        }
    }

    //memutar suara berdasarkan ID
    public void PanggilSfx(int id)
    {
        source_sfx.PlayOneShot(Clip[id]); //suara yang sesuai dengan ID
    }
}
