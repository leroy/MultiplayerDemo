using Godot;
using System;

public partial class App : Node
{
	private Control Menu;
	
	public override void _Ready()
	{
		Multiplayer.Set("server_relay", false);
		
		Menu = GetNode<Control>("Menu");
	}

	private void _on_host_pressed()
	{
		var client = MakeClient();
		client.Host = true;

		AddChild(client);
		
		Menu.QueueFree();
	}


	private void _on_client_pressed()
	{
		var client = MakeClient();
		client.Host = false;
		
		AddChild(client);
		
		Menu.QueueFree();
	}

	private Client MakeClient()
	{
		return GD.Load<PackedScene>("res://src/Scenes/Client.tscn").Instantiate<Client>();
	}
}


