using Godot;
using System;

public partial class CharacterBody3d : CharacterBody3D
{
	[Export] private Camera3D camera; // Reference to the camera node
    [Export]  public  float Speed = 5.0f; 
    [Export]  public  float JumpVelocity = 4.5f;
    [Export]  public  float mouseSensitivity = 1.0f; 

    private float rotation_x = 0.0f;
    public override void _Ready()
    {
        base._Ready();
        GD.Print("CharacterBody3d is ready.");
       // Input.MouseMode = Input.MouseModeEnum.Captured; // Capture the mouse for free look

    }
    public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;


		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		if (Input.IsActionJustPressed("esc"))
		{
			GetTree().Quit();
		}

		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		Vector2 inputDir = Input.GetVector("left", "right", "forward", "back");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

		if (Input.IsActionPressed("run"))
		{
			direction *= 2; // Double the speed when running
			GD.Print("Running: " + direction);
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

	public override void _UnhandledInput(InputEvent @event)//_UnhandleadInput
	{
		GD.Print("Unh input detected");
        if (@event is InputEventMouseMotion motionEvent)
		{
			RotateY(Mathf.DegToRad(-motionEvent.Relative.X * mouseSensitivity));

			rotation_x -= motionEvent.Relative.Y * mouseSensitivity;
			rotation_x = Mathf.Clamp(rotation_x, -60, 60);
			camera.RotationDegrees = new Vector3(rotation_x,0 ,0 );//camera.RotationDegrees.Y, camera.RotationDegrees.Z

			GD.Print($"Camera rotation:X={motionEvent.Relative.Y}\n Y={motionEvent.Relative.X} " );
            if (motionEvent.Relative.X != 0 || motionEvent.Relative.Y != 0)
            {
                GD.Print("Mouse is moving, camera rotation should update!");
            }
        }

	}

}
