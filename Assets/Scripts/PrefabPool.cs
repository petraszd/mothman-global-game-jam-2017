using UnityEngine;

public abstract class PrefabPool : MonoBehaviour
{
    public GameObject PoolPrefab;
    public int PoolSize = 10;

    private GameObject[] pool;

    protected virtual void Awake()
    {
        pool = new GameObject[PoolSize];
        for (var i = 0; i < PoolSize; ++i)
        {
            var obj = Instantiate(PoolPrefab, transform.position, Quaternion.identity);
            obj.SetActive(false);
            pool[i] = obj;
        }
    }

    public GameObject GetFromPool()
    {
        var obj = GetFirstInactive();

        obj.SetActive(true);
        obj.transform.parent = null;

        return obj;
    }

    private GameObject GetFirstInactive()
    {
        var obj = pool[0];
        for (var i = 0; i < pool.Length; ++i)
        {
            obj = pool[i];
            if (!obj.activeSelf)
            {
                break;
            }
        }
        // TODO: Warning if everyone is active
        return obj;
    }
}
