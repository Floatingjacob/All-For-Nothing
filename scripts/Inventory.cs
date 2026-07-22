using Godot;

public partial class Inventory : Node2D
{

	private int CurrentMap = 0;
	public static Inventory Instance;

	public override void _Ready()
	{
		Instance = this;
		GetNode<TextureButton>("Right").Pressed += () =>
		{
			GetNode<AudioStreamPlayer>("/root/Audio/Button").Play();
			if (Map.Instance.Maps.Length > 1)
			{
				Map.Instance.Maps = [.. Map.Instance.Maps[1..], Map.Instance.Maps[0]];
			}

			Refresh();
		};
		GetNode<TextureButton>("Left").Pressed += () =>
		{
			GetNode<AudioStreamPlayer>("/root/Audio/Button").Play();
			if (Map.Instance.Maps.Length > 1)
			{
				Map.Instance.Maps = [Map.Instance.Maps[^1], .. Map.Instance.Maps[..^1]];
			}

			Refresh();
		};
		GetNode<TextureButton>("Choose").Pressed += ChoosePressed;
		Refresh();
	}

	private async void ChoosePressed()
	{
		GetNode<AudioStreamPlayer>("/root/Audio/Button").Play();
		if (Map.Instance.Maps[0].Id == 0)
			foreach(Map.MapObject map in Map.Instance.Maps) 
				if (map.Id != 0) return;
				
		if (Map.Instance.Maps[0].Id == 3) GetNode<AnimatedSprite2D>("/root/Instance/Win").Play();
		else GetNode<AnimatedSprite2D>("/root/Instance/Lose").Play();
	}

	public void Refresh()
	{

		if (Map.Instance.Maps.Length == 0) return;

		Sprite2D left = GetNode<Sprite2D>("/root/Instance/InventoryMenu/LeftMap");
		Sprite2D center = GetNode<Sprite2D>("/root/Instance/InventoryMenu/ActiveMap");
		Sprite2D right = GetNode<Sprite2D>("/root/Instance/InventoryMenu/RightMap");

		left.Texture = null;
		center.Texture = null;
		right.Texture = null;

		center.Texture = Map.Instance.Maps[0].Sprite;
		right.Texture = Map.Instance.Maps[1].Sprite;
		left.Texture = Map.Instance.Maps[2].Sprite;
	}

}
