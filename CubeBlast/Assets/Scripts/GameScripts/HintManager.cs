using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintManager
{
    public static void CheckHints(Board m_Board)
    {
        bool [,] checkedCubes = new bool[m_Board.Size.Width, m_Board.Size.Height];
        List<GameObject> currentMatches = new List<GameObject>();
        for (int i = 0; i < m_Board.Size.Width; i++)
        {
            for (int j = 0; j < m_Board.Size.Height; j++)
            {
                
                if (!checkedCubes[i, j])
                {
                    if(m_Board.allPieces[i, j] != null)
                    {

                        if (m_Board.allPieces[i, j].GetComponent<Piece>().Type == PieceType.CUBE)
                        {
                            currentMatches.Clear();
                            CompareAdjancents(currentMatches, m_Board, new Vector2Int(i, j), m_Board.allPieces[i, j].tag, checkedCubes);
                            if (currentMatches.Count >= 5)
                            {
                                foreach (GameObject gameObject in currentMatches)
                                {
                                    CubePiece cube = gameObject.GetComponent<CubePiece>();
                                    cube.MakeTntHint();
                                }
                            }
                            else if (currentMatches.Count >= 3)
                            {
                                foreach (GameObject gameObject in currentMatches)
                                {
                                    CubePiece cube = gameObject.GetComponent<CubePiece>();
                                    cube.MakeRocketHint();
                                }
                            }
                            else if(currentMatches.Count > 0)
                            {
                                foreach (GameObject gameObject in currentMatches)
                                {
                                    CubePiece cube = gameObject.GetComponent<CubePiece>();
                                    cube.MakeDefaultSprite();
                                }
                            }
                            else
                            {
                                m_Board.allPieces[i, j].GetComponent<CubePiece>().MakeDefaultSprite();
                            }

                        }
                    }                    
                }
            }
        }
    }
    static void CompareAdjancents(List<GameObject> matches, Board board, Vector2Int position, string tag,bool[,] checkedCubes)
    {
        if (position.x > 0 && position.x < board.Size.Width)
        {
            CompareAdjancent(matches, board, position + Vector2Int.left, tag, checkedCubes);
        }
        if (position.x >= 0 && position.x < board.Size.Width - 1)
        {
            CompareAdjancent(matches, board, position + Vector2Int.right, tag, checkedCubes);
        }
        if (position.y > 0 && position.y < board.Size.Height)
        {
            CompareAdjancent(matches, board, position + Vector2Int.down, tag, checkedCubes);
        }
        if (position.y >= 0 && position.y < board.Size.Height - 1)
        {
            CompareAdjancent(matches, board, position + Vector2Int.up, tag, checkedCubes);
        }
    }
    static void CompareAdjancent(List<GameObject> matches, Board board, Vector2Int position, string tag, bool[,] checkedCubes)
    {
        if (board.allPieces[position.x, position.y] != null)
        {
            if (board.allPieces[position.x, position.y].CompareTag(tag))
            {
                GameObject match = board.allPieces[(uint)position.x, (uint)position.y];
                if (match != null)
                {
                    if (!matches.Contains(match))
                    {
                        checkedCubes[position.x, position.y] = true;
                        matches.Add(match);
                        CompareAdjancents(matches, board, position, tag, checkedCubes);
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
