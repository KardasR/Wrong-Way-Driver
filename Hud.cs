using Godot;
using System;

public partial class Hud : CanvasLayer
{

	#region Member Variables
	
	#endregion

	#region Signals

	[Signal]
	public delegate void StartGameEventHandler();

	#endregion

	#region Exports

	#endregion

	#region Constructors

	public Hud()
	{

	}

	#endregion

	#region Override Methdos

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

	public void ShowMessage(string text)
	{
		Label message = GetNode<Label>("Message");
		message.Text = text;
		message.Show();

		GetNode<Timer>("MessageTimer").Start();
	}

	async public void ShowGameOver()
	{
		ShowMessage("Game Over");

		Timer messageTimer = GetNode<Timer>("MessageTimer");
		await ToSignal(messageTimer, Timer.SignalName.Timeout);

		Label message = GetNode<Label>("Message");
		message.Text = "Dodge the wrong way drivers!";
		message.Show();

		GetNode<Timer>("MessageTimer").Start();
	}

	public void UpdateScore(int score)
	{
		GetNode<Label>("ScoreLabel").Text = score.ToString();
	}

	#endregion

	#region On Methods

	private void OnStartButtonPressed()
	{
		GetNode<Button>("StartButton").Hide();
		EmitSignal(SignalName.StartGame);
	}

	private void OnMessageTimerTimeout()
	{
		GetNode<Label>("Message").Hide();
	}

	#endregion
}
