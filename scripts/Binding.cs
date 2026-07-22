using Godot;

public partial class Binding : Node
{
    public override void _Ready()
    {
        if (Globals.Config.Load("user://config.cfg") == Error.Ok && Globals.Config.HasSection("controls"))
        {
            foreach(string action in Globals.Config.GetSectionKeys("controls"))
            {
                InputMap.ActionEraseEvents(action);
                foreach (InputEvent _event in Globals.Config.GetValue("controls", action).AsGodotArray())
                {
                    InputMap.ActionAddEvent(action, _event);
                }
            }
        }
    }

    public static string GetBind(string Action) => InputMap.ActionGetEvents(Action)[0].ToString().Split(" - ")[0];

    public static void Bind(string Action, InputEventKey NewKey)
    {
        InputMap.ActionEraseEvents(Action);
        InputMap.ActionAddEvent(Action, NewKey);
        foreach (StringName action in InputMap.GetActions())
        {
            if (action == "UI_INTERACT" || action == "ui_up" || action == "ui_left" || action == "ui_right" || action == "UI_PAUSE" || action == "SKIP")
            {
                Globals.Config.SetValue("controls", action, InputMap.ActionGetEvents(action));
            }
        }
        Globals.Config.Save("user://config.cfg");
    }

}