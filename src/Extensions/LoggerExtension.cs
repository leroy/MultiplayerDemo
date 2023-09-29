using System;
using Godot;
using MultiplayerDemo.Util;

namespace MultiplayerDemo.Extensions;

public static class LoggerExtension
{
    public static Logger Logger(this Node node)
    {
        return node.GetNode<Logger>("/root/Logger");
    }
}