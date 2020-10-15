using Godot;
using System;

public class HUD : MarginContainer
{
    private TextureRect[] _lifeCounters;

    public override void _Ready()
    {
        _lifeCounters = new TextureRect[]
        {
            GetNode<TextureRect>("HBoxContainer/LifeCounter/L1"),
            GetNode<TextureRect>("HBoxContainer/LifeCounter/L2"),
            GetNode<TextureRect>("HBoxContainer/LifeCounter/L3"),
            GetNode<TextureRect>("HBoxContainer/LifeCounter/L4"),
            GetNode<TextureRect>("HBoxContainer/LifeCounter/L5")
        };
    }

    private void OnPlayerLifeChanged(int value)
    {
        for (int i = 0; i < _lifeCounters.Length; i++)
        {
            _lifeCounters[i].Visible = value > i;
        }

    }

    private void OnScoreChanged(int value)
    {
        GetNode<Label>("HBoxContainer/ScoreLabel").Text = value.ToString();
    }
}
