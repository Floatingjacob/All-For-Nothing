using System.Threading.Tasks;
using Godot;

public partial class Game : Node2D
{

    public override void _Ready()
    {
        GetNode("/root/Instance/Transition").Connect("act", Callable.From(() =>
		{
			if (Globals.Seller < 3)
			{
				Start();
			}
			else
			{
				GetNode<Node2D>("/root/Instance/Game/SaleLayer").Hide();
				Inventory.Instance.Refresh();
				GetNode<Node2D>("/root/Instance/InventoryMenu").Show();
			}
		}));
    }

	public async void Start()
	{
		Globals.RoundInProgress = true;
		Dialogue.Instance.Reset();
		GetNode("/root/Globals").Set("transitioning", true);
		await Next();
		await ToSignal(GetNode("/root/Instance/Transition"), "doneTransitioning");
		await Map.Instance.Next();
		await Sun.Instance.Advance();
		GetNode("/root/Globals").Set("transitioning", true);
		Globals.RoundInProgress = false;
		return;
	}

	public async Task Next()
	{
		Globals.Seller++;
		Reset();
		GetNode<Node2D>("/root/Instance/Game/SaleLayer").Show();
		AnimatedSprite2D _vendor = GetNode<AnimatedSprite2D>("/root/Instance/Game/SaleLayer/Vendor");
		Sprite2D _map = GetNode<Sprite2D>("/root/Instance/Game/SaleLayer/Map");

		_vendor.Animation = $"{Globals.Seller}";
		_map.Texture = GD.Load<Texture2D>($"res://assets/sprites/maps/map {Globals.Seller}.png");
		_vendor.Show();
		_map.Show();
	}

	public void Reset()
	{
		AnimatedSprite2D _vendor = GetNode<AnimatedSprite2D>("/root/Instance/Game/SaleLayer/Vendor");
		Sprite2D _map = GetNode<Sprite2D>("/root/Instance/Game/SaleLayer/Map");

		_vendor.Hide();
		_map.Hide();
	}
}
