using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class CharacterBody3d : CharacterBody3D
{
	[Export] private Camera3D camera;
	[Export] private RayCast3D interactRay;

	[Export] public float Speed = 5.0f;
	[Export] public float JumpVelocity = 4.5f;

	[Export] public float mouseSensitivity = 1.0f;

	private float rotation_x = 0.0f;
	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;

		if (interactRay == null)
			interactRay = GetNodeOrNull<RayCast3D>("Camera3D/RayCast3D"); // підхопити за шляхом

		if (interactRay == null)
			GD.PushError("interactRay = null. Прив’яжи RayCast3D в інспекторі!");

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
		}
        if (Input.IsActionJustPressed("interact_action"))
        {
            TryInteract();
            GD.Print("Interact action pressed.");//checker
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
		
		if (@event is InputEventMouseMotion motionEvent)
		{
			RotateY(Mathf.DegToRad(-motionEvent.Relative.X * mouseSensitivity));

			rotation_x -= motionEvent.Relative.Y * mouseSensitivity;
			rotation_x = Mathf.Clamp(rotation_x, -60, 60);
			camera.RotationDegrees = new Vector3(rotation_x, 0, 0);

		}

	}
	private void TryInteract()
	{
		if (interactRay == null) { GD.Print("RayCast не прив’язаний"); return; }

		interactRay.ForceRaycastUpdate();
		if (!interactRay.IsColliding()) return;

		var hitNode = interactRay.GetCollider() as Node;
		if (hitNode == null) { GD.Print("Влучили не в Node"); return; }
		/////////////////////////////////////////////////////
		
		// Варіант через групу:
		if (hitNode.IsInGroup("press_button"))
		{
			hitNode.CallDeferred("OnPress");  // викличе метод на вузлі зі скриптом Press
			GD.Print($"{hitNode.Name} влучено, викликано OnPress через групу 'press_button'.");
            return;

		}

		// Варіант через тип:
		if (hitNode is Press p) {
			p.OnPress(); 
			return; }

		if (hitNode.GetParent() is Press pp) { 
			pp.OnPress(); 
			return; }

		GD.Print($"{hitNode.Name} влучено, але немає Press або групи 'press_button'.");

	}
}
