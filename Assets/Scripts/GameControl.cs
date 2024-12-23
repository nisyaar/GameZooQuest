using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    //var untuk mengontrol transisi dan pengaturan khusus
    public bool IsTransisi, IsTidakPerlu;

    // menyimpan nama scene yang aktif atau akan dituju
    private string SaveNamaScene;

    //sistem game
    private GameSystem gameSystem;

    //animator untuk latar belakang (transisi)
    [SerializeField] private Animator BackgroundController;

    private void Awake()
    {
        //jika kedua kondisi terpenuhi, nonaktifkan GameObject ini
        if (IsTransisi && IsTidakPerlu)
        {
            gameObject.SetActive(false);
        }
    }

    void Start()
    {
        //mulai animasi transisi
        BackgroundController.SetInteger("state", 2);

        //mendapatkan nama scene aktif dan menyimpannya
        Scene activeScene = SceneManager.GetActiveScene();
        SaveNamaScene = activeScene.name;
        Debug.Log("Scene yang sedang aktif: " + SaveNamaScene);

        //mencari komponen GameSystem di scene
        gameSystem = GameObject.Find("GameSystem")?.GetComponent<GameSystem>();

        //debugging untuk memastikan GameSystem ditemukan
        if (gameSystem != null)
        {
            Debug.Log("GameSystem berhasil ditemukan di Start().");
        }
        else
        {
            Debug.Log("GameSystem TIDAK ditemukan di Start()! Pastikan GameObject memiliki nama yang benar.");
        }
    }

    public void btn_sound(int id)
    {
        //efek suara berdasarkan ID yang diberikan
        if (Sound.instance == null)
        {
            Debug.LogError("Sound.instance belum diinisialisasi! Pastikan ada GameObject dengan script Sound di scene.");
            return;
        }

        Sound.instance.PanggilSfx(id);
    }

    public void btn_pindah(string nama)
    {
        //mengaktifkan GameObject dan menyimpan nama scene tujuan
        this.gameObject.SetActive(true);
        SaveNamaScene = nama;

        //debugging untuk memastikan BackgroundController telah di-assign
        if (BackgroundController == null)
        {
            Debug.LogError("BackgroundController belum di-assign!");
            return;
        }

        //memulai animasi transisi dan menunggu animasi selesai
        BackgroundController.SetInteger("state", 1);
        StartCoroutine(WaitForAnimationToFinish());
    }

    public void btn_retry()
    {
        //mengulang kembali scene aktif dengan animasi transisi
        Scene activeScene = SceneManager.GetActiveScene();
        SaveNamaScene = activeScene.name;
        BackgroundController.SetInteger("state", 1);
        StartCoroutine(WaitForAnimationToFinish());
    }

    public void btn_reset()
    {
        //reset data game melalui GameSystem
        if (gameSystem == null)
        {
            gameSystem = GameObject.Find("GameSystem")?.GetComponent<GameSystem>();
        }

        if (gameSystem != null)
        {
            gameSystem.ResetData(); //mengatur ulang nyawa, skor, dan level
            gameSystem.NewGame = true;
            Debug.Log("GameSystem ResetData() berhasil dipanggil.");
        }
        else
        {
            Debug.Log("GameSystem TIDAK ditemukan saat reset! Periksa nama GameObject.");
        }

        //memulai kembali dari scene awal "Page0"
        BackgroundController.SetInteger("state", 1);
        SceneManager.LoadScene("Page0");
    }

    private IEnumerator WaitForAnimationToFinish()
    {
        //menunggu animasi selesai sebelum memuat scene baru
        yield return new WaitForSeconds(0.9f); //sesuaikan durasi dengan animasi sebenarnya
        pindahScene();
    }

    public void pindahScene()
    {
        //memuat scene berdasarkan nama yang disimpan
        if (!string.IsNullOrEmpty(SaveNamaScene))
        {
            SceneManager.LoadScene(SaveNamaScene);
        }
        else
        {
            Debug.LogError("Scene name is empty, cannot load scene.");
        }
    }

    public void btn_quit()
    {
        //keluar dari game
        Application.Quit();
    }
}
