using Godot;
using System;

public class Level : Node2D
{
    [Signal] private delegate void ScoreChanged(int score);
    [Export] private PackedScene _collectible;
    private TileMap _pickups;
    private int _score;
    public override void _Ready()
    {
        _score = 0;
        EmitSignal("ScoreChanged", _score);
        _pickups = GetNode<TileMap>("Pickups");
        _pickups.Hide();
        GetNode<Player>("Player").Start(GetNode<Position2D>("PlayerSpawn").Position);
        SetCameraLimits();
        SpawnPickups();
    }

    private void SetCameraLimits()
    {
        var mapSize = GetNode<TileMap>("World").GetUsedRect();
        var cellSize = GetNode<TileMap>("World").CellSize;
        GetNode<Camera2D>("Player/Camera2D").LimitLeft = (int)((mapSize.Position.x - 5) * cellSize.x);
        GetNode<Camera2D>("Player/Camera2D").LimitRight = (int)((mapSize.End.x + 5) * cellSize.x);
    }

    private void SpawnPickups()
    {
        foreach (Vector2 cell in _pickups.GetUsedCells())
        {
            var id = _pickups.GetCellv(cell);
            var type = _pickups.TileSet.TileGetName(id);
            if (type is "cherry" || type is "gem")
            {
                var c = _collectible.Instance() as Collectible;
                var pos = _pickups.MapToWorld(cell);
                c.Init(type, pos + _pickups.CellSize / 2);
                AddChild(c);
                c.Connect("Pickup", this, "OnPickup");
            }
        }
    }

    private void OnPickup()
    {
        _score++;
        EmitSignal("ScoreChanged", _score);
    }

    private void OnPlayerDead()
    {
        return;
    }
}
