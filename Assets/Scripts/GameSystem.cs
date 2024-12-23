using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Events;

public class Data
{
    public static int DataLevel, DataScore, DataTimer, DataEnergy, mulai; //data game
    public static bool GameSelesai, SistemRandom; //status game
}

public class GameSystem : MonoBehaviour
{
    public static GameSystem instance;
    public bool NewGame = false; //status apakah game baru dimulai


    int MaxLevel = 15; //maks level

    [Header("Data Game")]
    public bool GameAktif; //status game aktif
    //public bool GameSelesai;
    //public bool SistemRandom;
    public int Target, DataTersedia; //target objek yang harus diselesaikan
    public int DataLevel, DataScore, DataTimer, DataEnergy; //datagame

    [Header("Komponen Game")]
    public Text TextLevel;
    public Text TextTimer;
    public Text TextScore;
    public RectTransform EnergyLife;

    [Header("Object Game")]
    [SerializeField] public GameObject Gui_Pause;
    public GameObject Gui_Transisi;


    [System.Serializable]
    public class DataGame
    {
        public string Nama; //nama soal
        public Sprite Gambar; //gambar soal
    }

    [Header("Default Setting")]
    public DataGame[] DataPermainan; //data soal
    [Space]
    public ObjTempatDrop[] DropTempat; //tempat drop
    public ObjDrag[] DragObj; //obj yang bisa digeser

    private void Awake() //nentuin status game saat diaktifkan
    {
        
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "PageSelesai" && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "MainMenu")
        {
            Data.SistemRandom = true; //aktifin sistem acak jika buka di main menu atau page selesai
        }
        else
        {
            Data.SistemRandom = false; //nonaktifin sistem acak
        }
        
        instance = this; //menyimpan instance

