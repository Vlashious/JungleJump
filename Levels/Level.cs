using Godot;
using System;

public class Level : Node2D
{
    private TileMap _pickups;
    public override void _Ready()
    {
        _pickups = GetNode<TileMap>("Pickups");
        _pickups.Hide();
        GetNode<Player>("Player").Start(GetNode<Position2D>("PlayerSpawn").Position);
    }
}
