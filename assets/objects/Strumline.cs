using Godot;
using System;

public partial class Strumline : Node2D
{
    private SparrowAnimation script;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        for (int i = 0; i < 1; i++) // Corrected loop condition
        {
            Node obj = GetNode<Node>($"Strum_{i}/Animation");
            script = obj as SparrowAnimation;

            script.setpath("NOTE_assets");
            script.create("idle", "left confirm", 24, true);
            script.play("idle");
        }
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
