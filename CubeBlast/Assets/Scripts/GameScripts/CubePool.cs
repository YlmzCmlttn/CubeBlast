using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePool : MonoBehaviour
{
    private List<GameObject> pool;

    private void Start()
    {
        pool = new List<GameObject>();
    }
    public void PushObject(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.parent = null;
        pool.Add(obj);
    }

    public GameObject PopObject()
    {
        if (pool.Count <= 0)
        {
            return PieceFactory.CreatePiece("rand", new Vector2(0, 0));
        }
        int randomIndex = Random.Range(0, pool.Count);
        GameObject obj = pool[randomIndex];
        pool.RemoveAt(randomIndex);
        obj.SetActive(true);
        obj.transform.localScale = Vector3.one;
        return obj;
    }

    public int Count()
    {
        return pool.Count;
    }
}