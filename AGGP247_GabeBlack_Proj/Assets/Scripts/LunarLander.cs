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

	public TMP_Text fuelField, xVelField, yVelField, restartText;

	public Color axisColor = Color.white;
	public Color lineColor = Color.gray;
	public Color divisionColor = Color.yellow;

	public Vector3 point;
	public Vector3 shipCenter;
	public Vector3 shipVelocity;
	public Vector3 testForce;

	public float rotCheckZone = 5.0f;
	public float maxXVel = 20.0f;
	public float maxYVel = 50.0f;

	public int numGround = 100;
	public int groundStart = -100;
	public int groundVariation = 1;

	public float groundSegmentLength = 10;

	public float shipRotSpeed = 0.5f;
	public float shipRotAngle;
	public float shipMoveSpeed = 10f;
	public float shipMass = 40f;
	public float shipMaxMoveSpeed = 10;
	public float gravity = -9.81f;

	public float fuel = 1000f;

	public float X_VEL, Y_VEL = 0;

	public List<Vector3> groundVect = new List<Vector3>();
	public List<Color> groundColor = new List<Color>();
	public List<bool> groundValid = new List<bool>();

	DrawingObjects spaceship = new DrawingObjects();
	DrawingObjects shipHitBox = new DrawingObjects();

	Vector3[] verts = new Vector3[4];
	Vector3[] shipPoints = new Vector3[7];
	Vector3[] shipHB = new Vector3[4];

	Grid2D grid = new Grid2D();

	bool terrainSet = false;
	bool canControl = true;

	private void Start()
	{
		grid.origin = new Vector3((Screen.width / 2), (Screen.height / 2), 0);
		grid.screenSize = new Vector3((Screen.width), (Screen.height), 0);

		fuelField.text = fuel.ToString();

		shipCenter = new Vector3(0, 40);

		Vector3 p1, p2, p3, p4, p5, p6, p7 = new Vector3();

		p1 = GridToScreen(new Vector3(shipCenter.x - 2.5f, shipCenter.y + 1f));
		p2 = GridToScreen(new Vector3(shipCenter.x - 2.5f, shipCenter.y - 1f));
		p3 = GridToScreen(new Vector3(shipCenter.x + 2.5f, shipCenter.y + 1f));
		p4 = GridToScreen(new Vector3(shipCenter.x + 2.5f, shipCenter.y - 1f));
		p5 = GridToScreen(new Vector3(shipCenter.x + 4f, shipCenter.y));
		p6 = GridToScreen(new Vector3(shipCenter.x - 4f, shipCenter.y + 1.5f));
		p7 = GridToScreen(new Vector3(shipCenter.x - 4f, shipCenter.y - 1.5f));

		shipPoints[0] = p1;
		shipPoints[1] = p2;
		shipPoints[2] = p3;
		shipPoints[3] = p4;
		shipPoints[4] = p5;
		shipPoints[5] = p6;
		shipPoints[6] = p7;

		Vector3 b1, b2, b3, b4 = new Vector3();

		b1 = GridToScreen(new Vector3(shipCenter.x - 3.8f, shipCenter.y + 1.35f));
		b2 = GridToScreen(new Vector3(shipCenter.x - 3.8f, shipCenter.y - 1.35f));
		b3 = GridToScreen(new Vector3(shipCenter.x + 3.8f, shipCenter.y - 1.35f));
		b4 = GridToScreen(new Vector3(shipCenter.x + 3.8f, shipCenter.y + 1.35f));

		shipHB[0] = b1;
		shipHB[1] = b2;
		shipHB[2] = b3;
		shipHB[3] = b4;
	}

	private void Update()
	{
		point = Input.mousePosition;
		fuelField.text = Mathf.RoundToInt(fuel).ToString();

		if(!terrainSet)
		{
			SetTerrain();
		}

		#region Spaceship

		if(canControl)
		{
			ControlSpaceship();
		}

		DrawSpaceShip(shipCenter, Color.cyan);
		GroundCollision();

		#endregion

		xVelField.text = System.Math.Round(X_VEL, 2).ToString();
		yVelField.text = System.Math.Round(Y_VEL, 2).ToString();

		DrawTerrain();

		//for (int i = 0; i + 2 <= groundVect.Count; i += 2)
		//{
		//	DrawLine(new Line(GridToScreen(groundVect[i]), GridToScreen(groundVect[i + 2]), Color.blue));
		//}
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

	public void SetTerrain()
	{
		float start = groundStart;
		float end = start + groundSegmentLength;

		float sRandY = -35f;
		float eRandY = -30f;

		bool isFirst = true;
		bool isSecond = false;

		while (groundVect.Count < numGround)
		{
			if(isFirst)
			{
				groundVect.Add(new Vector3(start, sRandY));
				groundVect.Add(new Vector3(end, eRandY));
				groundColor.Add(axisColor);
				//groundColor.Add(axisColor);
				groundValid.Add(false);
				groundValid.Add(false);


				start = end;
				end = start + groundSegmentLength;

				sRandY = -30f;
				eRandY = -30f;

				isFirst = false;
				isSecond = true;
			}
			else if(isSecond)
			{
				//groundVect.Add(new Vector3(start, sRandY));
				groundVect.Add(new Vector3(end, eRandY));

				groundColor.Add(divisionColor);
				groundColor.Add(divisionColor);
				groundValid.Add(true);
				groundValid.Add(true);

				start = end;
				end = start + groundSegmentLength;

				while(sRandY == eRandY)
				{
					sRandY += Random.Range(-groundVariation, groundVariation);
					eRandY += Random.Range(-groundVariation, groundVariation);
				}

				isSecond = false;
			}
			else
			{
				for (int i = 1; i < groundVect.Count; i++)
				{
					int rand = Random.Range(-groundVariation / 2, groundVariation);

					if (sRandY == groundVect[i].y)
					{
						sRandY = sRandY + rand;
					}

					if (eRandY == groundVect[i].y)
					{
						eRandY = eRandY + rand;
					}

					if (groundVect[i] == groundVect[i - 1])
					{
						Debug.Log(i);
						groundVect[i] = new Vector3(groundVect[i].x, groundVect[i].y + rand);
					}
				}

				if (sRandY == eRandY)
				{
					groundVect.Add(new Vector3(start, sRandY));
					groundVect.Add(new Vector3(end, eRandY));
					groundColor.Add(divisionColor);
					groundColor.Add(divisionColor);
					groundValid.Add(false);
					groundValid.Add(true);
				}
				else
				{
					groundVect.Add(new Vector3(start, sRandY));
					groundVect.Add(new Vector3(end, eRandY));
					groundColor.Add(axisColor);
					groundColor.Add(axisColor);
					groundValid.Add(false);
					groundValid.Add(false);
				}

				sRandY += Random.Range(-groundVariation, groundVariation);
				eRandY += Random.Range(-groundVariation, groundVariation);

				start = end;
				end = start + groundSegmentLength;
			}
		}

		terrainSet = true;
	}	

	public void DrawTerrain()
	{
		//DrawLine(new Line(GridToScreen(new Vector3(-grid.screenSize.x, -30, 0)), GridToScreen(new Vector3(grid.screenSize.x * 2, -30, 0)), axisColor));

		for(int i = 0; i < groundVect.Count + 1; i++)
		{
			if(i + 1 < groundVect.Count)
			{
				DrawLine(new Line(GridToScreen(groundVect[i]), GridToScreen(groundVect[i + 1]), groundColor[i]));
			}
		}
	}

	public bool isInsideTri(Vector3 triP1, Vector3 triP2, Vector3 triP3, Vector3 testPoint)
	{
		float totalArea = triArea(GridToScreen(triP1), GridToScreen(triP2), GridToScreen(triP3));

		//Debug.Log(totalArea);

		float area1 = triArea(GridToScreen(triP1), GridToScreen(triP2), testPoint);
		float area2 = triArea(GridToScreen(triP2), GridToScreen(triP3), testPoint);
		float area3 = triArea(GridToScreen(triP1), GridToScreen(triP3), testPoint);

		//Debug.Log("Total: " + totalArea);
		//Debug.Log("Sum: " + (area1 + area2 + area3));

		float sum = area1 + area2 + area3;

		if (sum == totalArea)
		{
			return true;
		}
		else if (sum == totalArea + 100)
		{
			return true;
		}
		else if (sum == totalArea - 100)
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

	#region Spaceship

	public void DrawSpaceShip(Vector3 center, Color color)
	{
		spaceship.Lines.Clear();
		shipHitBox.Lines.Clear();

		shipPoints[0] = GridToScreen(new Vector3(shipCenter.x - 2.5f, shipCenter.y + 1f));
		shipPoints[1] = GridToScreen(new Vector3(shipCenter.x - 2.5f, shipCenter.y - 1f));
		shipPoints[2] = GridToScreen(new Vector3(shipCenter.x + 2.5f, shipCenter.y + 1f));
		shipPoints[3] = GridToScreen(new Vector3(shipCenter.x + 2.5f, shipCenter.y - 1f));
		shipPoints[4] = GridToScreen(new Vector3(shipCenter.x + 4f, shipCenter.y));
		shipPoints[5] = GridToScreen(new Vector3(shipCenter.x - 4f, shipCenter.y + 1.5f));
		shipPoints[6] = GridToScreen(new Vector3(shipCenter.x - 4f, shipCenter.y - 1.5f));

		shipHB[0] = GridToScreen(new Vector3(shipCenter.x - 3.8f, shipCenter.y + 1.35f));
		shipHB[1] = GridToScreen(new Vector3(shipCenter.x - 3.8f, shipCenter.y - 1.35f));
		shipHB[2] = GridToScreen(new Vector3(shipCenter.x + 3.8f, shipCenter.y - 1.35f));
		shipHB[3] = GridToScreen(new Vector3(shipCenter.x + 3.8f, shipCenter.y + 1.35f));

		shipPoints[0] = DrawingTools.RotatePoint(GridToScreen(shipCenter), shipRotAngle, shipPoints[0]);
		shipPoints[1] = DrawingTools.RotatePoint(GridToScreen(shipCenter), shipRotAngle, shipPoints[1]);
		shipPoints[2] = DrawingTools.RotatePoint(GridToScreen(shipCenter), shipRotAngle, shipPoints[2]);
		shipPoints[3] = DrawingTools.RotatePoint(GridToScreen(shipCenter), shipRotAngle, shipPoints[3]);
		shipPoints[4] = DrawingTools.RotatePoint(GridToScreen(shipCenter), shipRotAngle, shipPoints[4]);
		shipPoints[5] = DrawingTools.RotatePoint(GridToScreen(shipCenter), shipRotAngle, shipPoints[5]);
		shipPoints[6] = DrawingTools.RotatePoint(GridToScreen(shipCenter), shipRotAngle, shipPoints[6]);

		shipHB[0] = DrawingTools.RotatePoint(GridToScreen(shipCenter), shipRotAngle, shipHB[0]);
		shipHB[1] = DrawingTools.RotatePoint(GridToScreen(shipCenter), shipRotAngle, shipHB[1]);
		shipHB[2] = DrawingTools.RotatePoint(GridToScreen(shipCenter), shipRotAngle, shipHB[2]);
		shipHB[3] = DrawingTools.RotatePoint(GridToScreen(shipCenter), shipRotAngle, shipHB[3]);

		spaceship.Lines.Add(new Line(shipPoints[0], shipPoints[1], color));
		spaceship.Lines.Add(new Line(shipPoints[1], shipPoints[3], color));
		spaceship.Lines.Add(new Line(shipPoints[3], shipPoints[2], color));
		spaceship.Lines.Add(new Line(shipPoints[2], shipPoints[0], color));
		spaceship.Lines.Add(new Line(shipPoints[3], shipPoints[4], color));
		spaceship.Lines.Add(new Line(shipPoints[2], shipPoints[4], color));
		spaceship.Lines.Add(new Line(shipPoints[0], shipPoints[5], color));
		spaceship.Lines.Add(new Line(shipPoints[1], shipPoints[6], color));
		spaceship.Lines.Add(new Line(shipPoints[5], shipPoints[6], color));

		shipHitBox.Lines.Add(new Line(shipHB[0], shipHB[1], Color.red));
		shipHitBox.Lines.Add(new Line(shipHB[1], shipHB[2], Color.red));
		shipHitBox.Lines.Add(new Line(shipHB[2], shipHB[3], Color.red));
		shipHitBox.Lines.Add(new Line(shipHB[3], shipHB[0], Color.red));

		spaceship.Draw();
		//shipHitBox.Draw();
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

			fuel -= 0.1f;
		}

		Vector3 acc = (force + grav) / shipMass;

		testForce = acc;

		if (shipVelocity.magnitude <= (shipMaxMoveSpeed * Time.deltaTime))
		{
			shipVelocity += acc * Time.deltaTime;
		}

		X_VEL = shipVelocity.x * 10000f;
		Y_VEL = shipVelocity.y * 10000f;

		shipCenter += (shipVelocity);
	}

	public void GroundCollision()
	{
		bool isSafe = false;
		bool isCrash = false;

		for(int i = 0; i < groundVect.Count; i+=2)
		{
			if(i >= 2)
			{
				if((isInsideTri(groundVect[i], groundVect[i - 1], groundVect[i - 2], shipHB[0])) || (isInsideTri(groundVect[i], groundVect[i - 1], groundVect[i - 2], shipHB[1]))
				   || (isInsideTri(groundVect[i], groundVect[i - 1], groundVect[i - 2], shipHB[2])) || (isInsideTri(groundVect[i], groundVect[i - 1], groundVect[i - 2], shipHB[3])))
				{
					float rot = shipRotAngle % 90;

					Debug.Log(Mathf.Abs(rot));

					if (groundColor[i] == divisionColor && (Mathf.Abs(rot) <= 90 + rotCheckZone || Mathf.Abs(rot) >= 90 - rotCheckZone) && X_VEL >= -maxXVel && Y_VEL >= -maxYVel && X_VEL <= maxXVel && Y_VEL <= maxYVel)
					{
						Debug.Log("Safe Landing Area");
						isSafe = true;
					}
					else
					{
						Debug.Log("Crash");
						isCrash = true;
					}

					if(isSafe)
					{
						restartText.text = "You made a safe landing!!\nPress 'Space' to go again!\nPress 'Q' to go quit";
					}

					if(isCrash)
					{
						restartText.text = "You crashed...\nPress 'Space' to go again!\nPress 'Q' to go quit";
					}

					shipVelocity = Vector3.zero;
					canControl = false;
					restartText.gameObject.SetActive(true);

					if (Input.GetKeyDown(KeyCode.Space))
					{
						canControl = true;
						restartText.gameObject.SetActive(false);
						SceneManager.LoadScene(2);
					}

					if (Input.GetKeyDown(KeyCode.Q))
					{
						canControl = true;
						restartText.gameObject.SetActive(false);
						SceneManager.LoadScene(0);
					}
				}
			}
		}
	}

	#endregion
}
