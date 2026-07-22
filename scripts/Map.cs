using System;
using System.Linq;
using System.Threading.Tasks;
using Godot;

public partial class Map : Node
{
    
    private static readonly string[] Dialogues = ["!Hey bro, I found this map on a path during my morning walk.\n\n|I don't have much use for it...\n\n", "!Hey buddy... *twitches*\n\n|I found this...[i]map...[/i]\n\n", "!Hey there, handsome *giggles*\n|I made something for you.\n|You just have to gift me some money before I give it to you.\n\n"];
    public static Map Instance;

    public MapObject[] Maps = [];

    public override void _Ready()
    {
        Instance = this;
    }

    public async Task Next()
    {
        Node2D _choiceButtons = GetNode<Node2D>("/root/Instance/Game/SaleLayer/Buttons");

        GetNode<Node2D>("/root/Instance/Game/SaleLayer").Show();
        MapObject _sale = MapObject.New();
        GetNode<Sprite2D>("/root/Instance/Game/SaleLayer/Map").Texture = _sale.Sprite;
        foreach(string s in MapObject.SellDialogue.Split('|'))
        {
            if(s[0] == '!') await Dialogue.Instance.FancyType(s.Substring(1), true);
            else await Dialogue.Instance.FancyType(s, false);
        }
        await Dialogue.Instance.FancyType($"I'll sell it to you for [color=cyan]${_sale.Price}.[/color] Deal?", false, true);
        bool _addMap;

        _choiceButtons.Show();

        while (Globals.SaleResult == 0) await ToSignal(GetTree().CreateTimer(0.01), SceneTreeTimer.SignalName.Timeout);
        
        _choiceButtons.Hide();
       // await ToSignal(GetTree().CreateTimer(0.3), SceneTreeTimer.SignalName.Timeout);
        _addMap = Globals.SaleResult == 1;

        if (_addMap)
        {
            if (Money.Balance < _sale.Price)
            {
                await Dialogue.Instance.FancyType("Oh... [color=red]You can't afford this...[/color]", true, true);
                _sale.Id = 0;
                _sale.Sprite = GD.Load<Texture2D>("res://assets/sprites/maps/x.png");
                Maps = Maps.Append(_sale).ToArray();
            }
            else
            {    
                _sale.Id = Globals.Seller;
                Maps = Maps.Append(_sale).ToArray();
                Money.Instance.Set(Money.Balance - _sale.Price);
                await Dialogue.Instance.FancyType($"Pleasure doing business.", true, true);
            }
        }
        else
        {
            _sale.Id = 0;
            _sale.Sprite = GD.Load<Texture2D>("res://assets/sprites/maps/x.png");
            Maps = Maps.Append(_sale).ToArray();
            await Dialogue.Instance.FancyType("No?\n", true);
            await Dialogue.Instance.FancyType("What a shame...", false, true);
        }
        Globals.SaleResult = 0;
    }

      public class MapObject
    {
        public static string SellDialogue;
        public int Id;
        public int Price = new Random().Next(200, 800);
        public Texture2D Sprite = GD.Load<Texture2D>($"res://assets/sprites/maps/map {Globals.Seller}.png");

        public static MapObject New()
        {
            SellDialogue = Dialogues[Globals.Seller - 1];
            return new MapObject();
        }
    }

}