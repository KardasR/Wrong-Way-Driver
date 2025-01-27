using Godot;
using System;

public partial class Main : Node
{

	#region Member Variables

	private int _score;
	
	#endregion

	#region Signals

	#endregion

	#region Exports

	[Export]
	public PackedScene MobScene { get; set; }

	#endregion

	#region Constructors

	#endregion

	#region Override Methods

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	#endregion

	#region Methods

	#endregion
}
