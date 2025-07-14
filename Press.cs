using Godot;
using System;

public partial class Press : Node3D
{
	[Export] public AnimationPlayer animationPress;
	[Export] public CharacterBody3d player;

	private bool isReady = true;
    public override void _Ready()
	{
		animationPress.AnimationFinished += OnAnimationFinished;
    }
	public void OnPressButton()
	{
		if (isReady) return;// If not ready, do nothing
        {
			isReady = false;
			animationPress.Play("press_move");
			player.AddMoney(1); // Example: Add 1 money when the button is pressed
		}
    }
	private void OnAnimationFinished(StringName anim)
	{
		if (anim == "press_move")
		{
			isReady = true; // Reset the ready state when the animation finishes
		}
		
    }

    public override void _Process(double delta)
	{
	}
}
