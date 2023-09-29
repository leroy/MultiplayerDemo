using Godot;
using System;

public partial class PlayerInput : MultiplayerSynchronizer
{
	[Export]
	public bool Jumping = false;

	// It seems that variables that are synchronized over the network need to be lowercase
	[Export]
	public Vector3 Direction = new();

	public override void _Ready()
	{
		UpdateAuthority();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Direction.X = Input.GetAxis("move_left", "move_right");
		Direction.Z = Input.GetAxis("move_up", "move_down");

		Direction = Direction.Rotated(Vector3.Up, GetViewport().GetCamera3D().Rotation.Y).Normalized();
		
		if (Input.IsActionPressed("jump"))
			Rpc(nameof(Jump));
	}

	[Rpc(CallLocal = true)]
	private void Jump() => Jumping = true;

	public void UpdateAuthority()
	{
		SetProcess(GetMultiplayerAuthority() == Multiplayer.GetUniqueId());
	}
}
