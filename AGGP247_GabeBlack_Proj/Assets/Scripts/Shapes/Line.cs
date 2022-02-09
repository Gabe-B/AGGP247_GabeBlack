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



    /// <summary>
    /// Draws the line instance, Is in screen space as default. Can pass a Grid2D to draw in the Grid Space.
    /// </summary>
    /// <param name="grid">Optional, Grid2D to draw relative to. (Start and End are in Grid Space when you do)</param>
    public void Draw(Grid_2D.Grid2D grid = null)
    {
        if (grid != null)
        {
            grid.DrawLine(this);
            return;
        }

        Glint.AddCommand(this);
    }

	public GLCommand ToCommand()
    {
        return new GLCommand(DrawMode.Lines, color, start, end);
    }
}