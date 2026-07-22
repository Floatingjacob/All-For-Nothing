using System.Threading.Tasks;
using Godot;

public partial class Sun : PathFollow2D
{
    [Export]
    public double Speed = 0.05;
    public static Sun Instance;
    private double OldProgress;
    private bool Advancing;

    public override void _Ready()
    {
        Instance = this;
    }


    public override void _PhysicsProcess(double delta)
    {
        if (Advancing)
        {
            if (OldProgress + 0.33 < ProgressRatio)
            {
                Advancing = false;
                return;
            }
            ProgressRatio += (float)(Speed * delta);
        }
    }

    public async Task Advance()
    {
        OldProgress = ProgressRatio;
        Advancing = true;
        while(Advancing) await ToSignal(GetTree().CreateTimer(0.01), SceneTreeTimer.SignalName.Timeout);
    }

    public void Reset()
    {
        OldProgress = 0;
        Advancing = false;
    }

}