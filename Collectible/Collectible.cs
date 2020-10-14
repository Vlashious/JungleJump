using Godot;
using System;
using System.Collections.Generic;

public class Collectible : Area2D
{
    [Signal] private delegate void Pickup();
    private readonly Dictionary<string, string> textures = new Dictionary<string, string>()
    {
        {"cherry", "res://assets/sprites/cherry.png"},
        {"gem", "res://assets/sprites/gem.png"}
    };
    public void Init(string type, Vector2 pos)
    {
        GetNode<Sprite>("Sprite").Texture = GD.Load(textures[type]) as Texture;
        Position = pos;
        Connect("body_entered", this, "OnBodyEntered");
    }
    private void OnBodyEntered(Node body)
    {
        EmitSignal("Pickup");
        QueueFree();
    }
}
