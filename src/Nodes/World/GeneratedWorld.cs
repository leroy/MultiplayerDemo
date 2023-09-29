using Godot;
using System;

public partial class GeneratedWorld : Node3D
{
	[Export]
	public Vector2I Size = new Vector2I(100, 100);
	
	[Export]
	public Noise Noise = new FastNoiseLite();
	
	public MeshInstance3D MeshInstance3D => GetNode<MeshInstance3D>("MeshInstance3D");
	
	public override void _Ready()
	{
		var surface = new SurfaceTool();

		surface.Begin(Mesh.PrimitiveType.Triangles);
		surface.SetColor(Colors.Red);
		
		surface.AddVertex(new Vector3(0, 0, 0));
		surface.AddVertex(new Vector3(1, 0, 0));
		surface.AddVertex(new Vector3(0, 1, 0));

		MeshInstance3D.Mesh = surface.Commit();

		MeshInstance3D.Mesh.SurfaceSetMaterial(0, new StandardMaterial3D());
		MeshInstance3D.MaterialOverride = new StandardMaterial3D();
	}
}
