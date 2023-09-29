using System;
using Godot;

namespace MultiplayerDemo.Util;

public partial class Logger : Node
{
    private string format(string[] message)
    {
        var dateTime = DateTime.Now.ToString("HH:mm:ss");

        var isServer = Multiplayer.IsServer();
        var instance = isServer ? "Server" : "Client";

        return $"[{dateTime}][{instance}] {message.Join(" ")}";
    }
    
    
    public void Info(params string[] message)
    {
        GD.Print(format(message));;
    }
    
}