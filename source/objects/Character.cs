using Godot;

using System.Collections.Generic;
using System;

using Newtonsoft.Json;

public class CharacterAnimation
{
    public bool loop { get; set; }
    public List<float> offsets { get; set; }
    public string anim { get; set; }
    public int fps { get; set; }
    public string name { get; set; }
    public List<int> indices { get; set; }
}

public class CharacterData
{
    public List<CharacterAnimation> animations { get; set; }
    public bool no_antialiasing { get; set; }
    public string image { get; set; }
    public List<float> position { get; set; }
    public string healthicon { get; set; }
    public bool flip_x { get; set; }
    public List<int> healthbar_colors { get; set; }
    public List<float> camera_position { get; set; }
    public double sing_duration { get; set; }
    public double scale { get; set; }
}

public partial class Character : Sprite2D
{
    CharacterData character;
	SparrowAnimation animation;
    Vector2 startingPosition;

	public override void _Ready()
	{
        string characterName = (String)GetMeta("CharacterName");

        character = loadCharacter(characterName);
        startingPosition = Position;

        animation = GetNode<Node>("Animation") as SparrowAnimation;
		animation.setpath(character.image);

        foreach (CharacterAnimation json in character.animations) {
            animation.create(json.anim, json.name, json.fps, json.loop);
        }

        FlipH = character.flip_x;

        float jsonScale = (float)character.scale;
        Scale = new Vector2(Scale.X * jsonScale, Scale.Y * jsonScale);
        playAnim("idle");
	}

    public void playAnim(string anim) {
        CharacterAnimation animdata = getAnimationData(character, anim);

        Vector2 haxeOffsets = new Vector2(animdata.offsets[0], animdata.offsets[1]);
        animation.play(anim);

        Position = new Vector2(startingPosition.X - haxeOffsets.X, startingPosition.Y - haxeOffsets.Y);
    }

    public static CharacterData loadCharacter(string charName) {
        string path = Paths.getPath("characters/" + charName + ".json");
        string jsonString = System.IO.File.ReadAllText(path);
        CharacterData charData = JsonConvert.DeserializeObject<CharacterData>(jsonString);
        return charData;
    }

    public static CharacterAnimation getAnimationData(CharacterData data, string anim) {
        CharacterAnimation animation = data.animations.Find(a => a.anim == anim);
        return animation;
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
                        playAnim("singUP");
                    }
                    break;

                case "game_left":
                    if (Input.IsActionJustPressed(action))
                    {
                        playAnim("singLEFT");
                    }
                    break;

                case "game_down":
                    if (Input.IsActionJustPressed(action))
                    {
                        playAnim("singDOWN");
                    }
                    break;

                case "game_right":
                    if (Input.IsActionJustPressed(action))
                    {
                        playAnim("singRIGHT");
                    }
                    break;

                default:
                    break;
            }
		}
    }
}