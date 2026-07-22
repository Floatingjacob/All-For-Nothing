using Godot;
using System;

public partial class CloudPath : Path2D
{
	[Export]
	double Speed = 1;
	private Texture2D[] Clouds = [GD.Load<Texture2D>("res://assets/sprites/clouds/Cloud 1.png"), GD.Load<Texture2D>("res://assets/sprites/clouds/Cloud 2.png")];
	private Random Rng = new();

	public override void _Ready()
	{
		
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
	{
		PathFollow2D _cloudPath = GetNode<PathFollow2D>("Cloud");
		Sprite2D _cloudSprite = GetNode<Sprite2D>("Cloud/Cloud");
		if (_cloudPath.ProgressRatio >= 0.9)
		{
			float rand = new RandomNumberGenerator().RandfRange(0.4F, 1);
			GetNode<Node2D>("../").GlobalPosition = new Vector2(-184, Rng.Next(88, 270));
			_cloudSprite.Texture = Clouds[Rng.Next(0, 1)];
			_cloudSprite.Scale = new Vector2(rand, rand);
			_cloudPath.ProgressRatio = 0;
		}
		_cloudPath.ProgressRatio += (float)(Speed * delta);
	}

}
