using Godot;
using System;

public partial class Player : Area2D
{
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
				velocity += Vector2.Up.Rotated(Rotation) * Velocity;
			}
			
			Position += velocity * delta;
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
			if (Input.IsActionPressed("ui_down"))
			{
				velocity -= Vector2.Down.Rotated(Rotation) * Velocity;
			}

			Position += velocity * delta;
		}
	}

	#endregion
}
