using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*[CreateAssetMenu(fileName = "LevelData", menuName = "MakeLevel/Level")]
public class LevelData : ScriptableObject
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
}*/
