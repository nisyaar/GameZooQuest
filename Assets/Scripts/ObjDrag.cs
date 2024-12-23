using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;

public class ObjDrag : MonoBehaviour
{
    [HideInInspector] public Vector2 SavePosisi; //posisi awal objek (untuk dikembalikan jika jawaban salah)
    [HideInInspector] public bool IsDiAtasObj; //menandakan apakah objek berada di atas area drop
    private Transform SaveObj; //menyimpan referensi ke objek area drop
    public int ID; //ID objek ini (untuk mencocokkan dengan area drop)
    public Text Teks; //text untuk menampilkan informasi terkait

    public UnityEvent OnDragTrue; //dipanggil saat drag berhasil (jawaban benar)

    void Start()
    {
        //menyimpan posisi awal objek
        SavePosisi = transform.position;
    }

    private void OnMouseDown()
    {
        //suara saat objek di-click
        Sound.instance.PanggilSfx(0);
    }

    private void OnMouseDrag()
    {
        //memindahkan posisi objek sesuai dengan posisi kursor, jika permainan belum selesai
        if (!Data.GameSelesai)
        {
            Vector2 Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = Pos;
        }
    }

    private void OnMouseUp()
    {
        //mengecek apakah objek dilepaskan di atas area drop
        if (IsDiAtasObj)
        {
            //mendapatkan ID dari area drop
            int ID_TempatDrop = SaveObj.GetComponent<TempatDrop>().ID;

            //jika ID objek cocok dengan ID area drop
            if (ID == ID_TempatDrop)
            {
                //menempelkan objek pada area drop
                transform.SetParent(SaveObj);
                transform.localPosition = Vector3.zero; //menempatkan objek di tengah area drop
                transform.localScale = new Vector2(0.8f, 0.8f); //menyesuaikan ukuran objek

                //menonaktifkan tampilan dan interaksi area drop
                SaveObj.GetComponent<SpriteRenderer>().enabled = false;
                SaveObj.GetComponent<Rigidbody2D>().simulated = false;
                SaveObj.GetComponent<BoxCollider2D>().enabled = false;

                //menonaktifkan collider pada objek ini untuk mencegah interaksi lebih lanjut
                gameObject.GetComponent<BoxCollider2D>().enabled = false;

                //memanggil event drag berhasil
                OnDragTrue.Invoke();

                //mengupdate data permainan, ambah skor dan data tersimpan
                GameSystem.instance.DataTersedia++;
                Data.DataScore += 100; //tambah skor
                Sound.instance.PanggilSfx(1); //suara "jawaban benar"
            }
            else
            {
                //jika ID tidak cocok, kembalikan objek ke posisi semula
                transform.position = SavePosisi;

                //mengurangi energi 
                Data.DataEnergy--;
                Sound.instance.PanggilSfx(2); //suara "jawaban salah"
            }
        }
        else
        {
            // jika dilepaskan di luar area drop, kembalikan ke posisi semula
            transform.position = SavePosisi;

        }
    }

    private void OnTriggerStay2D(Collider2D trig)
    {
        //mengecek apakah objek berada di atas area drop 
        if (trig.gameObject.CompareTag("Drop"))
        {
            IsDiAtasObj = true; //menandai bahwa objek berada di atas area drop
            SaveObj = trig.gameObject.transform; // menyimpan ke area drop
        }
    }

    private void OnTriggerExit2D(Collider2D trig)
    {
        //jika objek meninggalkan area drop, reset status
        if (trig.gameObject.CompareTag("Drop"))
        {
            IsDiAtasObj = false;
        }
    }
}
