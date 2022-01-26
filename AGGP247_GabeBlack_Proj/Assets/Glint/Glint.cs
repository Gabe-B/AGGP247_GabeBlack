using System.Collections.Generic;
using UnityEngine;
using Drawing.Glint;

[AddComponentMenu("Drawing/Glint")]
public class Glint : MonoBehaviour
{
	public static Glint self;

	Queue<GLCommand> commands = new Queue<GLCommand>();
	Material mat;

	private void Awake()
	{
		if (self == null)
		{
			DontDestroyOnLoad(gameObject);
			self = this;
		}
		else
		{
			Destroy(gameObject);
			return;
		}

		mat = new Material(Shader.Find("Hidden/Internal-Colored"));
	}

	private void OnPostRender()
	{
		if (commands.Count > 0)
		{
			GL.PushMatrix();
			mat.SetPass(0);
			GL.LoadPixelMatrix();

			GLCommand cmd;

			while (commands.Count > 0)
			{
				cmd = commands.Dequeue();

				GL.Begin((int)cmd.mode);
				GL.Color(cmd.color);
				for (int i = 0; i < cmd.verts.Length; i++)
				{
					GL.Vertex(cmd.verts[i]);
				}
				GL.End();
			}
			GL.PopMatrix();
		}
	}

	public static void AddCommand(GLCommand command)
	{
		self.commands.Enqueue(command);
	}

	public static void AddCommand(ICommandInstruction instruction)
	{
		self.commands.Enqueue(instruction.ToCommand());
	}
}
