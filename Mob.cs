using Godot;
using System;

public partial class Mob : RigidBody2D
{

	#region Member Variables

	private AnimatedSprite2D animatedSprite2D;

	#endregion

	#region Signals

	[Signal]
	private delegate void ScreenExitedEventHandler();

	#endregion

	#region Constructors

	public Mob()
	{
		
	}

	#endregion

	#region Override Methods

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// setup the animated sprite and start playing it.
		animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animatedSprite2D.Animation = "drive";

		animatedSprite2D.Play();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	#endregion

	#region Methods

	private void OnVisibleOnScreenNotifier2DScreenExited()
	{
		QueueFree();
	}

	#endregion
}
