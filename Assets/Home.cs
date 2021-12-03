using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class Home : MonoBehaviour
{
    public float levelNo;
    public GameObject SettingPanal;
    public GameObject StartButton;
    public Button SoundButton;
    public Button MusicButton;
    public Sprite SoundOn;
    public Sprite SoundOff; 
    public Sprite MusicOn;
    public Sprite MusicOff;
    public TextMeshProUGUI LevelNoText;
    public Image LevelBar;
    
    public float RemainPair;
    private void Start()
    {
        levelNo = (StorageManager.instance.CurrentLevel)+1;

        RemainPair = StorageManager.instance.CollectingPair;

        LevelNoText.text = levelNo.ToString();
        if(StorageManager.instance.CurrentLevel >= StorageManager.instance.m_levelData.Count)
        {
            RemainPair /= StorageManager.instance.m_levelData[Random.Range(0, StorageManager.instance.m_levelData.Count)].TotalPairs;
        }
        else
        {
            RemainPair /= StorageManager.instance.m_levelData[StorageManager.instance.CurrentLevel].TotalPairs;
        }
        LevelBar.fillAmount = RemainPair;
        //Debug.Log(RemainPair);
    }
    bool Sound = false;
    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }
    public void setting()
    {
        SettingPanal.SetActive(true);
        StartButton.SetActive(false);
    }
    public void Volume()
    {
        Sound = !Sound;
        if (Sound)
        {
            SoundButton.GetComponent<Image>().sprite = SoundOff;
        }
        else
        {
            SoundButton.GetComponent<Image>().sprite = SoundOn;
        }
    }
    private bool IsMusic;
    public void MusicOnOff()
    {
        IsMusic = !IsMusic;
        if (IsMusic)
        {
            MusicButton.GetComponent<Image>().sprite = MusicOff;
        }
        else
        {
            MusicButton.GetComponent<Image>().sprite = MusicOn;
        }
    }
    public void CloseSettingPanal()
    {
        SettingPanal.SetActive(false);
        StartButton.SetActive(true);
    }
}
