using Godot;
using System;

public partial class MoneyStatusBar : Control
{
	// Called when the node enters the scene tree for the first time.
	[Export] public Label moneyLabel;
	[Export] public PlayerChar player;
	[Export] public Button pressButton;
    public override void _Ready()
	{
		moneyLabel = GetNode<Label>("MoneyLabel");
		pressButton = GetNode<Button>("PressButton");
		player = GetNode<PlayerChar>("../PlayerChar"); // Adjust the path as necessary to find PlayerChar
		player.MoneyChanged += OnMoneyChanged; // Subscribe to the money changed signal
		pressButton.Pressed += OnPressButtonPressed; // Connect the button pressed signal
		OnMoneyChanged(player.GetMoney()); // Initialize the money label with the current amount

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
	public void OnPressButtonPressed()
	{

		player.AddMoney(1); 
    }
	public void OnMoneyChanged(int newAmount)
	{
		moneyLabel.Text = $"Money: {newAmount}";
	}
}
