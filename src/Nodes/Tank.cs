using Godot;
using System;

public partial class Tank : CharacterBody3D
{
	private float _gravity = (float)ProjectSettings.GetSetting("physics/3d/default_gravity");

	public override void _Process(double delta)
	{
		Vector3 velocity = Velocity;
		Vector3 direction = Rotation;

		Vector2 input = new Vector2();

		input.Y = Input.GetAxis("move_down", "move_up");
		input.X = Input.GetAxis("move_left", "move_right");

		direction.Y -= input.X * 0.005f;

		velocity.X = Mathf.Sin(direction.Y + Mathf.DegToRad(90)) * input.Y * 10;
		velocity.Z = Mathf.Cos(direction.Y + Mathf.DegToRad(90)) * input.Y * 10;


		velocity.Y -= _gravity * (float) delta;

		Rotation = direction;
		Velocity = velocity;
		MoveAndSlide();
	}
}
