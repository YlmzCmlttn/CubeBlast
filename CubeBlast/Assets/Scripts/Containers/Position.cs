using UnityEngine;

[System.Serializable]
public struct Position
{
    // Define private fields to store x and y values
    [SerializeField] private int x;
    [SerializeField] private int y;

    // Constructor to initialize x and y values
    public Position(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    // Properties to get and set the x and y values
    public int X
    {
        get { return x; }
        set { x = value; }
    }

    public int Y
    {
        get { return y; }
        set { y = value; }
    }

    // Properties to get the column and row values
    public int Column
    {
        get { return x; }
        set { x = value; }
    }

    public int Row
    {
        get { return y; }
        set { y = value; }
    }

    public Vector2Int Vector
    {
        get { return new Vector2Int(x, y); }
        set { x = value.x; y = value.y; }

    }
}