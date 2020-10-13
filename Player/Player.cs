using Godot;
using System;

public class Player : KinematicBody2D
{
    [Signal] private delegate void LifeChanged(int life);
    [Signal] private delegate void Dead();
    [Export] private readonly int _runSpeed;
    [Export] private readonly int _jumpSpeed;
    [Export] private readonly int _gravity;
    private enum STATE
    {
        IDLE,
        RUN,
        JUMP,
        HURT,
        DEAD
    }
    private STATE _state;
    private string _newAnim;
    private string _anim;
    private Vector2 _velocity = new Vector2();
    private int _lives;

    public override void _Ready()
    {
        ChangeState(STATE.IDLE);
    }

    public override void _PhysicsProcess(float delta)
    {
        _velocity.y += _gravity * delta;
        GetInput();
        if (_newAnim != _anim)
        {
            _anim = _newAnim;
            GetNode<AnimationPlayer>("AnimationPlayer").Play(_anim);
        }
        _velocity = MoveAndSlide(_velocity, Vector2.Up);
    }

    public void Start(Vector2 pos)
    {
        Position = pos;
        Show();
        ChangeState(STATE.IDLE);
        _lives = 3;
        EmitSignal("LifeChanged", _lives);
    }

    private void Hurt()
    {
        if (_state != STATE.HURT)
        {
            ChangeState(STATE.HURT);
        }
    }

    private void GetInput()
    {
        if (_state == STATE.HURT) return;
        var right = Input.IsActionPressed("right");
        var left = Input.IsActionPressed("left");
        var jump = Input.IsActionJustPressed("jump");

        _velocity.x = 0;
        if (right)
        {
            _velocity.x += _runSpeed;
            GetNode<Sprite>("Sprite").FlipH = false;
        }
        if (left)
        {
            _velocity.x -= _runSpeed;
            GetNode<Sprite>("Sprite").FlipH = true;
        }
        if (jump && IsOnFloor())
        {
            ChangeState(STATE.JUMP);
            _velocity.y = -_jumpSpeed;
        }
        if (_state == STATE.IDLE && _velocity.x != 0)
        {
            ChangeState(STATE.RUN);
        }
        if (_state == STATE.RUN && _velocity.x == 0)
        {
            ChangeState(STATE.IDLE);
        }
        if (_state == STATE.IDLE || _state == STATE.RUN && !IsOnFloor())
        {
            ChangeState(STATE.JUMP);
        }
        if (_state == STATE.JUMP && IsOnFloor())
        {
            ChangeState(STATE.IDLE);
        }
        if (_state == STATE.JUMP && _velocity.y > 0) _newAnim = "jump_down";
    }

    private async void ChangeState(STATE newState)
    {
        _state = newState;
        switch (_state)
        {
            case STATE.IDLE:
                _newAnim = "idle";
                break;
            case STATE.RUN:
                _newAnim = "run";
                break;
            case STATE.HURT:
                _newAnim = "hurt";
                _velocity.x = -100 * Math.Sign(_velocity.x);
                _velocity.y = -200;
                _lives -= 1;
                EmitSignal("LifeChanged", _lives);
                await ToSignal(GetTree().CreateTimer(0.5f), "timeout");
                ChangeState(STATE.IDLE);
                if (_lives <= 0)
                {
                    ChangeState(STATE.DEAD);
                }
                break;
            case STATE.JUMP:
                _newAnim = "jump_up";
                break;
            case STATE.DEAD:
                EmitSignal("Dead");
                Hide();
                break;
        }
    }
}
