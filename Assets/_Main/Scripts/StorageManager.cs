using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageManager : MonoBehaviour
{
    public static StorageManager instance;

    public List<LevelData> m_levelData = new List<LevelData>();

    [ShowInInspector, ReadOnly]
    public int CurrentLevel { get { return  PlayerPrefs.GetInt(nameof(CurrentLevel)); } set { PlayerPrefs.SetInt(nameof(CurrentLevel), value); } }
    [ShowInInspector, ReadOnly]
    public int TotalScore { get { return  PlayerPrefs.GetInt(nameof(TotalScore)); } set { PlayerPrefs.SetInt(nameof(TotalScore), value); } }
    [ShowInInspector,ReadOnly]
    public int CollectingPair { get { return PlayerPrefs.GetInt(nameof(CollectingPair)); } set { PlayerPrefs.SetInt(nameof(CollectingPair), value); } }
    [ShowInInspector, ReadOnly]
    public int UseTime { get { return PlayerPrefs.GetInt(nameof(UseTime)); } set { PlayerPrefs.SetInt(nameof(UseTime), value); } }
    [ShowInInspector, ReadOnly]
    public int levelStar { get { return PlayerPrefs.GetInt(nameof(levelStar)); } set { PlayerPrefs.SetInt(nameof(levelStar), value); } }

    // Start is called before the first frame update
    private void Awake()
    {
        //PlayerPrefs.DeleteAll();
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);
        DontDestroyOnLoad(this);
    }
}
[System.Serializable]
public class LevelData
{
    public int LevelNo;
    public int LevelTime;
    [Range(0.1f, 0.9f)]
    [SerializeField] private float UniquePairRatio;
    [OnValueChanged("Check"), SerializeField]
    public int TotalPairs;
    [ShowInInspector, ReadOnly] public int UniqueCount => (int)(TotalPairs * UniquePairRatio);
    [ShowInInspector, ReadOnly] public int ClonePairs => TotalPairs - UniqueCount;
    void Check()
    {
        if (TotalPairs % 2 != 0) TotalPairs++;
    }
}
