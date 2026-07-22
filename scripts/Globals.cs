using Godot;

public partial class Globals : Node
{
    public static AudioStream TransitionSFX = GD.Load<AudioStream>("res://assets/sfx/transition.ogg");
    public static int Seller = 0;
    public static bool DialogueOpen;
    public static ConfigFile Config = new();
    public static int SaleResult = 0;
    public static bool RoundInProgress;

    public static void Reset()
    {
        Seller = 0;
        SaleResult = 0;
        RoundInProgress = false;
        Dialogue.Instance.Reset();
        Sun.Instance.Reset();
        Money.Instance.Set(1000);
        Map.Instance.Maps = [];
    }

}