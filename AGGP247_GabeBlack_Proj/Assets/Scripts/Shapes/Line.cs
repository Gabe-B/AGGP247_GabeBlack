using UnityEngine;
using Drawing.Glint;

public struct Line : ICommandInstruction
{
	public Vector3 start, end;
	public Color color;

	public Line(Vector3 start, Vector3 end, Color color)
	{
		this.start = start;
		this.end = end;
		this.color = color;
	}

	public GLCommand ToCommand()
	{
		return new GLCommand(DrawMode.Lines, color, start, end);
	}
}