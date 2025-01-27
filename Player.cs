using Godot;
using System;

public partial class Player : Area2D
{

	#region Signals
	
	[Signal]
	public delegate void HitEventHandler();

	#endregion

	#region Member Variables

	/// <summary>
	/// <para>1+	=	multiply base speed by the gear</para>
	/// <para>0		=	park</para>
	/// <para>-1	=	reverse</para>
	/// </summary>
	private int _gear = 0;

	/// <summary>
	/// Base speed
	/// </summary>
	private int _speed = 100;

	/// <summary>
	/// Holds the size of the screen after the node is loaded into the scene
	/// </summary>
	private Vector2 _screensize = Vector2.Zero;

	private AnimatedSprite2D animatedSprite2D;

	#endregion

	#region Constructors

	#endregion

	#region Properties

	/// <summary>
	/// 
	/// </summary>
	private float Velocity
	{
		get
		{
			return _speed * _gear;
		}
	}

	#endregion

	#region Override Methods

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// setup the screensize variable with the correct size.
		_screensize = GetViewportRect().Size;

		animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Drive((float)delta);
		
	}

	// handle keypresses
    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventKey eventKey)
		{
			// shift up on shift key press
			if (eventKey.Pressed && eventKey.Keycode == Key.Shift)
			{
				if (_gear < 6)
				{
					_gear++;
				}
			}
			// shift down on ctrl key press
			if (eventKey.Pressed && eventKey.Keycode == Key.Ctrl)
			{
				if (_gear > -2)
				{
					_gear--;
				}
			}
			// park on shift press
			if (eventKey.Pressed && eventKey.Keycode == Key.Space)
			{
				_gear = 0;
			}
		}
    }

    #endregion

    #region Methods

    /// <summary>
    /// Move the player car around based on user input.
    /// </summary>
    private void Drive(float delta)
	{
		// drive forward
		if (_gear > 0)
		{
			// rotate the sprite based on user input
			int direction = 0;
			if (Input.IsActionPressed("ui_left"))
			{
				direction--;
			}
			if (Input.IsActionPressed("ui_right"))
			{
				direction++;
			}
			Rotation += Mathf.Pi * direction * delta;
			
			// move the sprite based on user input
			Vector2 velocity = Vector2.Zero;
			if (Input.IsActionPressed("ui_up"))
			{
				velocity += Vector2.Up.Rotated(Rotation).Normalized() * Velocity;
			}
			
			Position += velocity * delta;
			Position = new Vector2(
				x: Mathf.Clamp(Position.X, 0, _screensize.X),
				y: Mathf.Clamp(Position.Y, 0, _screensize.Y)
			);

			animatedSprite2D.Animation = velocity.Normalized() == Vector2.Zero ? "park" : "drive";
			animatedSprite2D.Play();
		}
		// park
		else if (_gear == 0)
		{
			// rotate the sprite based on user input
			int direction = 0;
			if (Input.IsActionPressed("ui_left"))
			{
				direction--;
			}
			if (Input.IsActionPressed("ui_right"))
			{
				direction++;
			}
			Rotation += Mathf.Pi * direction * delta;

			animatedSprite2D.Animation = direction == 0 ? "park" : "drive";
			animatedSprite2D.Play();
		}
		// reverse
		else if (_gear < 0)
		{
			// rotate the sprite based on user input
			int direction = 0;
			if (Input.IsActionPressed("ui_left"))
			{
				direction--;
			}
			if (Input.IsActionPressed("ui_right"))
			{
				direction++;
			}
			Rotation += Mathf.Pi * direction * delta;

			// move the sprite based on user input
			Vector2 velocity = Vector2.Zero;
			if (Input.IsActionPressed("ui_up"))
			{
				velocity -= Vector2.Down.Rotated(Rotation).Normalized() * Velocity;
			}

			Position += velocity * delta;
			Position = new Vector2(
				x: Mathf.Clamp(Position.X, 0, _screensize.X),
				y: Mathf.Clamp(Position.Y, 0, _screensize.Y)
			);

			animatedSprite2D.Animation = velocity.Normalized() == Vector2.Zero ? "park" : "drive";
			animatedSprite2D.Play();
		}
	}

	private void OnBodyEntered(Node2D body)
	{
		Hide();
		EmitSignal(SignalName.Hit);

		GetNode<CollisionPolygon2D>("CollisionPolygon2D").SetDeferred(CollisionPolygon2D.PropertyName.Disabled, true);
	}

	#endregion

}
