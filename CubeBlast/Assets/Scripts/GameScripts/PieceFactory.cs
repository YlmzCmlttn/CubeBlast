using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class PieceFactory : MonoBehaviour
{
    public GameObject m_RedPrefab;
    public GameObject m_GreenPrefab;
    public GameObject m_BluePrefab;
    public GameObject m_YellowPrefab;

    public GameObject m_VasePrefab;
    public GameObject m_StonePrefab;
    public GameObject m_BoxPrefab;

    public GameObject m_VerticalRocketPiecePrefab;
    public GameObject m_HorizontalRocketPiecePrefab;
    public GameObject m_TntPiecePrefab;


    public static GameObject CreatePiece(string id, Vector2 position)
    {
        GameObject prefab = GetPrefabFromId(id);
        if (prefab == null)
        {
            Debug.LogError("Prefab not found for piece type: " + id);
            return null;
        }
        GameObject pieceObject = Instantiate(prefab, new Vector3(position.x, position.y, -1*position.y), Quaternion.identity);

        Piece piece = pieceObject.GetComponent<Piece>();
        if (piece == null)
        {
            Debug.LogError("Prefab does not contain a Piece component: " + prefab.name);
            return null;
        }
        return pieceObject;
    }
    private static GameObject GetPrefabFromId(string id)
    {
        switch (id)
        {
            case "r":
                return Instance.m_RedPrefab;
            case "g":
                return Instance.m_GreenPrefab;
            case "b":
                return Instance.m_BluePrefab;
            case "y":
                return Instance.m_YellowPrefab;
            case "t":
                return Instance.m_TntPiecePrefab;
            case "roh":
                return Instance.m_HorizontalRocketPiecePrefab;
            case "rov":
                return Instance.m_VerticalRocketPiecePrefab;
            case "bo":
                return Instance.m_BoxPrefab;
            case "s":
                return Instance.m_StonePrefab;
            case "v":
                return Instance.m_VasePrefab;
            default:
                return GetRandomCubePiecePrefab();
        }
    }
    private static GameObject GetRandomCubePiecePrefab()
    {
        GameObject[] cubePiecePrefabs = { Instance.m_RedPrefab, Instance.m_GreenPrefab, Instance.m_BluePrefab, Instance.m_YellowPrefab };
        return cubePiecePrefabs[Random.Range(0, cubePiecePrefabs.Length)];
    }
    private static PieceFactory instance;
    private static PieceFactory Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PieceFactory>();
                if (instance == null)
                {
                    Debug.LogError("PieceFactory not found in the scene.");
                }
            }
            return instance;
        }
    }
}
