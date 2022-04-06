using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LunarLander : MonoBehaviour
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

	public TMP_Text fuelText;

	public Color axisColor = Color.white;
	public Color lineColor = Color.gray;
	public Color divisionColor = Color.yellow;

	public Vector3 point;
	public Vector3 shipCenter;
	public Vector3 shipVelocity;
	public Vector3 testForce;

	public float shipRotSpeed = 0.5f;
	public float shipRotAngle;
	public float shipMoveSpeed = 10f;
	public float shipMass = 40f;
	public float shipMaxMoveSpeed = 10;
	public float gravity = -9.81f;

	public float fuel = 1000f;

	DrawingObjects spaceship = new DrawingObjects();

	Vector3[] verts = new Vector3[4];
	Vector3[] shipPoints = new Vector3[7];

	Grid2D grid = new Grid2D();

	private void Start()
	{
		grid.origin = new Vector3((Screen.width / 2), (Screen.height / 2), 0);
		grid.screenSize = new Vector3((Screen.width), (Screen.height), 0);

		fuelText.text = fuel.ToString();

		shipCenter = new Vector3(0, 40);

		Vector3 p1, p2, p3, p4, p5, p6, p7 = new Vector3();

		p1 = GridToScreen(new Vector3(shipCenter.x - 5f, shipCenter.y + 2f));
		p2 = GridToScreen(new Vector3(shipCenter.x - 5f, shipCenter.y - 2f));
		p3 = GridToScreen(new Vector3(shipCenter.x + 5f, shipCenter.y + 2f));
		p4 = GridToScreen(new Vector3(shipCenter.x + 5f, shipCenter.y - 2f));
		p5 = GridToScreen(new Vector3(shipCenter.x + 8f, shipCenter.y));
		p6 = GridToScreen(new Vector3(shipCenter.x - 8f, shipCenter.y + 3f));
		p7 = GridToScreen(new Vector3(shipCenter.x - 8f, shipCenter.y - 3f));

		shipPoints[0] = p1;
		shipPoints[1] = p2;
		shipPoints[2] = p3;
		shipPoints[3] = p4;
		shipPoints[4] = p5;
		shipPoints[5] = p6;
		shipPoints[6] = p7;
	}

	private void Update()
	{
		point = Input.mousePosition;
		fuelText.text = Mathf.RoundToInt(fuel).ToString();

		DrawLine(new Line(GridToScreen(new Vector3(-grid.screenSize.x, -30, 0)), GridToScreen(new Vector3(grid.screenSize.x * 2, -30, 0)), axisColor));

		#region Spaceship

		ControlSpaceship();
		DrawSpaceShip(shipCenter, Color.cyan);

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

	public void DrawTerrain()
	{
		DrawLine(new Line(GridToScreen(new Vector3(-grid.screenSize.x, -30, 0)), GridToScreen(new Vector3(grid.screenSize.x * 2, -30, 0)), axisColor));

		float start = -grid.screenSize.x;
		float end = start + 10;

		while(end < (grid.screenSize.x * 2))
		{
			DrawLine(new Line(GridToScreen(new Vector3(start, -30, 0)), GridToScreen(new Vector3(end, -30, 0)), axisColor));

			start = end;
			end = start + 10;
		}
	}

	#region Spaceship

	public void DrawSpaceShip(Vector3 center, Color color)
	{
		spaceship.Lines.Clear();

		shipPoints[0] = GridToScreen(new Vector3(shipCenter.x - 5f, shipCenter.y + 2f));
		shipPoints[1] = GridToScreen(new Vector3(shipCenter.x - 5f, shipCenter.y - 2f));
		shipPoints[2] = GridToScreen(new Vector3(shipCenter.x + 5f, shipCenter.y + 2f));
		shipPoints[3] = GridToScreen(new Vector3(shipCenter.x + 5f, shipCenter.y - 2f));
		shipPoints[4] = GridToScreen(new Vector3(shipCenter.x + 8f, shipCenter.y));
		shipPoints[5] = GridToScreen(new Vector3(shipCenter.x - 8f, shipCenter.y + 3f));
		shipPoints[6] = GridToScreen(new Vector3(shipCenter.x - 8f, shipCenter.y - 3f));

		shipPoints[0] = DrawingTools.RotatePoint(GridToScreen(shipCenter), shipRotAngle, shipPoints[0]);
		shipPoints[1] = DrawingTools.RotatePoint(GridToScreen(shipCenter), shipRotAngle, shipPoints[1]);
		shipPoints[2] = DrawingTools.RotatePoint(GridToScreen(shipCenter), shipRotAngle, shipPoints[2]);
		shipPoints[3] = DrawingTools.RotatePoint(GridToScreen(shipCenter), shipRotAngle, shipPoints[3]);
		shipPoints[4] = DrawingTools.RotatePoint(GridToScreen(shipCenter), shipRotAngle, shipPoints[4]);
		shipPoints[5] = DrawingTools.RotatePoint(GridToScreen(shipCenter), shipRotAngle, shipPoints[5]);
		shipPoints[6] = DrawingTools.RotatePoint(GridToScreen(shipCenter), shipRotAngle, shipPoints[6]);

		spaceship.Lines.Add(new Line(shipPoints[0], shipPoints[1], color));
		spaceship.Lines.Add(new Line(shipPoints[1], shipPoints[3], color));
		spaceship.Lines.Add(new Line(shipPoints[3], shipPoints[2], color));
		spaceship.Lines.Add(new Line(shipPoints[2], shipPoints[0], color));
		spaceship.Lines.Add(new Line(shipPoints[3], shipPoints[4], color));
		spaceship.Lines.Add(new Line(shipPoints[2], shipPoints[4], color));
		spaceship.Lines.Add(new Line(shipPoints[0], shipPoints[5], color));
		spaceship.Lines.Add(new Line(shipPoints[1], shipPoints[6], color));
		spaceship.Lines.Add(new Line(shipPoints[5], shipPoints[6], color));

		spaceship.Draw();
	}

	public void ControlSpaceship()
	{
		Vector3 force = Vector3.zero;
		Vector3 grav = new Vector3(0, gravity);

		if (Input.GetKey(KeyCode.A))
		{
			shipPoints[0] = DrawingTools.RotatePoint(GridToScreen(shipCenter), shipRotSpeed, shipPoints[0]);
			shipPoints[1] = DrawingTools.RotatePoint(GridToScreen(shipCenter), shipRotSpeed, shipPoints[1]);
			shipPoints[2] = DrawingTools.RotatePoint(GridToScreen(shipCenter), shipRotSpeed, shipPoints[2]);
			shipPoints[3] = DrawingTools.RotatePoint(GridToScreen(shipCenter), shipRotSpeed, shipPoints[3]);
			shipPoints[4] = DrawingTools.RotatePoint(GridToScreen(shipCenter), shipRotSpeed, shipPoints[4]);
			shipPoints[5] = DrawingTools.RotatePoint(GridToScreen(shipCenter), shipRotSpeed, shipPoints[5]);
			shipPoints[6] = DrawingTools.RotatePoint(GridToScreen(shipCenter), shipRotSpeed, shipPoints[6]);

			shipRotAngle += shipRotSpeed;
		}

		if (Input.GetKey(KeyCode.D))
		{
			shipPoints[0] = DrawingTools.RotatePoint(GridToScreen(shipCenter), -shipRotSpeed, shipPoints[0]);
			shipPoints[1] = DrawingTools.RotatePoint(GridToScreen(shipCenter), -shipRotSpeed, shipPoints[1]);
			shipPoints[2] = DrawingTools.RotatePoint(GridToScreen(shipCenter), -shipRotSpeed, shipPoints[2]);
			shipPoints[3] = DrawingTools.RotatePoint(GridToScreen(shipCenter), -shipRotSpeed, shipPoints[3]);
			shipPoints[4] = DrawingTools.RotatePoint(GridToScreen(shipCenter), -shipRotSpeed, shipPoints[4]);
			shipPoints[5] = DrawingTools.RotatePoint(GridToScreen(shipCenter), -shipRotSpeed, shipPoints[5]);
			shipPoints[6] = DrawingTools.RotatePoint(GridToScreen(shipCenter), -shipRotSpeed, shipPoints[6]);

			shipRotAngle -= shipRotSpeed;
		}

		if (Input.GetKey(KeyCode.W) && fuel > 0)
		{
			force.y = shipMoveSpeed * Mathf.Sin(shipRotAngle * Mathf.Deg2Rad);
			force.x = shipMoveSpeed * Mathf.Cos(shipRotAngle * Mathf.Deg2Rad);

			fuel -= 0.01f;
		}

		Vector3 acc = (force + grav) / shipMass;

		testForce = acc;

		if (shipVelocity.magnitude <= (shipMaxMoveSpeed * Time.deltaTime))
		{
			shipVelocity += acc * Time.deltaTime;
		}

		shipCenter += (shipVelocity);
	}

	#endregion
}
