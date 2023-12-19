


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
    private List<string> corresDir = new List<string> { "LEFT", "DOWN", "UP", "RIGHT" };
    List<strumClass> currentStrums = new List<strumClass>(); 

	public override void _Ready()
	{
        foreach (Sprite2D child in GetChildren())
        {
            string name = child.Name;
            name = name.ToLower();

            if (name.Contains("strum")) {
                int number = Convert.ToInt32(name.Split("_")[1]);

                SparrowAnimation strumAnimation = child.GetNode<Node>("Animation") as SparrowAnimation;
                strumAnimation.setpath("NOTE_assets");

                strumClass newStrum = new strumClass(child, number);
                currentStrums.Add(newStrum);
            }
        }

        foreach (strumClass _class in currentStrums) {
            Sprite2D child = _class.Sprite;
            SparrowAnimation strumAnimation = child.GetNode<Node>("Animation") as SparrowAnimation;

            for (int i = 0; i <= 3; i++) {
                strumAnimation.create($"press{corresDir[i]}", $"{corresDir[i].ToLower()} press", 24, false);
                strumAnimation.create($"confirm{corresDir[i]}", $"{corresDir[i].ToLower()} confirm", 24, false);
                strumAnimation.create($"arrow{corresDir[i]}", $"arrow{corresDir[i]}", 24, false);
            }

            strumAnimation.centerOffsets();
        }

        for (int i = 0; i <= 3; i++) {
            playAnim(i, $"arrow{corresDir[i]}");
        }
	}

    public void playAnim(int strumNumber, string animation) {
        strumClass daStrum = null;
        foreach (strumClass _class in currentStrums) {
            if (_class.strumNumber == strumNumber) {
                daStrum = _class;
            }
        }
        if (daStrum != null) {
            SparrowAnimation strum = daStrum.Sprite.GetNode<Node>("Animation") as SparrowAnimation;
            strum.play(animation);
        }
    }

	public override void _Process(double delta)
    {
        // Check if the "game_up" input is pressed
        foreach (string action in InputMap.GetActions())
        {
            for (int i = 0; i <= 3; i++) {
                string lowerdir = corresDir[i];
                lowerdir = lowerdir.ToLower();
                if (action == $"game_{lowerdir}"){ 
                    if (Input.IsActionJustPressed(action)) {
                        playAnim(i, $"press{corresDir[i]}");
                    }
                    if (Input.IsActionJustReleased(action)) {
                        playAnim(i, $"arrow{corresDir[i]}");
                    }
                }
            }
		}
    }
}
