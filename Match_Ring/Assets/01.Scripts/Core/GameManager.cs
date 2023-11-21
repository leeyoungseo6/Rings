using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField]
    private PoolingListSO _poolListSO;

    public UnityEvent OnGameOver;
    public float Difficulty { get; set; }
     
    private void Awake()
    {
        Instance ??= this;
        
        Application.targetFrameRate = 60;
        CreateDataManager();
        CreateUIManager();
        CreatePoolManager();
    }

    private void CreateUIManager()
    {
        Transform canvasTrm = GameObject.Find("Canvas_Dynamic").transform;
        GameObject uiManager = new GameObject("UIManager")
        {
            transform =
            {
                parent = transform
            }
        };
        UIManager.Instance = uiManager.AddComponent<UIManager>();
        UIManager.Instance.Init(canvasTrm);
    }

    private void CreateDataManager()
    {
        GameObject dataManager = new GameObject("DataManager")
        {
            transform =
            {
                parent = transform
            }
        };
        DataManager.Instance = dataManager.AddComponent<DataManager>();
        DataManager.Instance.Init();
    }

    private void CreatePoolManager()
    {
        GameObject poolManager = new GameObject("PoolManager")
        {
            transform =
            {
                parent = transform
            }
        };
        PoolManager.Instance = poolManager.AddComponent<PoolManager>();
        foreach (PoolingPair pair in _poolListSO.Pairs)
        {
            PoolManager.Instance.CreatePool(pair.Prefab, pair.Count);
        }
    }

    public void GameOver()
    {
        OnGameOver?.Invoke();
    }

    private void Update()
    {
        if (UIManager.Instance.Score > 0) Difficulty += Time.deltaTime * 1.25f; 
    }
}