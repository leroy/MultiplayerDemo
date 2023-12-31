using Godot;
using System.Diagnostics;

public partial class Game : Node3D
{
	const float SpawnRandom = 5.0f;
	private Node Players => GetNode<Node>("Players");
	public override void _Ready()
	{
		Trace.WriteLine("Level loaded");
		if (!Multiplayer.IsServer()) return;

		Multiplayer.PeerConnected += AddPlayer;
		Multiplayer.PeerDisconnected += RemovePlayer;

		SpawnConnectedPlayers();

		SpawnLocalPlayer();
	}

	public override void _ExitTree()
	{
		if (!Multiplayer.IsServer())
			return;

		Multiplayer.PeerConnected -= AddPlayer;
		Multiplayer.PeerDisconnected -= RemovePlayer;
	}

	private void SpawnConnectedPlayers()
	{
		foreach (var id in Multiplayer.GetPeers())
			AddPlayer(id);
	}

	private void SpawnLocalPlayer()
	{
		if (!OS.HasFeature("dedicated_server"))
			AddPlayer(1);
	}

	private void AddPlayer(long id)
	{
		var packedScene = (ResourceLoader.Load("res://src/Nodes/Player.tscn") as PackedScene);
		var character = (Player)packedScene?.Instantiate();
		if (character == null)
		{
			OS.Alert("Cannot instantiate player.");
			return;
		}
		Trace.WriteLine($"Adding player {id}");
		character.Id = id;
		
		
		var position = new Vector3(GD.Randf() * SpawnRandom, 1, GD.Randf() * SpawnRandom);
		
		character.Position = position;
		character.Name = GetNameFromId(id);
		Players.AddChild(character, true);

	}

	private static string GetNameFromId(long id) => $"{id}";

	private void RemovePlayer(long id)
	{
		if (!Players.HasNode(GetNameFromId(id))) return;
		Players.GetNode(GetNameFromId(id)).QueueFree();
	}
}
