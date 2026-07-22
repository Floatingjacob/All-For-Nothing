using Godot;

public partial class Purchase : TextureButton
{
    public override void _Pressed()
    {
        GetNode<AudioStreamPlayer>("/root/Audio/Button").Play();
        Globals.SaleResult = 1;
    }
}