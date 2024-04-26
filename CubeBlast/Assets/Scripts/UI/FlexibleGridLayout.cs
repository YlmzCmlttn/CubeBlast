using UnityEngine;
using UnityEngine.UI;

public class FlexibleGridLayout : LayoutGroup
{
    public int rows;
    public int columns;
    public Vector2 cellSize;
    public Vector2 spacing;

    public override void CalculateLayoutInputVertical()
    {
        if(transform.childCount > 0)
        {
            float sqrRt = Mathf.Sqrt(transform.childCount);
            columns = Mathf.CeilToInt(sqrRt);
            rows = Mathf.CeilToInt(sqrRt);

            float parentWidth = rectTransform.rect.width;
            float parentHeight = rectTransform.rect.height;

            float cellWidth = (parentWidth / columns) - ((spacing.x / columns) * (columns - 1)) - (padding.left / columns) - (padding.right / columns);
            float cellHeight = (parentHeight / rows) - ((spacing.y / rows) * (rows - 1)) - (padding.top / rows) - (padding.bottom / rows);

            cellSize.x = cellWidth;
            cellSize.y = cellHeight;

            int columnCount = Mathf.CeilToInt(transform.childCount / (float)rows);
            int rowCount = Mathf.CeilToInt(transform.childCount / (float)columns);

            for (int i = 0; i < rectChildren.Count; i++)
            {
                int row = i % rowCount;
                int column = i / rowCount;

                RectTransform item = rectChildren[i];

                float xPos = (cellSize.x + spacing.x) * column + padding.left;
                float yPos = (cellSize.y + spacing.y) * row + padding.top;

                SetChildAlongAxis(item, 0, xPos, cellSize.x);
                SetChildAlongAxis(item, 1, yPos, cellSize.y);
            }
        }
    }

    public override void SetLayoutHorizontal() { }

    public override void SetLayoutVertical() { }

    protected override void OnValidate()
    {
        base.OnValidate();
        CalculateLayoutInputVertical();
    }
}
