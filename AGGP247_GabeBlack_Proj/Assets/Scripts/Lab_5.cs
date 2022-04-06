using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lab_5 : MonoBehaviour
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

	public Color axisColor = Color.white;
	public Color lineColor = Color.gray;
	public Color divisionColor = Color.yellow;

	public Vector3 point;
	public Vector3 shipCenter;
	public Vector3 shipVelocity;
	public Vector3 testForce;

	public bool isDrawingOrigin = true;
	public bool isDrawingAxis = true;

	public float shipRotSpeed = 0.5f;
	public float shipRotAngle;
	public float shipMoveSpeed = 10f;
	public float shipMass = 40f;
	public float shipMaxMoveSpeed = 10;

	DrawingObjects spaceship = new DrawingObjects();

	Vector3[] verts = new Vector3[4];
	Vector3[] shipPoints = new Vector3[3];

	Grid2D grid = new Grid2D();

	private void Start()
	{
		grid.origin = new Vector3((Screen.width / 2), (Screen.height / 2), 0);
		grid.screenSize = new Vector3((Screen.width), (Screen.height), 0);

		shipCenter = new Vector3(0, 0);

		Vector3 p1, p2, p3 = new Vector3();

		p1 = GridToScreen(new Vector3(shipCenter.x - 5f, shipCenter.y + 2f));
		p2 = GridToScreen(new Vector3(shipCenter.x - 5f, shipCenter.y - 2f));
		p3 = GridToScreen(new Vector3(shipCenter.x + 5f, shipCenter.y));

		shipPoints[0] = p1;
		shipPoints[1] = p2;
		shipPoints[2] = p3;
	}

	private void Update()
	{
		point = Input.mousePosition;

		if (isDrawingOrigin)
		{
			DrawOrigin();
		}

		if (isDrawingAxis)
		{
			DrawLine(new Line(GridToScreen(new Vector3(-grid.screenSize.x, 0, 0)), GridToScreen(new Vector3(grid.screenSize.x * 2, 0, 0)), axisColor));
			DrawLine(new Line(GridToScreen(new Vector3(0, -grid.screenSize.y * 2, 0)), GridToScreen(new Vector3(0, grid.screenSize.y * 2, 0)), axisColor));
		}

		#region Spaceship

		ControlSpaceship();
		DrawSpaceShip(shipCenter, Color.cyan);

		#endregion

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

		if(Input.GetKeyDown(KeyCode.P))
		{
			SceneManager.LoadScene(0);
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

	#region Spaceship

	public void DrawSpaceShip(Vector3 center, Color color)
	{
		spaceship.Lines.Clear();

		shipPoints[0] = GridToScreen(new Vector3(shipCenter.x - 5f, shipCenter.y + 2f));
		shipPoints[1] = GridToScreen(new Vector3(shipCenter.x - 5f, shipCenter.y - 2f));
		shipPoints[2] = GridToScreen(new Vector3(shipCenter.x + 5f, shipCenter.y));

		shipPoints[0] = DrawingTools.RotatePoint(GridToScreen(shipCenter), shipRotAngle, shipPoints[0]);
		shipPoints[1] = DrawingTools.RotatePoint(GridToScreen(shipCenter), shipRotAngle, shipPoints[1]);
		shipPoints[2] = DrawingTools.RotatePoint(GridToScreen(shipCenter), shipRotAngle, shipPoints[2]);

		spaceship.Lines.Add(new Line(shipPoints[0], shipPoints[1], color));
		spaceship.Lines.Add(new Line(shipPoints[1], shipPoints[2], color));
		spaceship.Lines.Add(new Line(shipPoints[2], shipPoints[0], color));

		spaceship.Draw();
	}

	public void ControlSpaceship()
	{
		Vector3 force = Vector3.zero;

		if (Input.GetKey(KeyCode.A))
		{
			shipPoints[0] = DrawingTools.RotatePoint(GridToScreen(shipCenter), shipRotSpeed, shipPoints[0]);
			shipPoints[1] = DrawingTools.RotatePoint(GridToScreen(shipCenter), shipRotSpeed, shipPoints[1]);
			shipPoints[2] = DrawingTools.RotatePoint(GridToScreen(shipCenter), shipRotSpeed, shipPoints[2]);

			shipRotAngle += shipRotSpeed;
		}

		if (Input.GetKey(KeyCode.D))
		{
			shipPoints[0] = DrawingTools.RotatePoint(GridToScreen(shipCenter), -shipRotSpeed, shipPoints[0]);
			shipPoints[1] = DrawingTools.RotatePoint(GridToScreen(shipCenter), -shipRotSpeed, shipPoints[1]);
			shipPoints[2] = DrawingTools.RotatePoint(GridToScreen(shipCenter), -shipRotSpeed, shipPoints[2]);

			shipRotAngle -= shipRotSpeed;
		}

		if(Input.GetKey(KeyCode.W))
		{
			force.y = shipMoveSpeed * Mathf.Sin(shipRotAngle * Mathf.Deg2Rad);
			force.x = shipMoveSpeed * Mathf.Cos(shipRotAngle * Mathf.Deg2Rad);
		}

		Vector3 acc = force / shipMass;

		testForce = acc;

		if(shipVelocity.magnitude <= (shipMaxMoveSpeed * Time.deltaTime))
		{
			shipVelocity += acc * Time.deltaTime;
		}

		shipCenter += (shipVelocity);
	}

	#endregion
}
