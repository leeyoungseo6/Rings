using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField]
    private PoolingListSO _poolListSO;

    public UnityEvent OnGameOver; 
    public float Difficulty = 0;
     
    private void Awake()
    {
        Instance ??= this;
        
        Application.targetFrameRate = 60;
        CreatePoolManager();
        CreateDataManager();
        CreateUIManager();
    }

    private void CreateDataManager()
    {
        DataManager.Instance = new DataManager();
    }

    private void CreateUIManager()
    {
        Transform canvasTrm = GameObject.Find("Canvas").transform;
        UIManager.Instance = new UIManager(canvasTrm);
    }

    private void CreatePoolManager()
    {
        PoolManager.Instance = new PoolManager(transform);
        foreach (PoolingPair pair in _poolListSO.Pairs)
        {
            PoolManager.Instance.CreatePool(pair.Prefab, pair.Count);
        }
    }

    private void Update()
    {
        Difficulty += Time.deltaTime;
    }

    public void GameOver()
    {
        OnGameOver?.Invoke();
        DataManager.Instance.SaveGameData();
    }
}