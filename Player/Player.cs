using Godot;
using System;

public class Player : KinematicBody2D
{
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

    private void ChangeState(STATE newState)
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
                break;
            case STATE.JUMP:
                _newAnim = "jump_up";
                break;
            case STATE.DEAD:
                Hide();
                break;
        }
    }
}
