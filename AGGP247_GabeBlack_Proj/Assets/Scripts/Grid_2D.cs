using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid_2D : MonoBehaviour
{
    public class Grid2D
    {
        public Vector3 screenSize;
        public Vector3 origin;

        public float gridSize = 10f;
        public float minGridSize = 2f;
        public float originSize = 20f;

        public int divisionCount = 5;
        public int minDivisionCount = 2;
    }

    public Color axisColor = Color.white;
    public Color lineColor = Color.gray;
    public Color divisionColor = Color.yellow;

    public bool isDrawingOrigin = true;
    public bool isDrawingAxis = true;
    public bool isDrawingDivisions = true;

    public Vector3 gridSize;

    Grid2D grid = new Grid2D();
    DrawingTools dt = new DrawingTools();

    Vector3 temp;

	private void Start()
    {
        grid.origin = new Vector3((Screen.width / 2), (Screen.height / 2), 0);
        grid.screenSize = new Vector3((Screen.width), (Screen.height), 0);
    }

    private void Update()
    {
        temp = ScreenToGrid(new Vector3(0, 0, 0));
        int count = 0;

        if (isDrawingOrigin)
        {
            DrawOrigin();
        }

        #region Line drawing
        while (temp.x >= 0)
		{
            if (count % grid.divisionCount == 0 && count != 0)
            {
                if(isDrawingDivisions)
				{
                    DrawLine(new Line(new Vector3(temp.x, grid.screenSize.y, 0), new Vector3(temp.x, -grid.screenSize.y, 0), divisionColor));
                }
            }
            else if (count != 0)
            {
                DrawLine(new Line(new Vector3(temp.x, grid.screenSize.y, 0), new Vector3(temp.x, -grid.screenSize.y, 0), lineColor));
            }

            temp.x -= (2 * grid.gridSize);
            count++;
        }
        
        while (temp.x <= grid.screenSize.x)
        {
            if (count % grid.divisionCount == 0 && count != 0)
            {
                if (isDrawingDivisions)
                {
                    DrawLine(new Line(new Vector3(temp.x, grid.screenSize.y, 0), new Vector3(temp.x, -grid.screenSize.y, 0), divisionColor));
                }
            }
            else if (count != 0)
            {
                DrawLine(new Line(new Vector3(temp.x, grid.screenSize.y, 0), new Vector3(temp.x, -grid.screenSize.y, 0), lineColor));
            }

            temp.x += (2 * grid.gridSize);
            count--;
        }
        
        count = 0;

        while (temp.y >= 0)
        {
            if (count % grid.divisionCount == 0 && count != 0)
            {
                if (isDrawingDivisions)
                {
                    DrawLine(new Line(new Vector3(grid.screenSize.x, temp.y, 0), new Vector3(-grid.screenSize.x, temp.y, 0), divisionColor));
                }
            }
            else if (count != 0)
            {
                DrawLine(new Line(new Vector3(grid.screenSize.x, temp.y, 0), new Vector3(-grid.screenSize.x, temp.y, 0), lineColor));
            }

            temp.y -= (2 * grid.gridSize);
            count++;
        }

        while (temp.y <= grid.screenSize.y)
        {
            if (count % grid.divisionCount == 0 && count != 0)
            {
                if (isDrawingDivisions)
                {
                    DrawLine(new Line(new Vector3(grid.screenSize.x, temp.y, 0), new Vector3(-grid.screenSize.x, temp.y, 0), divisionColor));
                }
            }
            else if (count != 0)
            {
                DrawLine(new Line(new Vector3(grid.screenSize.x, temp.y, 0), new Vector3(-grid.screenSize.x, temp.y, 0), lineColor));
            }

            temp.y += (2 * grid.gridSize);
            count--;
        }
        #endregion

        if (isDrawingAxis)
        {
            DrawLine(new Line(GridToScreen(new Vector3(-grid.screenSize.x * 2, 0, 0)), GridToScreen(new Vector3(grid.screenSize.x * 2, 0, 0)), axisColor));
            DrawLine(new Line(GridToScreen(new Vector3(0, -grid.screenSize.y * 2, 0)), GridToScreen(new Vector3(0, grid.screenSize.y * 2, 0)), axisColor));
        }

		#region Keyboard/Mouse Controls
		if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (isDrawingOrigin)
            {
                isDrawingOrigin = false;
            }
            else
            {
                isDrawingOrigin = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
		{
            if(isDrawingAxis)
			{
                isDrawingAxis = false;
			}
            else
			{
                isDrawingAxis = true;
			}
		}

        if(Input.GetKeyDown(KeyCode.Alpha3))
		{
            if(isDrawingDivisions)
			{
                isDrawingDivisions = false;
			}
            else
			{
                isDrawingDivisions = true;
			}
		}

        if (Input.GetKey(KeyCode.LeftControl))
		{
            if (Input.mouseScrollDelta.y > 0)
            {
                if((grid.divisionCount + 5) == 0)
				{
                    grid.divisionCount += 10;
				}
                else
				{
                    grid.divisionCount += 5;
                }
            }
            else if (Input.mouseScrollDelta.y < 0)
            {
                if((grid.divisionCount - 5) == 0)
				{
                    grid.divisionCount -= 10;
                }
                else
				{
                    grid.divisionCount -= 5;
                }
            }

            Debug.Log("Division count: " + grid.divisionCount);
        }
        else
		{
            if (Input.mouseScrollDelta.y > 0)
            {
                grid.gridSize += 0.2f;
            }
            else if (Input.mouseScrollDelta.y < 0)
            {
                grid.gridSize -= 0.2f;
            }

            Debug.Log("Grid size: " + grid.gridSize);
        }

        if(Input.GetMouseButton(2))
		{
            grid.origin = Input.mousePosition;
		}
		#endregion
	}

	//Takes the potential grid space and outputs it into screen space
	public Vector3 GridToScreen(Vector3 gridSpace)
    {
        return new Vector3(grid.origin.x + gridSpace.x, grid.origin.y + gridSpace.y, 0);
    }

    //Takes in screen space and outputs it as grid space
    public Vector3 ScreenToGrid(Vector3 screenSpace)
    {
        return new Vector3(grid.origin.x - screenSpace.x, grid.origin.y - screenSpace.y, 0);
    }

    //Draws the given line
    public void DrawLine(Line line)
    {
        Glint.AddCommand(line);
    }

    //Draws the Origin Point (or Symbol)
    public void DrawOrigin()
    {
        DrawLine(new Line(GridToScreen(new Vector3(-grid.originSize, 0, 0)), GridToScreen(new Vector3(0, grid.originSize, 0)), lineColor));
        DrawLine(new Line(GridToScreen(new Vector3(0, grid.originSize, 0)), GridToScreen(new Vector3(grid.originSize, 0, 0)), lineColor));
        DrawLine(new Line(GridToScreen(new Vector3(grid.originSize, 0, 0)), GridToScreen(new Vector3(0, -grid.originSize, 0)), lineColor));
        DrawLine(new Line(GridToScreen(new Vector3(0, -grid.originSize, 0)), GridToScreen(new Vector3(-grid.originSize, 0, 0)), lineColor));
    }

    public void DrawLine(Line line, bool DrawOnGrid = true)
	{

	}

    /*public void DrawObject(DrawingObject lineObj, bool DrawOnGrid = true)
	{

	}

    public float ScaleGrid2Screen(float value)
    {
        return (value * GridSize);
    }

    public float ScaleScreen2Grid(float value)
    {
        return (value / GridSize);
    }*/
}