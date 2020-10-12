using Godot;
using System;

public class Player : KinematicBody2D
{
    private enum STATE
    {
        IDLE,
        RUN,
        JUMP,
        HURT,
        DEAD
    }
    private STATE _state;
    private string _new_anim;
    private string _anim;

    public override void _Ready()
    {
        ChangeState(STATE.IDLE);
    }

    public override void _PhysicsProcess(float delta)
    {
        if (_new_anim != _anim)
        {
            _anim = _new_anim;
            GetNode<AnimationPlayer>("AnimationPlayer").Play(_anim);
        }
    }
    private void ChangeState(STATE newState)
    {
        _state = newState;
        switch (_state)
        {
            case STATE.IDLE:
                _new_anim = "idle";
                break;
            case STATE.RUN:
                _new_anim = "run";
                break;
            case STATE.HURT:
                _new_anim = "hurt";
                break;
            case STATE.JUMP:
                _new_anim = "jump_up";
                break;
            case STATE.DEAD:
                Hide();
                break;
        }
    }
}
