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
		public float originSize = 2;

		public int divisionCount = 5;
		public int minDivisionCount = 2;

		public void DrawLine(Line line, bool DrawOnGrid = true)
		{

		}

		public void DrawObject(DrawingObjects lineObj, bool DrawOnGrid = true)
		{

		}
	}

	public Color axisColor = Color.white;
	public Color lineColor = Color.gray;
	public Color divisionColor = Color.yellow;

	public bool isDrawingOrigin = true;
	public bool isDrawingAxis = true;
	public bool isDrawingDivisions = true;

	public Vector3 start, end;

	bool diamondInit = false;

	DrawingObjects diamond = new DrawingObjects();
	DrawingObjects hex = new DrawingObjects();
	DrawingObjects letterP = new DrawingObjects();

	Vector3[] verts = new Vector3[4];

	Grid2D grid = new Grid2D();

	Vector3 temp;
	
	float diamondRotSpeed;

	public float test;

	private void Start()
	{
		grid.origin = new Vector3((Screen.width / 2), (Screen.height / 2), 0);
		grid.screenSize = new Vector3((Screen.width), (Screen.height), 0);
	}

	private void Update()
	{
		temp = new Vector3(0, 0, 0);
		int count = 0;

		if (isDrawingOrigin)
		{
			DrawOrigin();
		}

		#region Line drawing
		while (temp.x <= grid.screenSize.x)
		{
			if (count % grid.divisionCount == 0 && count != 0)
			{
				if (isDrawingDivisions)
				{
					DrawLine(new Line(GridToScreen(new Vector3(temp.x, grid.screenSize.y, 0)), GridToScreen(new Vector3(temp.x, -grid.screenSize.y, 0)), divisionColor));
				}
			}
			else if (count != 0)
			{
				if (isDrawingDivisions)
				{
					DrawLine(new Line(GridToScreen(new Vector3(temp.x, grid.screenSize.y, 0)), GridToScreen(new Vector3(temp.x, -grid.screenSize.y, 0)), lineColor));
				}
			}

			//Debug.Log(temp);

			temp.x += grid.gridSize * 0.2f;
			count++;
		}

		while (temp.x >= -grid.screenSize.x)
		{
			if (count % grid.divisionCount == 0 && count != 0)
			{
				if (isDrawingDivisions)
				{
					DrawLine(new Line(GridToScreen(new Vector3(temp.x, grid.screenSize.y, 0)), GridToScreen(new Vector3(temp.x, -grid.screenSize.y, 0)), divisionColor));
				}
			}
			else if (count != 0)
			{
				if (isDrawingDivisions)
				{
					DrawLine(new Line(GridToScreen(new Vector3(temp.x, grid.screenSize.y, 0)), GridToScreen(new Vector3(temp.x, -grid.screenSize.y, 0)), lineColor));
				}
			}

			temp.x -= grid.gridSize * 0.2f;
			count--;
		}

		count = 0;

		while (temp.y <= grid.screenSize.y)
		{
			if (count % grid.divisionCount == 0 && count != 0)
			{
				if (isDrawingDivisions)
				{
					DrawLine(new Line(GridToScreen(new Vector3(grid.screenSize.x, temp.y, 0)), GridToScreen(new Vector3(-grid.screenSize.x, temp.y, 0)), divisionColor));
				}
			}
			else if (count != 0)
			{
				if (isDrawingDivisions)
				{
					DrawLine(new Line(GridToScreen(new Vector3(grid.screenSize.x, temp.y, 0)), GridToScreen(new Vector3(-grid.screenSize.x, temp.y, 0)), lineColor));
				}
			}
			temp.y += grid.gridSize * 0.2f;
			count++;
		}

		while (temp.y >= -grid.screenSize.y)
		{
			if (count % grid.divisionCount == 0 && count != 0)
			{
				if (isDrawingDivisions)
				{
					DrawLine(new Line(GridToScreen(new Vector3(grid.screenSize.x, temp.y, 0)), GridToScreen(new Vector3(-grid.screenSize.x, temp.y, 0)), divisionColor));
				}
			}
			else if (count != 0)
			{
				if (isDrawingDivisions)
				{
					DrawLine(new Line(GridToScreen(new Vector3(grid.screenSize.x, temp.y, 0)), GridToScreen(new Vector3(-grid.screenSize.x, temp.y, 0)), lineColor));
				}
			}

			temp.y -= grid.gridSize * 0.2f;
			count--;
		}
		#endregion

		if (isDrawingAxis)
		{
			DrawLine(new Line(GridToScreen(new Vector3(-grid.screenSize.x, 0, 0)), GridToScreen(new Vector3(grid.screenSize.x * 2, 0, 0)), axisColor));
			DrawLine(new Line(GridToScreen(new Vector3(0, -grid.screenSize.y * 2, 0)), GridToScreen(new Vector3(0, grid.screenSize.y * 2, 0)), axisColor));
		}

		RotateDiamondAroundOrigin(1440);
		DrawHexagon();
		DrawLetterP();
		DrawParabolaOne();
		DrawParabolaTwo();
		DrawParabolaThree();
		DrawParabolaFour();

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
			if (isDrawingAxis)
			{
				isDrawingAxis = false;
			}
			else
			{
				isDrawingAxis = true;
			}
		}

		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			if (isDrawingDivisions)
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
				if ((grid.divisionCount + 5) == 0)
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
				if ((grid.divisionCount - 5) == 0)
				{
					grid.divisionCount -= 10;
				}
				else
				{
					grid.divisionCount -= 5;
				}
			}

			//Debug.Log("Division count: " + grid.divisionCount);
		}
		else
		{
			if (Input.mouseScrollDelta.y > 0)
			{
				grid.gridSize += 1f;
			}
			else if (Input.mouseScrollDelta.y < 0)
			{
				grid.gridSize -= 1f;
			}

			//Debug.Log("Grid size: " + grid.gridSize);
		}

		if (Input.GetMouseButton(2))
		{
			grid.origin = Input.mousePosition;
		}
		#endregion
	}

	//Takes the potential grid space and outputs it into screen space
	public Vector3 GridToScreen(Vector3 gridSpace)
	{
		return new Vector3(grid.origin.x + (gridSpace.x * grid.gridSize), grid.origin.y + (gridSpace.y * grid.gridSize), 0);
	}

	//Takes in screen space and outputs it as grid space
	public Vector3 ScreenToGrid(Vector3 screenSpace)
	{
		return new Vector3((screenSpace.x - grid.origin.x) / grid.gridSize, (screenSpace.y - grid.origin.y) / grid.gridSize, 0);
	}

	//Draws the given line
	public void DrawLine(Line line)
	{
		Glint.AddCommand(line);
	}

	//Draws the Origin Point (or Symbol)
	public void DrawOrigin()
	{
		/*DrawLine(new Line(GridToScreen(new Vector3(-grid.originSize, 0, 0)), GridToScreen(new Vector3(0, grid.originSize, 0)), lineColor));
        DrawLine(new Line(GridToScreen(new Vector3(0, grid.originSize, 0)), GridToScreen(new Vector3(grid.originSize, 0, 0)), lineColor));
        DrawLine(new Line(GridToScreen(new Vector3(grid.originSize, 0, 0)), GridToScreen(new Vector3(0, -grid.originSize, 0)), lineColor));
        DrawLine(new Line(GridToScreen(new Vector3(0, -grid.originSize, 0)), GridToScreen(new Vector3(-grid.originSize, 0, 0)), lineColor));

        start = DrawingTools.RotatePoint(grid.origin, 45.0f, new Vector3(-grid.originSize, 0, 0));
        end = DrawingTools.RotatePoint(grid.origin, 45.0f, new Vector3(0, grid.originSize, 0));

        DrawLine(new Line(GridToScreen(start), GridToScreen(end), lineColor));*/

		verts[0] = GridToScreen(new Vector3(-grid.originSize, 0, 0));
		verts[1] = GridToScreen(new Vector3(0, grid.originSize, 0));
		verts[2] = GridToScreen(new Vector3(grid.originSize, 0, 0));
		verts[3] = GridToScreen(new Vector3(0, -grid.originSize, 0));

		Drawing.Glint.GLCommand diamond = new Drawing.Glint.GLCommand(Drawing.Glint.DrawMode.Quads, lineColor, verts);
		Glint.AddCommand(diamond);
	}

	public void RotateDiamondAroundOrigin(float time = 5.0f)
	{
		diamondRotSpeed = 360 / time;

		Vector3 p1, p2, p3, p4 = new Vector3();

		diamond.Lines.Clear();
		
		p1 = GridToScreen(new Vector3(-7.5f, 15));
		p2 = GridToScreen(new Vector3(0, 3 * 7.5f));
		p3 = GridToScreen(new Vector3(7.5f, 15));
		p4 = GridToScreen(new Vector3(0, 7.5f));

		//diamond.Lines.Add(new Line(p1, p2, lineColor));
		//diamond.Lines.Add(new Line(p2, p3, lineColor));
		//diamond.Lines.Add(new Line(p3, p4, lineColor));
		//diamond.Lines.Add(new Line(p4, p1, lineColor));

		//Debug.Log("1: " + ScreenToGrid(p1));
		//Debug.Log("2: " + ScreenToGrid(p2));
		//Debug.Log("3: " + ScreenToGrid(p3));
		//Debug.Log("4: " + ScreenToGrid(p4));

		//diamond.Draw();

		//Debug.Log("Origin: " + grid.origin);


		p1 = DrawingTools.RotatePoint(grid.origin, -test, p1);
		p2 = DrawingTools.RotatePoint(grid.origin, -test, p2);
		p3 = DrawingTools.RotatePoint(grid.origin, -test, p3);
		p4 = DrawingTools.RotatePoint(grid.origin, -test, p4);

		//Debug.Log("1 post: " + ScreenToGrid(p1));
		//Debug.Log("2 post: " + ScreenToGrid(p2));
		//Debug.Log("3 post: " + ScreenToGrid(p3));
		//Debug.Log("4 post: " + ScreenToGrid(p4));

		diamond.Lines.Add(new Line(p1, p2, lineColor));
		diamond.Lines.Add(new Line(p2, p3, lineColor));
		diamond.Lines.Add(new Line(p3, p4, lineColor));
		diamond.Lines.Add(new Line(p4, p1, lineColor));

		diamond.Draw();

		test += diamondRotSpeed;
	}

	public void DrawHexagon()
	{
		hex.Lines.Clear();

		Vector3 hexCenter = GridToScreen(new Vector3(-40, 15));
		float rotAngle = 360 / 6;
		Vector3 p1, p2, p3, p4, p5, p6 = new Vector3();

		p1 = GridToScreen(new Vector3(-40, 20));

		p2 = DrawingTools.RotatePoint(hexCenter, -rotAngle, p1);
		p3 = DrawingTools.RotatePoint(hexCenter, -rotAngle, p2);
		p4 = DrawingTools.RotatePoint(hexCenter, -rotAngle, p3);
		p5 = DrawingTools.RotatePoint(hexCenter, -rotAngle, p4);
		p6 = DrawingTools.RotatePoint(hexCenter, -rotAngle, p5);

		hex.Lines.Add(new Line(p1, p2, lineColor));
		hex.Lines.Add(new Line(p2, p3, lineColor));
		hex.Lines.Add(new Line(p3, p4, lineColor));
		hex.Lines.Add(new Line(p4, p5, lineColor));
		hex.Lines.Add(new Line(p5, p6, lineColor));
		hex.Lines.Add(new Line(p6, p1, lineColor));

		hex.Draw();
	}

	public void DrawLetterP()
	{
		letterP.Lines.Clear();

		Vector3 p1, p2, p3, p4, p5 = new Vector3();

		p1 = GridToScreen(new Vector3(0, 0));
		p2 = GridToScreen(new Vector3(0, 10));
		p3 = GridToScreen(new Vector3(10, 10));
		p4 = GridToScreen(new Vector3(0, 20));
		p5 = GridToScreen(new Vector3(10, 20));

		letterP.Lines.Add(new Line(p1, p2, Color.red));
		letterP.Lines.Add(new Line(p2, p3, Color.red));
		letterP.Lines.Add(new Line(p2, p4, Color.red));
		letterP.Lines.Add(new Line(p3, p5, Color.red));
		letterP.Lines.Add(new Line(p4, p5, Color.red));

		letterP.Draw();
	}

	public void DrawParabolaOne()
	{
		Vector3 prevPoint = new Vector3(-2000, Mathf.Pow(-2000, 2));

		for(int x = -2000; x <= 2000; x++)
		{
			Vector3 nextPoint = new Vector3(x, x * x);

			DrawLine(new Line(GridToScreen(new Vector3(prevPoint.x, prevPoint.y)), GridToScreen(new Vector3(nextPoint.x, nextPoint.y)), Color.blue));

			prevPoint = nextPoint;
		}
		
	}

	public void DrawParabolaTwo()
	{
		Vector3 prevPoint = new Vector3(-2000, (Mathf.Pow(-2000, 2) + (2 * -2000) + 1));

		for (int x = -2000; x <= 2000; x++)
		{
			Vector3 nextPoint = new Vector3(x, (Mathf.Pow(x, 2) + (2 * x) + 1));

			DrawLine(new Line(GridToScreen(new Vector3(prevPoint.x, prevPoint.y)), GridToScreen(new Vector3(nextPoint.x, nextPoint.y)), Color.cyan));

			prevPoint = nextPoint;
		}

	}

	public void DrawParabolaThree()
	{
		Vector3 prevPoint = new Vector3(-2000, (-2 * (Mathf.Pow(-2000, 2)) + (10 * -2000) + 12));

		for (int x = -2000; x <= 2000; x++)
		{
			Vector3 nextPoint = new Vector3(x, (-2 * (Mathf.Pow(x, 2)) + (10 * x) + 12));

			DrawLine(new Line(GridToScreen(new Vector3(prevPoint.x, prevPoint.y)), GridToScreen(new Vector3(nextPoint.x, nextPoint.y)), Color.magenta));

			prevPoint = nextPoint;
		}

	}

	public void DrawParabolaFour()
	{
		Vector3 prevPoint = new Vector3((-(Mathf.Pow(-2000, 3))), -2000);

		for (int x = -2000; x <= 2000; x++)
		{
			Vector3 nextPoint = new Vector3((-(Mathf.Pow(x, 3))), x);

			DrawLine(new Line(GridToScreen(new Vector3(prevPoint.x, prevPoint.y)), GridToScreen(new Vector3(nextPoint.x, nextPoint.y)), Color.green));

			prevPoint = nextPoint;
		}

	}

	public void DrawCircle(Vector3 pos, float radius, int sides, Color color)
	{
		Vector3 prevPoint = new Vector3(Mathf.Sqrt(-Mathf.Pow(pos.y, 2) + Mathf.Pow(radius, 2)), -2000);
	}

	/*
    public float ScaleGrid2Screen(float value)
    {
        return (value * GridSize);
    }

    public float ScaleScreen2Grid(float value)
    {
        return (value / GridSize);
    }*/
}