using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SliderSettings : MonoBehaviour
{
    [SerializeField] Slider master;
    [SerializeField] Slider sfx;
    [SerializeField] Slider music;
    [SerializeField] Slider sensitivity;
    [SerializeField] PlayerSoundsManager soundManager;
    [SerializeField] cameraControl cam;
    [SerializeField] SoundManager musicSM;

    //shootingSounds;
    //reloadSounds;
    //cockingSounds;
    //footStepSounds;
    //extraSounds;
    void Awake()
    {
        //soundManager = GetComponent<PlayerSoundsManager>();
        //cam = GetComponent<cameraControl>();
        //musicSM = GetComponent<SoundManager>();
        if(!PlayerPrefs.HasKey("Master") || !PlayerPrefs.HasKey("SFX") || !PlayerPrefs.HasKey("Music") || !PlayerPrefs.HasKey("Sensitivity"))
        {
            PlayerPrefs.SetFloat("Master", 1);
            PlayerPrefs.SetFloat("SFX", 1);
            PlayerPrefs.SetFloat("Music", musicSM.source.volume);
            PlayerPrefs.SetFloat("Sensitivity", .5f);
            LoadSoundValues();
        }
        else
        {
            LoadSoundValues();
        }
    }

    public void MasterVolume()
    {
        AudioListener.volume = master.value;
        SaveSoundValues();
    }

    public void ChangeSFX()
    {
        soundManager.shootingSounds.volume = sfx.value;
        soundManager.reloadSounds.volume = sfx.value;
        soundManager.cockingSounds.volume = sfx.value;
        soundManager.footStepSounds.volume = sfx.value;
        soundManager.extraSounds.volume = sfx.value;
        SaveSoundValues();
    }

    public void ChangeMusic()
    {
        musicSM.source.volume = music.value;
        SaveSoundValues();
    }

    public void ChangeSens()
    {
        cam.sensitivity = 1f;
        cam.sensitivity = sensitivity.value * 1000f;
        SaveSoundValues();
    }

    private void SaveSoundValues()
    {
        PlayerPrefs.SetFloat("Master", master.value);
        PlayerPrefs.SetFloat("SFX", sfx.value);
        PlayerPrefs.SetFloat("Music", music.value);
        PlayerPrefs.SetFloat("Sensitivity", sensitivity.value);
    }

    private void LoadSoundValues()
    {
        master.value = PlayerPrefs.GetFloat("Master");
        sfx.value = PlayerPrefs.GetFloat("SFX");
        music.value = PlayerPrefs.GetFloat("Music");
        sensitivity.value = PlayerPrefs.GetFloat("Sensitivity");
    }
}
