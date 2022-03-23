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

	public bool isDrawingOrigin = true;
	public bool isDrawingAxis = true;

	public float shipVelocity = 0;

	DrawingObjects spaceship = new DrawingObjects();

	Vector3[] verts = new Vector3[4];

	Grid2D grid = new Grid2D();

	private void Start()
	{
		grid.origin = new Vector3((Screen.width / 2), (Screen.height / 2), 0);
		grid.screenSize = new Vector3((Screen.width), (Screen.height), 0);
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
		DrawSpaceShip(new Vector3(0, 0), Color.cyan);

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
		if (Input.GetKeyDown(KeyCode.W))
		{
			shipVelocity += 1f;
		}
	}

	#endregion
}
