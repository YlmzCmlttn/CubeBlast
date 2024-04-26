using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class GridBackground : MonoBehaviour
{
    private Board board;
    public Sprite backgroundSprite;
    public Sprite cornerSprite;
    public Sprite lineSprite;
    public float xOffset;
    public float yOffset;
    public bool updated;

    void Start()
    {
        board = FindObjectOfType<Board>();
        CreateBackground();
    }
    void CreateBackground()
    {
        GameObject blackGridBackground = new GameObject("blackGridBackground");
        SpriteRenderer spriteRenderer = blackGridBackground.AddComponent<SpriteRenderer>();
        spriteRenderer.color = Color.black;
        spriteRenderer.sprite = backgroundSprite;
        blackGridBackground.transform.parent = this.transform;
        blackGridBackground.transform.position = new Vector3((board.Size.Width/2.0f)-0.5f, (((board.Size.Height - 1) + (162.0f / 142.0f))/2.0f)-0.5f, 20);
        blackGridBackground.transform.localScale = new Vector3(board.Size.Width, (board.Size.Height-1)+(162.0f / 142.0f), 1);

        GameObject bottomleftCorner = new GameObject("bottomleftCorner");
        SpriteRenderer leftCornerSpriteRenderer = bottomleftCorner.AddComponent<SpriteRenderer>();
        leftCornerSpriteRenderer.sprite = cornerSprite;

        bottomleftCorner.transform.parent = this.transform;
        bottomleftCorner.transform.position = new Vector3(xOffset, yOffset, 10);

        GameObject bottomrightCorner = new GameObject("bottomrightCorner");
        SpriteRenderer bottomrightCornerSpriteRenderer = bottomrightCorner.AddComponent<SpriteRenderer>();
        bottomrightCornerSpriteRenderer.sprite = cornerSprite;
        bottomrightCorner.transform.parent = this.transform;
        bottomrightCorner.transform.rotation = Quaternion.Euler(
            0,
            0,
            90
        );

        bottomrightCorner.transform.position = new Vector3(board.Size.Width - 1 + -1*xOffset, yOffset, 10);



        GameObject toprightCorner = new GameObject("toprightCorner");
        SpriteRenderer toprightCornerSpriteRenderer = toprightCorner.AddComponent<SpriteRenderer>();
        toprightCornerSpriteRenderer.sprite = cornerSprite;
        toprightCorner.transform.parent = this.transform;
        toprightCorner.transform.rotation = Quaternion.Euler(
            0,
            0,
            180
        );

        toprightCorner.transform.position = new Vector3(board.Size.Width - 1 + -1 * xOffset, board.Size.Height - 1 + -1* yOffset, 10);

        GameObject topleftCorner = new GameObject("topleftCorner");
        SpriteRenderer topleftCornerSpriteRenderer = topleftCorner.AddComponent<SpriteRenderer>();
        topleftCornerSpriteRenderer.sprite = cornerSprite;
        topleftCorner.transform.parent = this.transform;
        topleftCorner.transform.rotation = Quaternion.Euler(
            0,
            0,
            270
        );

        topleftCorner.transform.position = new Vector3(xOffset, board.Size.Height - 1 + -1 * yOffset, 10);

        GameObject bottomLine = new GameObject("bottomLine");
        SpriteRenderer bottomLineSpriteRenderer = bottomLine.AddComponent<SpriteRenderer>();
        bottomLineSpriteRenderer.sprite = lineSprite;
        bottomLine.transform.parent = this.transform;
        bottomLine.transform.position = new Vector3(xOffset, yOffset, 10);
        bottomLine.transform.localScale = new Vector3((board.Size.Width - 0.2f) * 142.0f / 8.0f, 1, 0);

        GameObject rightLine = new GameObject("rightLine");
        SpriteRenderer rightLineSpriteRenderer = rightLine.AddComponent<SpriteRenderer>();
        rightLineSpriteRenderer.sprite = lineSprite;
        rightLine.transform.parent = this.transform;
        rightLine.transform.position = new Vector3(board.Size.Width - 1 + -1 * xOffset, yOffset, 10);
        rightLine.transform.localScale = new Vector3((board.Size.Height-1 - 0.2f) * 142.0f / 8.0f + 162.0f / 8.0f, 1, 0);

        rightLine.transform.rotation = Quaternion.Euler(
            0,
            0,
            90
        );

        GameObject topLine = new GameObject("topLine");
        SpriteRenderer topLineSpriteRenderer = topLine.AddComponent<SpriteRenderer>();
        topLineSpriteRenderer.sprite = lineSprite;
        topLine.transform.parent = this.transform;
        topLine.transform.position = new Vector3(board.Size.Width - 1 + -1 * xOffset, board.Size.Height - 1 + -1 * yOffset, 10);
        topLine.transform.localScale = new Vector3((board.Size.Width - 0.2f) * 142.0f / 8.0f, 1, 0);
        topLine.transform.rotation = Quaternion.Euler(
            0,
            0,
            180
        );

        GameObject leftLine = new GameObject("leftLine");
        SpriteRenderer leftLineSpriteRenderer = leftLine.AddComponent<SpriteRenderer>();
        leftLineSpriteRenderer.sprite = lineSprite;
        leftLine.transform.parent = this.transform;
        leftLine.transform.position = new Vector3(xOffset, board.Size.Height - 1 + -1 * yOffset, 10);
        leftLine.transform.localScale = new Vector3((board.Size.Height - 1 - 0.2f) * 142.0f / 8.0f + 162.0f / 8.0f, 1, 0);

        leftLine.transform.rotation = Quaternion.Euler(
            0,
            0,
            270
        );



    }

    private void Update()
    {
        if (updated)
        {
            CreateBackground();
            updated = false;
        }
    }
}
