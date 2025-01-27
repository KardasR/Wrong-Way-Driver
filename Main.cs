using Godot;

public partial class Main : Node
{

	#region Member Variables

	private int _score;

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
		NewGame();
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
	}

	private void NewGame()
	{
		_score = 0;

		// move the player to the starting position.
		Player player = GetNode<Player>("Player");
		Marker2D startPosition = GetNode<Marker2D>("StartPosition");
		player.Start(startPosition.Position);

		GetNode<Timer>("StartTimer").Start();
	}

	private void OnScoreTimerTimeout()
	{
		_score++;
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
		// create a "wrong way driver"
		Mob wwd = MobScene.Instantiate<Mob>();

		// choose a random location on the path
		PathFollow2D wwdSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
		wwdSpawnLocation.ProgressRatio = GD.Randf();

		// set the wrong way drivers direction perpendicular to the path direction.
		float direction = wwdSpawnLocation.Rotation + Mathf.Pi / 2;

		// set the mobs position to a random location.
		wwd.Position = wwdSpawnLocation.Position;

		// add some randomness to the direction.
		direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
		wwd.Rotation = direction;

		// choose the velocity
		Vector2 velocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0);
		wwd.LinearVelocity = velocity.Rotated(direction);

		// spawn the wwd by adding it to the Main scene.
		AddChild(wwd);
	}

	#endregion
}
