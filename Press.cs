using Godot;
using System;


public partial class Press : Node3D
{
    [Export] public AnimationPlayer Anim;        // перетягни сюди AnimationPlayer
    [Export] public string AnimName = "press_move";

    private bool _busy;

    public override void _Ready()
    {
        if (Anim == null)
            Anim = GetNodeOrNull<AnimationPlayer>("AnimationPlayer"); // запасний варіант

        if (Anim == null)
        {
            GD.PushError($"{Name}: AnimationPlayer не знайдено. Прив’яжи його в інспекторі або виправ шлях.");
            return;
        }

        Anim.AnimationFinished += OnAnimFinished;
    }

    public void OnPress()
    {
        if (_busy || Anim == null) return;

        _busy = true;
        Anim.Play(AnimName);
    }

    private void OnAnimFinished(StringName anim)
    {
        if (anim == AnimName)
            _busy = false;
    }


    public override void _Process(double delta)
    {
    }
}
