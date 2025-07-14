using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

public partial class CharacterBody3d : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;
	public int money { get; private set; } = 0;
    public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;
		

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("left", "right", "forward", "back");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (Input.IsActionPressed("run"))
		{
			direction *= 2; // Double the speed when running
			Console.WriteLine("Running: " + direction);
        }
        if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
	public override void _Ready()
	{
		// Initialize any necessary properties or states here.
		base._Ready();
		GD.Print("CharacterBody3d is ready.");
    }
    public override void _UnhandledInput(InputEvent @event)
    {

        base._UnhandledInput(@event);
    }
	public void AddMoney(int amount)
	{
		money += amount;
		EmitSignal("MoneyChanged", money);
    }
	public int GetMoney()
	{
		return money;
    }
	[Signal] public delegate void MoneyChangedEventHandler(int newAmount);
    [Signal] public delegate void money_updatedEventHandler(int money);//signal to notify when money is updated
}
