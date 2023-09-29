using Godot;
using System;
using MultiplayerDemo.Extensions;

public partial class Player : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;
	
	private PlayerInput Input => GetNode<PlayerInput>("PlayerInput");

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	private long _internalId = 1;
	
	[Export]
	public long Id
	{
		set
		{
			_internalId = value;
			CallDeferred(nameof(UpdateAuthority));
		}
		get => _internalId;
	}

	private Label3D PlayerName;
	private Camera3D Camera3D;

	public override void _Ready()
	{
		Camera3D = GetNode<Camera3D>("Camera3D");
		PlayerName = GetNode<Label3D>("PlayerName");
		
		PlayerName.Text = $"Player {Id}";

		if (Id == Multiplayer.GetUniqueId())
		{
			Camera3D.Current = true;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;
		Vector3 rotation = Rotation;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y -= gravity * (float)delta;

		// Handle Jump.
		if (Input.Jumping && IsOnFloor())
			velocity.Y = JumpVelocity;

		Input.Jumping = false;

		Vector3 inputDir = Input.Direction;
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		if (velocity.Length() > 0)
		{
			var directionRotation = new Vector2(velocity.Z, velocity.X).Angle();
			rotation.Y = Mathf.LerpAngle(Rotation.Y, directionRotation, (float)delta * 10);
		}
		

		Rotation = rotation;
		Velocity = velocity;
		MoveAndSlide();
	}
	
	private void UpdateAuthority()
	{
		this.Logger().Info($"Updating authority for player {Id}");
		Input.SetMultiplayerAuthority((int) Id);
		Input.UpdateAuthority();
	}
}
