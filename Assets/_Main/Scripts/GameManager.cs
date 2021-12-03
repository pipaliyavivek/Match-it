using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public static GameManager Instance;
    [Space (20)]
    public bool isGameOn = false;
    public LayerMask mask;
    public Item inHand;
    public List<GameObject> Objs;
    public List<GameObject> shuffledList;
    //public int PairSpawnAtOneTime;
    //int RemainObject;
    //public bool isAllObjectSpawn;
    Ray ray;
    RaycastHit hit;

    //public List<LevelData> m_levelData = new List<LevelData>();
    //public int NumberOfItem;

    [SerializeField] private List<GameObject> m_LevelObejctPrefebs = new List<GameObject>();

    [SerializeField] internal List<GameObject> m_liveObject = new List<GameObject>();
    [SerializeField] private int LiveObjectAtATime;
    // list 

    //public float LevelTime;
    //public string m_DisplayLevelTime;

    [Space(50)]
    public Collider SpawnArea;
    [HideInInspector]
    public SpawnPool myPool;
    public TextMeshProUGUI stars_txt;
    public int levelStar;
    //int _CurentScore = 0;
    //public int CurentScore
    //{
    //    get
    //    {
    //        stars_txt.text = StorageManager.instance.TotalScore.ToString();
    //        return StorageManager.instance.TotalScore;
    //    }
    //    set
    //    {
    //        StorageManager.instance.TotalScore = value;
    //        stars_txt.text = StorageManager.instance.TotalScore.ToString();
    //    }
    //}
    private void Awake()
    {
        // PlayerPrefs.DeleteAll();
        Objs = new List<GameObject>();
        Objs.AddRange(Resources.LoadAll<GameObject>("Objects").ToList());

        shuffledList = new List<GameObject>();
        shuffledList = Objs.OrderBy(x => Random.value).ToList();

        Instance = this;
        if (!myPool) myPool = GetComponent<SpawnPool>();
    }
    private void Start()
    {
        StartCoroutine(StartLevel());
    }
    private void Update()
    {
        //if (isGameOn)
        //{
        if (Input.GetMouseButtonDown(0))
        {
            if (inHand == null)
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, mask))
                {
                    try
                    {
                        inHand = hit.collider.GetComponent<Item>();
                        inHand.Pick();
                    }
                    catch (System.Exception)
                    {

                    }                        
                }
            }                
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (inHand)
            {
                inHand.Release();
                inHand = null;
            }       
        }
        //}
        // TotalCollectingPair >= m_levelData[StorageManager.instance.CurrentLevel].TotalPairs
        if (m_liveObject.Count == 0 && isGameOn)
        {
            StartCoroutine(LevelWin());
            isGameOn = false;
        }
    }
    IEnumerator StartLevel()
    {
        //if (StorageManager.instance.CollectingPair > 0)
        //{
        //    m_levelData[StorageManager.instance.CurrentLevel].TotalPairs -= StorageManager.instance.CollectingPair;
        //    StorageManager.instance.CollectingPair = 0;
        //}
        stars_txt.text = StorageManager.instance.levelStar.ToString();
        //var localLevel = Instantiate(StorageManager.instance.m_levelData[StorageManager.instance.CurrentLevel]);
        var localLevel = StorageManager.instance.m_levelData[StorageManager.instance.CurrentLevel];
        Debug.Log(localLevel);
        if (StorageManager.instance.CollectingPair > 0)
        {
            localLevel.TotalPairs -= StorageManager.instance.CollectingPair;
        }
        //LevelData localLevel = m_levelData[levelIndex];
        //CurentScore = 0;
        myPool.DespawnAll();
        int spawned = 0;
        
        //all diffrent 
        m_LevelObejctPrefebs.AddRange(shuffledList.GetRange(0, (localLevel.UniqueCount)));
        //add same 
        GameObject localNewobj = shuffledList[Random.Range(m_LevelObejctPrefebs.Count, shuffledList.Count)];
        for (int i = 0; i < localLevel.ClonePairs; i++)
        {
            m_LevelObejctPrefebs.Add(localNewobj);
        }
        m_LevelObejctPrefebs.AddRange(m_LevelObejctPrefebs);
        m_LevelObejctPrefebs = m_LevelObejctPrefebs.OrderBy(x => Random.value).ToList();
        //if (StorageManager.instance.CollectingPair > 0)
        //{
        //    while (spawned < m_LevelObejctPrefebs.Count)
        //    {
        //        yield return new WaitUntil(() => m_liveObject.Count < LiveObjectAtATime);
        //        var r_point = RandomPointInBounds(SpawnArea.bounds);
        //        myPool.Spawn(m_LevelObejctPrefebs[spawned], r_point, Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f)));
        //        m_liveObject.Add(m_LevelObejctPrefebs[spawned].gameObject);
        //        spawned++;
        //        yield return new WaitForSeconds(0.01f);
        //    }
        //}
        //else
        //{
            while (spawned < m_LevelObejctPrefebs.Count)
            {
                yield return new WaitUntil(() => m_liveObject.Count < LiveObjectAtATime);
                var r_point = RandomPointInBounds(SpawnArea.bounds);
                myPool.Spawn(m_LevelObejctPrefebs[spawned], r_point, Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f)));
                m_liveObject.Add(m_LevelObejctPrefebs[spawned].gameObject);
                spawned++;
                yield return new WaitForSeconds(0.01f);
            }
        //}
        isGameOn = true;
    }
    IEnumerator LevelWin()
    {
        UIManager.instance.WaitTime = 0.05f;
        yield return new WaitForSeconds(0.1f);
    }
    public static Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }

}
