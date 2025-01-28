using System;
using System.ComponentModel.DataAnnotations.Schema;
using Godot;

public partial class Main : Node
{

	#region Member Variables

	private int _score;

	/// <summary>
	/// Holds the size of the screen after the node is loaded into the scene
	/// </summary>
	private Vector2 _screensize = Vector2.Zero;

	private Hud _hud;

	#endregion

	#region Signals

	[Signal]
	public delegate void TimeoutEventHandler();

	#endregion

	#region Exports

	[Export]
	public PackedScene MobScene { get; set; }

	#endregion

	#region Constructors

	public Main()
	{

	}

	#endregion

	#region Override Methods

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//NewGame();
		_hud = GetNode<Hud>("HUD");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	#endregion

	#region Methods

	private void GameOver()
	{
		// stop the timers
		GetNode<Timer>("MobTimer").Stop();
		GetNode<Timer>("ScoreTimer").Stop();

		// update the hud
		GetNode<Hud>("HUD").ShowGameOver();
	}

	private void NewGame()
	{
		// update the hud
		_hud.UpdateScore(_score);
		_hud.ShowMessage("Get Ready!");

		// setup the player
		Player player = GetNode<Player>("Player");
		Marker2D startPosition = GetNode<Marker2D>("StartPosition");
		player.Start(startPosition.Position);

		GetNode<CollisionPolygon2D>("Player/CollisionPolygon2D").SetDeferred(CollisionPolygon2D.PropertyName.Disabled, false);
		player.Rotation = Mathf.Tau;

		player.Show();

		_score = 0;
		_screensize = player.GetViewportRect().Size;

		GetNode<Timer>("StartTimer").Start();
	}

	private void SpawnWWDs()
	{
		// create a "wrong way driver"
		Mob wwd = MobScene.Instantiate<Mob>();

		// set the mobs spawn position to a random location on the x axis.
		PathFollow2D wwdSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
		wwdSpawnLocation.ProgressRatio = GD.Randf();
		wwd.Position = new Vector2(
			x: GD.RandRange(128, (int)_screensize.X - 128),
			y: 0
		);

		// set a direction for the mob sprite to face
		wwd.Rotation = Mathf.Tau / 2;

		// choose the velocity
		Vector2 velocity = new Vector2((float)GD.RandRange(450.0, 800.0), 0);
		wwd.LinearVelocity = velocity.Rotated(Mathf.Tau / 4);	// force the mob to go downwards

		// spawn the wwd by adding it to the Main scene.
		AddChild(wwd);
	}

	#endregion

	#region On Methods

	private void OnScoreTimerTimeout()
	{
		_score++;
		_hud.UpdateScore(_score);
		GD.Print($"Score:{_score}");
	}

	private void OnStartTimerTimeout()
	{
		GetNode<Timer>("MobTimer").Start();
		GetNode<Timer>("ScoreTimer").Start();

		GD.Print("start timer");
	}

	private void OnMobTimerTimeout()
	{
		SpawnWWDs();
	}

	#endregion
}
