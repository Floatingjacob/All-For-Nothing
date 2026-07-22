using Godot;

public partial class Decline : TextureButton
{
    public override void _Pressed()
    {
        GetNode<AudioStreamPlayer>("/root/Audio/Button").Play();
        Globals.SaleResult = 2;
    }
}