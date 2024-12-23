using UnityEngine;
using UnityEngine.UI;

public class CanvasSettings : MonoBehaviour
{
    //slider untuk pengaturan volume SFX dan BGM
    public Slider SliderSfx, SliderBGM;

    private void OnEnable() 
    {
        //ketika GameObject diaktifkan, nilai slider disinkronkan dengan volume audio saat ini
        SliderSfx.value = Sound.instance.source_sfx.volume; //mengatur slider SFX sesuai volume SFX
        SliderBGM.value = Sound.instance.source_bgm.volume; //mengatur slider BGM sesuai volume BGM
    }

    public void ubahVolume(bool SFX)
    {
        //fungsi untuk mengubah volume berdasarkan nilai slider
        if (SFX)
        {
            //jika parameter SFX bernilai true, ubah volume SFX
            Sound.instance.source_sfx.volume = SliderSfx.value;
        }
        else
        {
            //jika parameter SFX bernilai false, ubah volume BGM
            Sound.instance.source_bgm.volume = SliderBGM.value;
        }
    }
}
