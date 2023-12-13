using Godot;
using System.Collections.Generic;
using System;

public partial class Character : Sprite2D
{
	SparrowAnimation animation;

	public override void _Ready()
	{
		animation = GetNode<Node>("Animation") as SparrowAnimation;
		animation.setpath("pickleman");

		animation.create("idle","pickleman idle", 24, true);
		animation.create("up","pickleman up", 24, false);
		animation.create("down","pickleman down", 24, false);
		animation.create("left","pickleman left", 24, false);
		animation.create("right","pickleman right", 24, false);

		animation.play("idle");
	}

	public override void _Process(double delta)
    {
        // Check if the "game_up" input is pressed
        foreach (string action in InputMap.GetActions())
        {
            switch (action)
            {
                case "game_up":
                    if (Input.IsActionJustPressed(action))
                    {
                        animation.play("up");
                    }
                    break;

                case "game_left":
                    if (Input.IsActionJustPressed(action))
                    {
                        animation.play("left");
                    }
                    break;

                case "game_down":
                    if (Input.IsActionJustPressed(action))
                    {
                        animation.play("down");
                    }
                    break;

                case "game_right":
                    if (Input.IsActionJustPressed(action))
                    {
                        animation.play("right");
                    }
                    break;

                default:
                    break;
            }
		}
    }
}