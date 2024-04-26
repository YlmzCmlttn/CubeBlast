using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] public World m_World;
    private Level m_Level;
    [SerializeField] private int m_LevelIndex;

    public Size Size { get; private set; }

    public GameObject[,] allPieces;

    public GameObject[] RocketParts = new GameObject[4];

    public GameObject tntParticle;
    public GameState gameState;
    [SerializeField] private bool m_ManuelLevelSelection;
    private List<GameObject> pool;

    private void Awake()
    {
        if (!m_ManuelLevelSelection)
        {
            if (PlayerPrefs.HasKey("Level"))
            {
                m_LevelIndex = PlayerPrefs.GetInt("Level");
            }
        }
        SetUpLevel();
    }

    void SetUpLevel()
    {
        if (m_World != null)
        {
            if (m_LevelIndex < m_World.levels.Length)
            {
                if (m_World.levels[m_LevelIndex] != null)
                {
                    Size = new Size(m_World.levels[m_LevelIndex].gridWidth, m_World.levels[m_LevelIndex].gridHeight);
                }
                m_Level = m_World.levels[m_LevelIndex];
            }
        }
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

    public Level GetLevel()
    {
        return m_Level;
    }
    void Start()
    {
        pool = new List<GameObject>();
        allPieces = new GameObject[Size.Width, Size.Height];
        gameState = GameState.MOVE;
        SetUpPieces();
        HintManager.CheckHints(this);
    }
    void SetUpPieces()
    {
        for (int i = 0; i < Size.Width; i++)
        {
            for (int j = 0; j < Size.Height; j++)
            {
                long indexNumber = i + j * Size.Width;
                string pieceId = m_Level.grid[indexNumber];

                GameObject piece = PieceFactory.CreatePiece(pieceId, new Vector2(i, j));

                if (piece != null)
                {
                    if (piece.GetComponent<Piece>().Type != PieceType.CUBE)
                    {
                        PushObject(PieceFactory.CreatePiece("rand", new Vector2(i, j)));
                    }
                    piece.GetComponent<Piece>().position = new Position(i, j);
                    piece.transform.parent = transform;
                    allPieces[i, j] = piece;
                }
            }
        }
    }
    IEnumerator DecreaseRowCo()
    {
        yield return new WaitForSeconds(0.1f);
        int nullCount = 0;
        for (int i = 0; i < Size.Width; i++)
        {
            for (int j = 0; j < Size.Height; j++)
            {
                if (allPieces[i, j] == null)
                {
                    nullCount++;
                }
                else
                {
                    if (nullCount > 0)
                    {
                        if (allPieces[i, j].GetComponent<Piece>().moveable)
                        {
                            GameObject pieceObject = allPieces[i, j];
                            Piece piece = pieceObject.GetComponent<Piece>();
                            piece.position.Y -= nullCount;
                            allPieces[i, j] = null;
                            allPieces[i, piece.position.Y] = pieceObject;
                        }
                        else
                        {
                            nullCount = 0;
                        }
                    }
                }
            }
            nullCount = 0;
        }
        RefillBoard();
    }
    public void DecreaseRow()
    {
        
        StartCoroutine(DecreaseRowCo());
    }
    private void RefillBoard()
    {
        StartCoroutine(RefillBoardCo());
    }
    private IEnumerator CheckHintsCo()
    {
        yield return new WaitForSeconds(0.1f);
        HintManager.CheckHints(this);
    }
    private IEnumerator RefillBoardCo()
    {
        if (gameState == GameState.MOVE)
        {
            gameState = GameState.WAIT;
        }
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < Size.Width; i++)
        {
            for (int j = Size.Height - 1; j >= 0; j--)
            {
                if (allPieces[i, j] == null)
                {
                    GameObject piece = PopObject();

                    if (piece != null)
                    {
                        piece.GetComponent<Piece>().position = new Position(i, j);
                        piece.transform.parent = transform;
                        piece.transform.position = new Vector3(i, Size.Height + j*1.3f, -1 * j);
                        allPieces[i, j] = piece;
                    }
                }
                else
                {
                    break;
                }
            }
        }
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(CheckHintsCo());
        yield return new WaitForSeconds(0.2f);
        if(gameState == GameState.WAIT)
        {
            gameState = GameState.MOVE;
        }

    }
    public void CreateTntAt(Position position)
    {
        GameObject piece = PieceFactory.CreatePiece("t", position.Vector);
        if (piece != null)
        {
            piece.GetComponent<Piece>().position = position;
            piece.transform.parent = transform;
            allPieces[position.X, position.Y] = piece;
        }
    }

    public void CreateRocketAt(Position position)
    {
        int vertical = Random.Range(0, 100);
        GameObject piece;
        if (vertical > 50)
        {
            piece = PieceFactory.CreatePiece("rov", position.Vector);
        }
        else
        {
            piece = PieceFactory.CreatePiece("roh", position.Vector);
        }
        if (piece != null)
        {
            piece.GetComponent<Piece>().position = position;
            piece.transform.parent = transform;
            allPieces[position.X, position.Y] = piece;
        }
    }
    public bool isInsideTheBoardAndNotNull(int x, int y)
    {
        return isInsideTheBoardAndNotNull(new Position(x, y));
    }
    public bool isInsideTheBoard(Position pos)
    {
        return (pos.X >= 0 && pos.Y >= 0 && pos.X < Size.Width && pos.Y < Size.Height);
    }
    public bool isInsideTheBoardAndNotNull(Position pos)
    {
        if (isInsideTheBoard(pos))
        {
            return allPieces[pos.X,pos.Y] != null;
        }
        else
        {
            return false;
        }
    }

    public void TriggerHorizontalRocketAt(Position position)
    {
        StartCoroutine(TriggerHorizontalRocketAtCo(position));
    }
    public void TriggerVerticalRocketAt(Position position)
    {
        StartCoroutine(TriggerVerticalRocketAtCo(position));
    }
    public void TriggerTntAt(Position position)
    {
        StartCoroutine(TriggerTntAtCo(position));

    }
    public void TriggerComboTntAt(Position position)
    {
        StartCoroutine(TriggerComboTntAtCo(position));

    }

    IEnumerator TriggerComboTntAtCo(Position position)
    {

        GameObject particle = Instantiate(tntParticle, new Vector3(position.X, position.Y), Quaternion.identity);
        yield return null;
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (isInsideTheBoardAndNotNull(position.X + i, position.Y + j))
                {
                    allPieces[position.X + i, position.Y + j].GetComponent<Piece>().DamagedBy(PieceType.SPECIAL);
                }
            }
        }
        yield return new WaitForSeconds(0.05f);
        for (int i = -2; i < 3; i++)
        {
            if (isInsideTheBoardAndNotNull(position.X + i, position.Y - 2))
            {
                allPieces[position.X + i, position.Y - 2].GetComponent<Piece>().DamagedBy(PieceType.SPECIAL);
            }
            if (isInsideTheBoardAndNotNull(position.X + i, position.Y + 2))
            {
                allPieces[position.X + i, position.Y + 2].GetComponent<Piece>().DamagedBy(PieceType.SPECIAL);
            }

            if (isInsideTheBoardAndNotNull(position.X - 2, position.Y + i))
            {
                allPieces[position.X - 2, position.Y + i].GetComponent<Piece>().DamagedBy(PieceType.SPECIAL);
            }
            if (isInsideTheBoardAndNotNull(position.X + 2, position.Y + i))
            {
                allPieces[position.X + 2, position.Y + i].GetComponent<Piece>().DamagedBy(PieceType.SPECIAL);
            }
        }

        yield return new WaitForSeconds(0.05f);
        for (int i = -3; i < 4; i++)
        {
            if (isInsideTheBoardAndNotNull(position.X + i, position.Y - 3))
            {
                allPieces[position.X + i, position.Y - 3].GetComponent<Piece>().DamagedBy(PieceType.SPECIAL);
            }
            if (isInsideTheBoardAndNotNull(position.X + i, position.Y + 3))
            {
                allPieces[position.X + i, position.Y + 3].GetComponent<Piece>().DamagedBy(PieceType.SPECIAL);
            }

            if (isInsideTheBoardAndNotNull(position.X - 3, position.Y + i))
            {
                allPieces[position.X - 3, position.Y + i].GetComponent<Piece>().DamagedBy(PieceType.SPECIAL);
            }
            if (isInsideTheBoardAndNotNull(position.X + 3, position.Y + i))
            {
                allPieces[position.X + 3, position.Y + i].GetComponent<Piece>().DamagedBy(PieceType.SPECIAL);
            }
        }
        DecreaseRow();
        yield return new WaitForSeconds(0.5f);
        Destroy(particle);
    }

    IEnumerator TriggerTntAtCo(Position position)
    {

        GameObject particle = Instantiate(tntParticle, new Vector3(position.X, position.Y), Quaternion.identity);        
        yield return null;
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (isInsideTheBoardAndNotNull(position.X + i, position.Y + j))
                {
                    allPieces[position.X + i, position.Y + j].GetComponent<Piece>().DamagedBy(PieceType.SPECIAL);
                }
            }
        }
        yield return new WaitForSeconds(0.05f);
        for (int i = -2; i < 3; i++)
        {
            if (isInsideTheBoardAndNotNull(position.X + i, position.Y - 2))
            {
                allPieces[position.X + i, position.Y - 2].GetComponent<Piece>().DamagedBy(PieceType.SPECIAL);
            }
            if (isInsideTheBoardAndNotNull(position.X + i, position.Y + 2))
            {
                allPieces[position.X + i, position.Y + 2].GetComponent<Piece>().DamagedBy(PieceType.SPECIAL);
            }

            if (isInsideTheBoardAndNotNull(position.X - 2, position.Y + i))
            {
                allPieces[position.X - 2, position.Y + i].GetComponent<Piece>().DamagedBy(PieceType.SPECIAL);
            }
            if (isInsideTheBoardAndNotNull(position.X + 2, position.Y + i))
            {
                allPieces[position.X + 2, position.Y + i].GetComponent<Piece>().DamagedBy(PieceType.SPECIAL);
            }
        }
        DecreaseRow();
        yield return new WaitForSeconds(0.5f);
        Destroy(particle);
    }

    IEnumerator TriggerVerticalRocketAtCo(Position position)
    {
        bool decreased = false;
        GameObject partTop = Instantiate(RocketParts[0], new Vector3(position.X, position.Y, -10), Quaternion.identity);
        GameObject partBottom = Instantiate(RocketParts[1], new Vector3(position.X, position.Y, -10), Quaternion.identity);
        Queue<Piece> topTargets = new Queue<Piece>();
        Queue<Piece> bottomTargets = new Queue<Piece>();
        Piece topCurrentTarget = null;
        Piece bottomCurrentTarget = null;
        Vector3 topEndPosition = new Vector3(position.X, Size.Height + 50, -10);
        Vector3 bottomEndPosition = new Vector3(position.X, -50, -10);
        for (int i = position.Y - 1; i >= 0; i--)
        {
            if (isInsideTheBoardAndNotNull(position.X, i))
            {
                bottomTargets.Enqueue(allPieces[position.X, i].GetComponent<Piece>());

            }
        }
        if (bottomTargets.Count > 0)
        {
            bottomCurrentTarget = bottomTargets.Dequeue();
        }

        for (int i = position.Y + 1; i < Size.Height; i++)
        {
            if (isInsideTheBoardAndNotNull(position.X, i))
            {
                topTargets.Enqueue(allPieces[position.X, i].GetComponent<Piece>());
            }
        }
        if (topTargets.Count > 0)
        {
            topCurrentTarget = topTargets.Dequeue();
        }
        while ((Mathf.Abs(bottomEndPosition.y - partBottom.transform.position.y) > 0.1) || (Mathf.Abs(topEndPosition.y - partTop.transform.position.y) > 0.1))
        {
            yield return null;
            partBottom.transform.position = Vector3.MoveTowards(partBottom.transform.position, bottomEndPosition, 30f * Time.deltaTime);
            if (bottomCurrentTarget != null)
            {
                if (partBottom.transform.position.y < bottomCurrentTarget.position.Y)
                {
                    bottomCurrentTarget.DamagedBy(PieceType.SPECIAL);
                    if (bottomTargets.Count > 0)
                    {
                        bottomCurrentTarget = bottomTargets.Dequeue();
                    }
                    else
                    {
                        bottomCurrentTarget = null;
                    }
                }
            }

            partTop.transform.position = Vector3.MoveTowards(partTop.transform.position, topEndPosition, 30f * Time.deltaTime);
            if (topCurrentTarget != null)
            {
                if (partTop.transform.position.y > topCurrentTarget.position.Y)
                {
                    topCurrentTarget.DamagedBy(PieceType.SPECIAL);
                    if (topTargets.Count > 0)
                    {
                        topCurrentTarget = topTargets.Dequeue();
                    }
                    else
                    {
                        topCurrentTarget = null;
                    }
                }
            }
            if (topCurrentTarget == null && bottomCurrentTarget == null && !decreased)
            {
                yield return null;
                DecreaseRow();
                decreased = true;
            }
        }
        partBottom.transform.position = bottomEndPosition;
        partTop.transform.position = topEndPosition;
        Destroy(partBottom);
        Destroy(partTop);
    }


    IEnumerator TriggerHorizontalRocketAtCo(Position position)
    {
        bool decreased = false;
        GameObject partLeft = Instantiate(RocketParts[2], new Vector3(position.X, position.Y, -10), Quaternion.identity);
        GameObject partRight = Instantiate(RocketParts[3], new Vector3(position.X, position.Y, -10), Quaternion.identity);
        Queue<Piece> leftTargets = new Queue<Piece>();
        Queue<Piece> rightTargets = new Queue<Piece>();
        Piece leftCurrentTarget = null;
        Piece rightCurrentTarget = null;
        Vector3 leftEndPosition = new Vector3(-20, position.Y, -10);
        Vector3 rightEndPosition = new Vector3(Size.Width + 20, position.Y, -10);
        for (int i = position.X - 1; i >= 0; i--)
        {
            if (isInsideTheBoardAndNotNull(i, position.Y))
            {
                leftTargets.Enqueue(allPieces[i, position.Y].GetComponent<Piece>());
                
            }
        }
        if (leftTargets.Count > 0)
        {
            leftCurrentTarget = leftTargets.Dequeue();
        }

        for (int i = position.X + 1; i < Size.Width; i++)
        {
            if (isInsideTheBoardAndNotNull(i, position.Y))
            {
                rightTargets.Enqueue(allPieces[i, position.Y].GetComponent<Piece>());
                
            }
        }
        if (rightTargets.Count > 0)
        {
            rightCurrentTarget = rightTargets.Dequeue();
        }
        while ((Mathf.Abs(leftEndPosition.x - partLeft.transform.position.x) > 0.1) || (Mathf.Abs(rightEndPosition.x - partRight.transform.position.x) > 0.1))
        {
            yield return null;
            partLeft.transform.position = Vector3.MoveTowards(partLeft.transform.position, leftEndPosition, 30f * Time.deltaTime);
            if (leftCurrentTarget != null)
            {
                if (partLeft.transform.position.x < leftCurrentTarget.position.X)
                {
                    leftCurrentTarget.DamagedBy(PieceType.SPECIAL);
                    if (leftTargets.Count > 0)
                    {
                        leftCurrentTarget = leftTargets.Dequeue();
                    }
                    else
                    {
                        leftCurrentTarget = null;
                    }
                }
            }

            partRight.transform.position = Vector3.MoveTowards(partRight.transform.position, rightEndPosition, 30f * Time.deltaTime);
            if (rightCurrentTarget != null)
            {
                if (partRight.transform.position.x > rightCurrentTarget.position.X)
                {
                    rightCurrentTarget.DamagedBy(PieceType.SPECIAL);
                    if (rightTargets.Count > 0)
                    {
                        rightCurrentTarget = rightTargets.Dequeue();
                    }
                    else
                    {
                        rightCurrentTarget = null;
                    }
                }
            }
            if (rightCurrentTarget == null && leftCurrentTarget == null && !decreased)
            {

                yield return null;
                DecreaseRow();
                decreased = true;
            }
        }
        partLeft.transform.position = leftEndPosition;
        partRight.transform.position = rightEndPosition;
        Destroy(partLeft);
        Destroy(partRight);
    }
    void Update()
    {
    }


}
