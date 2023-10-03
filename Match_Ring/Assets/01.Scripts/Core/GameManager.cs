using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
     private PoolingListSO _poolListSO;
     
    private void Awake()
    {
        CreatePoolManager();
    }

    private void CreatePoolManager()
    {
        PoolManager.Instance = new PoolManager(transform);
        foreach (PoolingPair pair in _poolListSO.Pairs)
        {
            PoolManager.Instance.CreatePool(pair.Prefab, pair.Count);
        }
    }
}