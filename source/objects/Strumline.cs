using Godot;

using System;
using System.Collections.Generic;

public class strumClass {
    public Sprite2D Sprite { get; set; }
    public int strumNumber { get; set; }

    public strumClass(Sprite2D _sprite, int _strumNumber)
    {
        Sprite = _sprite;
        strumNumber = _strumNumber;
    }
}

public partial class Strumline : Node2D
{
    List<strumClass> currentStrums = new List<strumClass>(); 

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        foreach (Sprite2D child in GetChildren())
        {
            string name = child.Name;
            name = name.ToLower();

            if (name.Contains("strum")) {
                int number = Convert.ToInt32(name.Split("_")[1]);

                strumClass newStrum = new strumClass(child, number);
                currentStrums.Add(newStrum);
            }
        }

        foreach (strumClass _class in currentStrums) {
            Sprite2D child = _class.Sprite;
            SparrowAnimation strumAnimation = child.GetNode<Node>("Animation") as SparrowAnimation;
            strumAnimation.setpath("NOTE_assets");
            strumAnimation.create("idle", "left confirm", 24, true);
            strumAnimation.play("idle");
        }
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
