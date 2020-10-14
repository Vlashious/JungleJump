using Godot;
using System;

public class Enemy : KinematicBody2D
{
    [Export] private int _speed;
    [Export] private int _gravity;
    private Vector2 _vel = new Vector2();
    private int _facing = 1;
    public override void _PhysicsProcess(float delta)
    {
        GetNode<Sprite>("Sprite").FlipH = _vel.x > 0;
        _vel.y += _gravity * delta;
        _vel.x = _facing * _speed;

        _vel = MoveAndSlide(_vel, Vector2.Up);
        for (int i = 0; i < GetSlideCount(); i++)
        {
            var collision = GetSlideCollision(i);
            if ((string)collision.Collider.Get("name") == "Player")
            {
                var player = collision.Collider as Player;
                player.Hurt();
            }
            if (collision.Normal.x != 0)
            {
                _facing = Math.Sign(collision.Normal.x);
                _vel.y = -100;
            }
        }
        if (Position.y > 1000)
        {
            QueueFree();
        }
    }

    public void TakeDamage()
    {
        GetNode<AnimationPlayer>("AnimationPlayer").Play("death");
        GetNode<AnimationPlayer>("AnimationPlayer").Connect("animation_finished", this, "OnAnimFinished");
        GetNode<CollisionShape2D>("CollisionShape2D").Disabled = true;
        SetPhysicsProcess(false);
    }

    private void OnAnimFinished(string name)
    {
        if (name == "death") QueueFree();
    }
}
