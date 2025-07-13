using Godot;
using System;

public partial class World : Node3D
{
	[Export] public PlayerChar player;
	[Export] public Press press;
	[Export] public MoneyStatusBar moneyStatusBar;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}