        if (Data.GameSelesai && Data.SistemRandom == false) //jika game selsai, langsung keluar dari fungsi sitem acak
        return;
        
    }

    
    void Start()
    {

        Debug.Log("Sistem Random = " + Data.SistemRandom);

        
        if (Data.GameSelesai && Data.SistemRandom == false)
        return;

        //reset data jika game akan di play
        if(NewGame == false && Data.mulai == 0){
            Data.mulai+=1;
            Data.DataTimer = 60 * 15; //timer 15 menit
            Data.DataScore = 0; //skor awal
            Data.DataEnergy = 5; //energi awal
            Data.DataLevel = 0; //level awal
        }
        
        if (Gui_Transisi.GetComponent<GameControl>() != null)
        {
            Debug.Log("Komponen GameControl ditemukan di awal permainan.");
        }
        else
        {
            Debug.Log("Komponen GameControl TIDAK ditemukan di awal permainan!");
        }
        
        Data.GameSelesai=false; //menandai game belum selesai
        
        ResetData(); //atur game ulang
        Target = DropTempat.Length; //hitung jumlah target
        if (Data.SistemRandom)
            AcakSoal();  //acak soal
        AcakSoal();
        DataTersedia = 0; //reset jumlah soal selesai
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "MainMenu")
        {
            GameAktif = true; //game aktif
        }
    }

    public void ResetData()
    {
        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Page0")
        {
            Debug.Log("permainan ter reset");
           // NewGame = false;
            Data.SistemRandom = true;
            Data.DataTimer = 60 * 15;
            Data.DataScore = 0;
            Data.DataEnergy = 5;
            Data.DataLevel = 0;
            Debug.Log("Initial DataEnergy: " + Data.DataEnergy);
        }
    }

    float s;
    void Update()
    {   

        // if (Data.SistemRandom)  // Check if it's true
        // {
        //     Debug.Log("SistemRandom is true");
        // }
        // else
        // {
        //     Debug.Log("SistemRandom is false");
        // }

        if (Data.GameSelesai)
        return;

        //Debug.Log(Data.GameSelesai);
        //Debug.Log(NewGame);
        //Debug.Log(Data.mulai);
        //Debug.Log("Initial DataEnergy: " + Data.DataEnergy);
        if(Input.GetKeyDown(KeyCode.Space))
            AcakSoal();

        if(GameAktif && !Data.GameSelesai)
        {
            if(Data.DataTimer > 0)
            {
                s += Time.deltaTime; //nambah waktu
                if( s >= 1)
                {
                    Data.DataTimer--; //kurangi waktu tiap detik
                    s = 0;
                }
            }
            if(Data.DataTimer <= 0){
                GameAktif = false; //jika waktu habis game berakhir
                Data.GameSelesai = true;
                

                //game kalah
                Sound.instance.PanggilSfx(4);

                Gui_Transisi.GetComponent<GameControl>().btn_pindah("PageSelesai");

            }

            if(Data.DataEnergy <= 0) //jika eergi habis, game selesai
            { 
                Data.GameSelesai = true;
                Data.SistemRandom = false;

                //fungsi kalah
                Sound.instance.PanggilSfx(4);

                Gui_Transisi.GetComponent<GameControl>().btn_pindah("PageSelesai");

                
            }

            if(DataTersedia >= Target) //jika target selsai
            {
                Data.GameSelesai = false;
                GameAktif = false;

                //game menang
                if(Data.DataLevel < (MaxLevel -1))
                {
                    Data.DataLevel++; //naik ke level selenjatnya
                    //pindah ke next level
                    UnityEngine.SceneManagement.SceneManager.LoadScene("Page"+Data.DataLevel);
                   // Gui_Transisi.GetComponent<GameControl>().btn_pindah("Game"+Data.DataLevel);
                    Sound.instance.PanggilSfx(3);

                }
                else //jika game selesai
                {
                    Data.GameSelesai = true;
                    Data.SistemRandom = false;
                    
                    //game selesai pindah ke menu selesai
                    Gui_Transisi.GetComponent<GameControl>().btn_pindah("PageSelesai");
                    Sound.instance.PanggilSfx(5);
                    Sound.instance.PanggilSfx(6);
                }
            }
        }

        KomponenGame();
    }

    public List<int> _AcakSoal = new List<int>(); //list soal acak
    public List<int> _AcakPos = new List<int>(); //list posisi acak
    int random;
    int random2;

    public void AcakSoal()
    {
        
            // Inisialisasi list dengan ukuran yang benar
            _AcakSoal = new List<int>(new int[DragObj.Length]);
            _AcakPos = new List<int>(new int[DropTempat.Length]);

            // Mengacak _AcakSoal
            for(int i = 0; i < _AcakSoal.Count; i++)
            {
                random = Random.Range(1, DataPermainan.Length);
                while (_AcakSoal.Contains(random))
                {
                    random = Random.Range(1, DataPermainan.Length);
                }
                _AcakSoal[i] = random;

                DragObj[i].ID = random - 1;
                DragObj[i].Teks.text = DataPermainan[random - 1].Nama;

            }

            // Mengacak _AcakPos
            for(int i = 0; i < _AcakPos.Count; i++)
            {
                random2 = Random.Range(1, _AcakSoal.Count + 1);
                while (_AcakPos.Contains(random2))
                {
                    random2 = Random.Range(1, _AcakSoal.Count + 1);
                }
                _AcakPos[i] = random2;

                DropTempat[i].Drop.ID = _AcakSoal[random2 - 1] - 1;
                DropTempat[i].Gambar.sprite = DataPermainan[DropTempat[i].Drop.ID].Gambar;

            }
        
    }

    public void KomponenGame() //ui game
    {
        TextLevel.text = (Data.DataLevel + 1).ToString(); //nampilin level

        int Menit = Mathf.FloorToInt(Data.DataTimer / 60);
        int Detik = Mathf.FloorToInt(Data.DataTimer % 60);
        TextTimer.text = Menit.ToString("00")+ ":" + Detik.ToString("00"); //nampilin timer

        TextScore.text = Data.DataScore.ToString(); //nampilin score

        EnergyLife.sizeDelta = new Vector2(49f * Data.DataEnergy, 44f); //nampilin energi
    }

    public void btn_pause(bool pause) //jika pause game
    {
        if(pause){
            GameAktif = false; //pause
            Gui_Pause.SetActive(true); //tampilin menu pause
        }

        else{
            GameAktif = true; //lanjut game
            Gui_Pause.SetActive(false); //sembunyikan menu pause
        }
    }
}