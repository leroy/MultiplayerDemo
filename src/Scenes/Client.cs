using Godot;
using System;

public partial class Client : Node
{
	[Export]
	public bool Host;
	
	public override void _Ready()
	{
		Connect();
		StartGame();
	}

	private void Connect()
	{
		var peer = new ENetMultiplayerPeer();

		if (Host)
		{
			var error = peer.CreateServer(1337);

			if (error != Error.Ok)
			{
				GD.PrintErr(error);
			}
		}
		else
		{
			var error = peer.CreateClient("127.0.0.1", 1337);

			if (error != Error.Ok)
			{
				GD.PrintErr(error);
			}
		}
		
		Multiplayer.MultiplayerPeer = peer;
	}

	private void StartGame()
	{
		if (Multiplayer.IsServer())
		{
			ChangeLevel(GD.Load("res://src/Scenes/Game.tscn") as PackedScene);
		}
	}

	private void ChangeLevel(PackedScene scene)
	{
		var level = GetNode<Node>("Level");

		foreach (var node in level.GetChildren())
		{
			level.RemoveChild(node);
			node.QueueFree();
		}
		
		level.AddChild(scene.Instantiate(), true);
	}
}
