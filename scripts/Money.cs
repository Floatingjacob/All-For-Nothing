using Godot;

public partial class Money : RichTextLabel
{
    public static Money Instance;
    public static int Balance = 1000;

    public override void _Ready()
    {
        Instance = this;
        Text = $"${Balance}";
    }
    public void Set(int Amount)
    {
        Text = $"${Amount}";
        Balance = Amount;
    }
}