using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject GamePausePanal;
    public GameObject GameWinPanal;
    public GameObject GameLossPanal;
    public GameObject GamePlayPanal;

    float Minute;
    float Sec;
    public float WaitTime = 1;
    public TextMeshProUGUI TimeText;
    public TextMeshProUGUI LevelNo;

    public ParticleSystem mConfetti;
    //public TextMeshProUGUI Score;
    //float CountDown;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {

        LevelData localLevel;
        //var localLevel = Instantiate(StorageManager.instance.m_levelData[StorageManager.instance.CurrentLevel]);
             if (StorageManager.instance.CurrentLevel >= StorageManager.instance.m_levelData.Count)
             {
                 localLevel = StorageManager.instance.m_levelData[Random.Range(0, StorageManager.instance.m_levelData.Count)];
             }
             else
             {
                 localLevel = StorageManager.instance.m_levelData[StorageManager.instance.CurrentLevel];
             }
            if (StorageManager.instance.UseTime > 0)
            {
                localLevel.LevelTime -= StorageManager.instance.UseTime;
            }
            //instance = this;.
            //Score.text = StorageManager.instance.TotalScore.ToString();
            LevelNo.text = "Level " + (StorageManager.instance.m_levelData[StorageManager.instance.CurrentLevel].LevelNo).ToString();
            if (localLevel.LevelTime > 60 && localLevel.LevelTime < 3600)
            {
                Minute = (int)(localLevel.LevelTime / 60);
                float TempSec = Minute * 60;
                Sec = localLevel.LevelTime - TempSec;
            }
            else
            {
                Sec = localLevel.LevelTime;
                Minute = 0;
            }
            StartCoroutine(DesplayTime());
    }
    IEnumerator DesplayTime()
    {
        while (true)
        {
            if (Sec == 0)
            {
                Minute--;
                Sec = 60;
            }

            Sec--;
            StorageManager.instance.UseTime++;
            TimeText.text = string.Format("{0:00}:{1:00}", Minute, Sec);
            if (Minute == 0 && Sec == 0 && GameManager.Instance.m_liveObject.Count > 0)
            {
                
                TimeText.text = string.Format("{0:00}:{1:00}", 0, 0);
                GameLossPanal.SetActive(true);
                StorageManager.instance.CollectingPair = 0;
                StorageManager.instance.UseTime = 0;
                StorageManager.instance.levelStar = 0;
                Time.timeScale = 0;
            }
            if (Minute == 0 && Sec == 0 && GameManager.Instance.m_liveObject.Count <= 0)
            {
                GameWinPanal.SetActive(true);
                mConfetti.Play();
                StorageManager.instance.CollectingPair = 0;
                StorageManager.instance.UseTime = 0;
                StorageManager.instance.levelStar = 0;
                StorageManager.instance.CurrentLevel++;
                Debug.Log("Win");
                break;
            }
            yield return new WaitForSeconds(WaitTime);
        }
    }
    public void GamePause()
    {
        GamePausePanal.SetActive(true);
        Time.timeScale = 0;
    }
    public void Home()
    {
        //Load Main Menu
        Time.timeScale = 1;
        //GamePausePanal.SetActive(false);
        //GameWinPanal.SetActive(false);
        //GameLossPanal.SetActive(false);
        SceneManager.LoadScene("Home");
        //Application.Quit();
    }

    public void Replay()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Play()
    {
        Time.timeScale = 1;
        GamePausePanal.SetActive(false);
    }

    public void CloseGamePausePanal()
    {
        Time.timeScale = 1;
        GameLossPanal.SetActive(false);
        GameWinPanal.SetActive(false);
        GamePausePanal.SetActive(false);
    }

    public void CloseWinAndLossPanal()
    {
        //Load Main Menu
        Time.timeScale = 0;
    }

    public void NextLevel()
    {
        Time.timeScale = 1;
        GameWinPanal.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //LevelNo.text = "Level " + (GameManager.Instance.m_levelData[StorageManager.instance.CurrentLevel].LevelNo+1).ToString();
    }
}
