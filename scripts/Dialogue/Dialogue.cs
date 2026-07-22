using System;
using System.Threading.Tasks;
using Godot;

public partial class Dialogue : Node2D
{
    private static AudioStream DialogueSoundEffect;
    public static Dialogue Instance {get; private set;}
    [Signal]
    public delegate void EEventHandler();
    private string OldText;
    [Export]
    public double TypingInterval = 0.05;
    
    public async Task FancyType(string NewText, bool ClearOldText, bool End = false)
    {
        AnimatedSprite2D _vendor = GetNode<AnimatedSprite2D>("Vendor");
        RichTextLabel _textContainer = GetNode<RichTextLabel>("DialogueBox");
        RichTextLabel _continueMarker = GetNode<RichTextLabel>("Continue");

        _continueMarker.Hide();

        Globals.DialogueOpen = true;

        if (ClearOldText)
        {
            _textContainer.Clear();
            _textContainer.Text = "";
            _textContainer.VisibleCharacters = 0;
            OldText = "";
        }

        OldText += NewText;
        _textContainer.AppendText(NewText);
        _vendor.Play($"{Globals.Seller}");
        while (_textContainer.VisibleCharacters < _textContainer.GetTotalCharacterCount() && !Input.IsActionPressed("SKIP"))
        {
            
            if (!GetNode<AudioStreamPlayer>("/root/Audio/SFX").Playing)
            {
                switch (Globals.Seller)
                {
                    case 1:
                        GetNode<AudioStreamPlayer>("/root/Audio/SFX").Stream = GD.Load<AudioStream>($"res://assets/sfx/dialogue/squeakm{new Random().Next(1, 3)}.wav");
                        GetNode<AudioStreamPlayer>("/root/Audio/SFX").Play();
                        break;
                    case 2:
                        GetNode<AudioStreamPlayer>("/root/Audio/SFX").Stream = GD.Load<AudioStream>($"res://assets/sfx/dialogue/c{new Random().Next(1, 4)}.wav");
                        GetNode<AudioStreamPlayer>("/root/Audio/SFX").Play();
                        break;
                    case 3:
                        GetNode<AudioStreamPlayer>("/root/Audio/SFX").Stream = GD.Load<AudioStream>($"res://assets/sfx/dialogue/squeakf{new Random().Next(1, 4)}.wav");
                        GetNode<AudioStreamPlayer>("/root/Audio/SFX").Play();
                    break;
                }
                
            }

            _textContainer.VisibleCharacters++;
            await ToSignal(GetTree().CreateTimer(TypingInterval), SceneTreeTimer.SignalName.Timeout);
        }
        _vendor.Stop();
        _textContainer.Text = OldText;
        _textContainer.VisibleCharacters = _textContainer.GetParsedText().Length; // This makes sure all the text is visible if we skip the typing effect

        if (!End){
            _continueMarker.Show();
            await ToSignal(this, SignalName.E);
            _continueMarker.Hide(); 
        }
         Globals.DialogueOpen = false;
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("UI_INTERACT")) EmitSignal(SignalName.E);
    }

    /*public void SetSpeaker(AnimatedSprite2D Speaker)
    {
        foreach (Node2D Child in GetNode("speaker").GetChildren())
        {
            if (Child != Speaker) Child.Hide();
            else 
            {
                Child.Show();
                return;
            }
        }
    }*/

    public override void _Ready()
    {
        Instance = this;
        if (Globals.Config.Load("user://config.cfg") == Error.Ok) TypingInterval = (double)Globals.Config.GetValue("typing", "speed");
    }

    public override void _Process(double delta)
    {
        string _continueText = GetNode<RichTextLabel>("Continue").Text;
        if (HasNode("Continue") && _continueText != $"[wave]{Binding.GetBind("UI_INTERACT")}") _continueText = $"[wave]{Binding.GetBind("UI_INTERACT")}";
    }

    public void Reset()
    {
        EmitSignal(SignalName.E);
        GetNode<RichTextLabel>("DialogueBox").Text = "";
        GetNode<RichTextLabel>("DialogueBox").VisibleCharacters = -1;
        OldText = "";
        Globals.DialogueOpen = false;
    }

}