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
	}

	public class Ellipse
	{
		public Vector3 Position = Vector2.zero;
		public Vector3 Center = Vector2.zero;
		public Vector3 Axis = Vector2.one;
		public float Roation = 0;
		public int Sides = 32;
		public float Width = 2.0f;
		public Color color = Color.white;
	}

	public class Circle : Ellipse
	{
		public float radius = 10f;
	}

	#region public variables
	public Color axisColor = Color.white;
	public Color lineColor = Color.gray;
	public Color divisionColor = Color.yellow;

	public Vector3 start, end, point;

	public bool isDrawingOrigin = true;
	public bool isDrawingAxis = true;
	public bool isDrawingDivisions = true;

	public float test;
	public float radius = 10;
	public float filledRadius = 10;
	public float rectWidth = 20;
	public float rectHeight = 10;
	public float triSize = 10;


	public int circleSides = 4;
	public int ellipseSides = 32;
	public int filledSides = 32;

	public Vector3 triP1, triP2, triP3;

	public float shipVelocity = 0;
	#endregion

	#region Private variables
	DrawingObjects diamond = new DrawingObjects();
	DrawingObjects hex = new DrawingObjects();
	DrawingObjects letterP = new DrawingObjects();
	DrawingObjects rectangle = new DrawingObjects();
	DrawingObjects filledRectangle = new DrawingObjects();
	DrawingObjects tri = new DrawingObjects();
	DrawingObjects filledTri = new DrawingObjects();
	DrawingObjects spaceship = new DrawingObjects();

	Ellipse ellipse = new Ellipse();
	Ellipse ellipse2 = new Ellipse();

	Circle circle = new Circle();
	Circle circle2 = new Circle();


	Vector3[] verts = new Vector3[4];
	Vector3[] rectVerts = new Vector3[4];
	Vector3 temp;

	Grid2D grid = new Grid2D();

	bool groupOne, drawCollisions = true;
	bool drawGraphs = false;

	float diamondRotSpeed;
	float dist;
	#endregion

	private void Start()
	{
		triP1 = new Vector3(-20, 12);
		triP2 = new Vector3(-40, 10);
		triP3 = new Vector3(-25, 35);

		grid.origin = new Vector3((Screen.width / 2), (Screen.height / 2), 0);
		grid.screenSize = new Vector3((Screen.width), (Screen.height), 0);

		#region ellipses and circles
		ellipse.Position = new Vector3(0, 0);
		ellipse.Axis = new Vector3(40, 10);
		ellipse.color = Color.green;

		circle.Position = new Vector3(0, 0);
		circle.color = Color.cyan;

		ellipse2.Position = new Vector3(30, 30);
		ellipse2.Axis = new Vector3(20, 30);
		ellipse2.color = Color.green;

		circle2.Position = new Vector3(-30, -30);
		circle2.color = Color.cyan;
		#endregion
	}

	private void Update()
	{
		#region update variables
		temp = new Vector3(0, 0, 0);
		point = Input.mousePosition;

		int count = 0;

		ellipse.Sides = ellipseSides;
		circle.radius = radius;
		circle.Sides = circleSides;

		ellipse2.Sides = ellipseSides;
		circle2.radius = radius;
		circle2.Sides = circleSides;
		#endregion

		if (isDrawingOrigin)
		{
			DrawOrigin();
		}

		// Commented out for lab 5
		#region Line drawing
		/*
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
		*/
		#endregion

		if (isDrawingAxis)
		{
			DrawLine(new Line(GridToScreen(new Vector3(-grid.screenSize.x, 0, 0)), GridToScreen(new Vector3(grid.screenSize.x * 2, 0, 0)), axisColor));
			DrawLine(new Line(GridToScreen(new Vector3(0, -grid.screenSize.y * 2, 0)), GridToScreen(new Vector3(0, grid.screenSize.y * 2, 0)), axisColor));
		}

		#region Spaceship

		ControlSpaceship();
		DrawSpaceShip(new Vector3(0, 0), Color.cyan);

		#endregion

		// Commented out for lab 5
		#region Draw Collisions
		/*
		if (drawCollisions)
		{
			if (GetDistToFilledCircle(new Vector3(20, 20), point, filledRadius))
			{
				DrawFilledCircle(new Vector3(20, 20), filledRadius, filledSides, Color.red);
			}
			else
			{
				DrawCircle(new Vector3(20, 20), filledRadius, filledSides, Color.blue);
			}

			DrawRectangle(new Vector3(-20, -20), rectWidth, rectHeight, Color.cyan);

			if (isInsideRect(rectVerts, point))
			{
				DrawFilledRectangle(new Vector3(-20, -20), rectWidth, rectHeight, Color.black);
			}

			DrawTriangle(triP1, triP2, triP3, Color.green);
			//DrawFilledCircle(triCenter(triP1, triP2, triP3), 1, 32, Color.blue);

			if (isInsideTri())
			{
				DrawFilledTriangle(triP1, triP2, triP3, Color.white); //DID NOT GET TO WORK
			}
		}
		*/
		#endregion

		// Commented out for lab 5
		#region Draw Graphs
		/*
		if (drawGraphs)
		{
			RotateDiamondAroundOrigin(1440);
			DrawHexagon();
			DrawLetterP();
			DrawParabolaOne();
			DrawParabolaTwo();
			DrawParabolaThree();
			DrawParabolaFour();

			if (groupOne)
			{
				DrawCircle(circle.Position, circle.radius, circle.Sides, circle.color);
				DrawElipse(ellipse.Position, ellipse.Axis, ellipse.Sides, ellipse.color);
			}
			else
			{
				DrawCircle(circle2.Position, circle2.radius, circle2.Sides, circle2.color);
				DrawElipse(ellipse2.Position, ellipse2.Axis, ellipse2.Sides, ellipse2.color);
			}
		}
		*/
		#endregion

		#region Keyboard/Mouse Controls

		if (Input.GetKeyDown(KeyCode.Tab))
		{
			if (groupOne)
			{
				groupOne = false;
			}
			else
			{
				groupOne = true;
			}
		}

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

		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			if (drawGraphs)
			{
				drawGraphs = false;
			}
			else
			{
				drawGraphs = true;
			}
		}

		if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			if (drawCollisions)
			{
				drawCollisions = false;
			}
			else
			{
				drawCollisions = true;
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
		verts[0] = GridToScreen(new Vector3(-grid.originSize, 0, 0));
		verts[1] = GridToScreen(new Vector3(0, grid.originSize, 0));
		verts[2] = GridToScreen(new Vector3(grid.originSize, 0, 0));
		verts[3] = GridToScreen(new Vector3(0, -grid.originSize, 0));

		Drawing.Glint.GLCommand diamond = new Drawing.Glint.GLCommand(Drawing.Glint.DrawMode.Quads, lineColor, verts);
		Glint.AddCommand(diamond);
	}

	#region Lab 1-3
	public void RotateDiamondAroundOrigin(float time = 5.0f)
	{
		diamondRotSpeed = 360 / time;

		Vector3 p1, p2, p3, p4 = new Vector3();

		diamond.Lines.Clear();

		p1 = GridToScreen(new Vector3(-7.5f, 15));
		p2 = GridToScreen(new Vector3(0, 3 * 7.5f));
		p3 = GridToScreen(new Vector3(7.5f, 15));
		p4 = GridToScreen(new Vector3(0, 7.5f));

		p1 = DrawingTools.RotatePoint(grid.origin, -test, p1);
		p2 = DrawingTools.RotatePoint(grid.origin, -test, p2);
		p3 = DrawingTools.RotatePoint(grid.origin, -test, p3);
		p4 = DrawingTools.RotatePoint(grid.origin, -test, p4);

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

		for (int x = -2000; x <= 2000; x++)
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
		if (sides < 3)
		{
			sides = 3;
		}

		Vector3 prevPoint = DrawingTools.CircleRadiusPoint(pos, (360f / sides), radius); //new Vector3(pos.x + radius, pos.y + radius);

		for (int x = 0; x <= sides; x++)
		{
			Vector3 nextPoint = DrawingTools.RotatePoint(pos, (360f / sides), prevPoint);

			DrawLine(new Line(GridToScreen(new Vector3(prevPoint.x, prevPoint.y)), GridToScreen(new Vector3(nextPoint.x, nextPoint.y)), color));

			prevPoint = nextPoint;
		}
	}

	public void DrawElipse(Vector3 pos, Vector2 axis, int sides, Color color)
	{
		if (sides < 3)
		{
			sides = 3;
		}

		Vector3 prevPoint = DrawingTools.EllipseRadiusPoint(pos, 0, axis);

		for (int x = 1; x <= sides; x++)
		{
			Vector3 nextPoint = DrawingTools.EllipseRadiusPoint(pos, (360f / sides) * x, axis);

			DrawLine(new Line(GridToScreen(new Vector3(prevPoint.x, prevPoint.y)), GridToScreen(new Vector3(nextPoint.x, nextPoint.y)), color));

			prevPoint = nextPoint;
		}
	}

	#endregion

	#region Collision Example

	public void DrawFilledCircle(Vector3 pos, float radius, int sides, Color color)
	{
		if (sides < 3)
		{
			sides = 3;
		}

		for (float r = radius; r > 0; r -= 0.1f)
		{
			Vector3 prevPoint = DrawingTools.CircleRadiusPoint(pos, (360f / sides), r);

			for (int x = 0; x <= sides; x++)
			{
				Vector3 nextPoint = DrawingTools.RotatePoint(pos, (360f / sides), prevPoint);

				DrawLine(new Line(GridToScreen(new Vector3(prevPoint.x, prevPoint.y)), GridToScreen(new Vector3(nextPoint.x, nextPoint.y)), color));

				prevPoint = nextPoint;
			}
		}
	}

	public bool GetDistToFilledCircle(Vector3 circleCenter, Vector3 point, float circleRadius)
	{
		dist = Vector3.Distance(circleCenter, ScreenToGrid(point));

		if (dist < circleRadius)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public void DrawRectangle(Vector3 pos, float width, float height, Color color)
	{
		rectangle.Lines.Clear();


		Vector3 p1, p2, p3, p4 = new Vector3();

		p1 = GridToScreen(new Vector3(pos.x - (width * 0.5f), pos.y + (height * 0.5f)));
		p2 = GridToScreen(new Vector3(pos.x + (width * 0.5f), pos.y + (height * 0.5f)));
		p3 = GridToScreen(new Vector3(pos.x + (width * 0.5f), pos.y - (height * 0.5f)));
		p4 = GridToScreen(new Vector3(pos.x - (width * 0.5f), pos.y - (height * 0.5f)));

		rectVerts[0] = p1;
		rectVerts[1] = p2;
		rectVerts[2] = p3;
		rectVerts[3] = p4;

		rectangle.Lines.Add(new Line(p1, p2, color));
		rectangle.Lines.Add(new Line(p2, p3, color));
		rectangle.Lines.Add(new Line(p3, p4, color));
		rectangle.Lines.Add(new Line(p4, p1, color));

		rectangle.Draw();
	}

	public void DrawFilledRectangle(Vector3 pos, float width, float height, Color color)
	{
		while (width > 0 || height > 0)
		{
			filledRectangle.Lines.Clear();


			Vector3 p1, p2, p3, p4 = new Vector3();

			p1 = GridToScreen(new Vector3(pos.x - (width * 0.5f), pos.y + (height * 0.5f)));
			p2 = GridToScreen(new Vector3(pos.x + (width * 0.5f), pos.y + (height * 0.5f)));
			p3 = GridToScreen(new Vector3(pos.x + (width * 0.5f), pos.y - (height * 0.5f)));
			p4 = GridToScreen(new Vector3(pos.x - (width * 0.5f), pos.y - (height * 0.5f)));

			filledRectangle.Lines.Add(new Line(p1, p2, color));
			filledRectangle.Lines.Add(new Line(p2, p3, color));
			filledRectangle.Lines.Add(new Line(p3, p4, color));
			filledRectangle.Lines.Add(new Line(p4, p1, color));

			filledRectangle.Draw();

			width -= 0.1f;
			height -= 0.1f;
		}
	}

	public bool isInsideRect(Vector3[] verts, Vector3 point)
	{
		if ((point.x >= verts[0].x && point.y <= verts[0].y) && (point.x <= verts[1].x && point.y <= verts[1].y) &&
		   (point.x <= verts[2].x && point.y >= verts[2].y) && (point.x >= verts[3].x && point.y >= verts[3].y))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public void DrawTriangle(Vector3 p1, Vector3 p2, Vector3 p3, Color color)
	{
		tri.Lines.Clear();

		tri.Lines.Add(new Line(GridToScreen(p1), GridToScreen(p2), color));
		tri.Lines.Add(new Line(GridToScreen(p2), GridToScreen(p3), color));
		tri.Lines.Add(new Line(GridToScreen(p3), GridToScreen(p1), color));

		tri.Draw();
	}

	//DID NOT GET TO WORK
	public void DrawFilledTriangle(Vector3 p1, Vector3 p2, Vector3 p3, Color color)
	{
		Vector3 center = triCenter(p1, p2, p3);

		filledTri.Lines.Clear();

		filledTri.Lines.Add(new Line(GridToScreen(p1), GridToScreen(p2), color));
		filledTri.Lines.Add(new Line(GridToScreen(p2), GridToScreen(p3), color));
		filledTri.Lines.Add(new Line(GridToScreen(p1), GridToScreen(p3), color));

		filledTri.Draw();
	}

	public bool isInsideTri()
	{
		float totalArea = triArea(GridToScreen(triP1), GridToScreen(triP2), GridToScreen(triP3));

		float area1 = triArea(GridToScreen(triP1), GridToScreen(triP2), point);
		float area2 = triArea(GridToScreen(triP2), GridToScreen(triP3), point);
		float area3 = triArea(GridToScreen(triP1), GridToScreen(triP3), point);

		//Debug.Log("Total: " + totalArea);
		//Debug.Log("Sum: " + (area1 + area2 + area3));

		float sum = area1 + area2 + area3;

		if (sum == totalArea)
		{
			return true;
		}
		else if (sum == totalArea + 10)
		{
			return true;
		}
		else if (sum == totalArea - 10)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public float triArea(Vector3 p1, Vector3 p2, Vector3 p3)
	{
		return Mathf.Abs((float)((p1.x * (p2.y - p3.y) + p2.x * (p3.y - p1.y) + p3.x * (p1.y - p2.y)) / 2.0));
	}

	public Vector3 triCenter(Vector3 p1, Vector3 p2, Vector3 p3)
	{
		float x = (p1.x + p2.x + p3.x) / 3f;
		float y = (p1.y + p2.y + p3.y) / 3f;

		Vector3 returnPoint = new Vector3(x, y);

		return returnPoint;
	}

	#endregion

	#region Spaceship
	
	public void DrawSpaceShip(Vector3 center, Color color)
	{
		spaceship.Lines.Clear();

		Vector3 p1, p2, p3 = new Vector3();

		p1 = GridToScreen(new Vector3(center.x - 5f, center.y + 2f));
		p2 = GridToScreen(new Vector3(center.x - 5f, center.y - 2f));
		p3 = GridToScreen(new Vector3(center.x + 5f, center.y));

		spaceship.Lines.Add(new Line(p1, p2, color));
		spaceship.Lines.Add(new Line(p2, p3, color));
		spaceship.Lines.Add(new Line(p3, p1, color));

		spaceship.Draw();
	}

	public void ControlSpaceship()
	{
		if(Input.GetKeyDown(KeyCode.W))
		{
			shipVelocity += 1f;
		}
	}

	#endregion
}