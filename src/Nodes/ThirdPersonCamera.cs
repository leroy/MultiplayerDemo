using System.Reflection;
using Godot;

namespace MultiplayerDemo.Nodes;

[GlobalClass]
public partial class ThirdPersonCamera: Camera3D
{
	[Export]
	public float Sensitivity = 0.001f;
	
	[Export]
	public float TiltLowerLimit = (float) Mathf.DegToRad(-30.0);
	
	[Export]
	public float TiltUpperLimit = (float) Mathf.DegToRad(30.0);
	
	[Export]
	public Node3D Follow;

	private Vector3 RelativePosition;

	private Node3D Target;
	
	public override void _Ready()
	{
		RelativePosition = GlobalPosition - Follow.GlobalPosition;

		CallDeferred(nameof(SetupCamera));
	}

	private void SetupCamera()
	{
		var parent = GetParent();

		Target = new Node3D();

		parent.RemoveChild(this);
		Target.AddChild(this);
		Target.GlobalPosition = Follow.GlobalPosition;

		Position = RelativePosition;
		
		parent.AddChild(Target);
	}	

	public override void _Process(double delta)
	{
		Target.GlobalPosition = Follow.GlobalPosition;
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_cancel"))
		{
			Input.MouseMode = Input.MouseModeEnum.Visible;
		}

		if (@event is InputEventMouseButton eventMouseButton && eventMouseButton.ButtonIndex == MouseButton.Left &&
			eventMouseButton.Pressed)
		{
			Input.MouseMode = Input.MouseModeEnum.Captured;
		}

		if (@event is InputEventMouseMotion eventMouseMotion && Input.MouseMode == Input.MouseModeEnum.Captured)
		{
			var rotation = Target.Rotation;
			var cameraRotation = Rotation;
			
			cameraRotation.X -= eventMouseMotion.Relative.Y * Sensitivity;
			cameraRotation.X = Mathf.Clamp(cameraRotation.X, TiltLowerLimit, TiltUpperLimit);
			rotation.Y -= eventMouseMotion.Relative.X * Sensitivity;

			
			Rotation = cameraRotation;
			Target.Rotation = rotation;
		}
	}
}
