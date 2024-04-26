using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindMatches
{    
    public static List<GameObject> FindAdjancents(Board board, Vector2Int position, string tag)
    {
        List<GameObject> matches = new List<GameObject>();
        CompareAdjancents(matches, board, position,tag);
        return matches;
    }

    static void CompareAdjancents(List<GameObject> matches, Board board, Vector2Int position, string tag)
    {
        if (position.x > 0 && position.x < board.Size.Width)
        {
            CompareAdjancent(matches, board, position + Vector2Int.left, tag);
        }
        if (position.x >= 0 && position.x < board.Size.Width - 1)
        {
            CompareAdjancent(matches, board, position + Vector2Int.right, tag);
        }
        if (position.y > 0 && position.y < board.Size.Height)
        {
            CompareAdjancent(matches, board, position + Vector2Int.down, tag);
        }
        if (position.y >= 0 && position.y < board.Size.Height - 1)
        {
            CompareAdjancent(matches, board, position + Vector2Int.up, tag);
        }
    }
    static void CompareAdjancent(List<GameObject> matches, Board board, Vector2Int position, string tag)
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
                        matches.Add(match);
                        CompareAdjancents(matches, board, position, tag);
                    }
                }
            }
        }
    }
}
