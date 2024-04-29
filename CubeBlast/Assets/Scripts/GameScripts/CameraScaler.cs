using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class CameraScaler : MonoBehaviour
{
    public float cameraOffset;
    private float aspectRatio = 0.5625f;
    public float padding = 2;
    public float yOffset = 1;    
    void Start()
    {
        Board board = FindObjectOfType<Board>();
        if (board != null)
        {
            RepositionCamera(board.Size);
        }
        else
        {
            Debug.Log("Board is null");
        }
    }
    void RepositionCamera(Size size)
    {
        int x = size.Width - 1; int y = size.Height - 1;

        Vector3 tempPosition = new Vector3(x / 2.0f, y / 2.0f + yOffset, cameraOffset);
        transform.position = tempPosition;
        /*if (size.Width >= size.Height)
        {
            Camera.main.orthographicSize = (size.Width / 2 + padding) / aspectRatio;
        }
        else
        {
            Camera.main.orthographicSize = size.Height / 2 + padding;
        }*/
        Camera.main.orthographicSize = (size.Width / 2 + padding) / aspectRatio;
    }
}
