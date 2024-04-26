using UnityEngine;

[System.Serializable]
public enum PieceType
{
    CUBE,
    SPECIAL,
    OBSTACLE
}
[System.Serializable]
public enum SpecialItemType
{
    VERTICAL_ROCKET,
    HORIZONTAL_ROCKET,
    TNT,
}

[System.Serializable]
public enum GameState
{
    WIN,
    LOSE,
    MOVE,
    WAIT
}
