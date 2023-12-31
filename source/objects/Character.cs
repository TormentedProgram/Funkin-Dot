

using System.Collections.Generic;


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
	public SparrowAnimation animation;

    public Vector2 startingPosition;
    float startingScale = 1;

    bool isPlayer = false;

    public bool isSinging = false;

    float animDuration = 0;

    private Timer returnToIdleTimer;

	public override void _Ready()
	{
        string characterName = (String)GetMeta("CharacterName");
        isPlayer = (bool)GetMeta("isPlayer");

        character = loadCharacter(characterName);
        startingPosition = new Vector2(Position.X + character.position[0], Position.Y + character.position[1]);

        TextureFilter = character.no_antialiasing ? TextureFilterEnum.Nearest : TextureFilterEnum.Linear;

        animation = GetNode<Node>("Animation") as SparrowAnimation;
		animation.setpath(character.image);

        foreach (CharacterAnimation json in character.animations) {
            animation.create(json.anim, json.name, json.fps, json.loop);
        }

        returnToIdleTimer = new Timer();
        returnToIdleTimer.OneShot = true;
        AddChild(returnToIdleTimer);

        animDuration = (float)character.sing_duration;
        startingScale = Scale.X;

        FlipH = character.flip_x;
        if (isPlayer) FlipH = !FlipH;

        float jsonScale = (float)character.scale;
        Scale = new Vector2(Scale.X * jsonScale, Scale.Y * jsonScale);

        playAnim("idle");
	}

    private async void returnToIdle()
    {
        if (returnToIdleTimer.IsStopped())
        {
            returnToIdleTimer.WaitTime = Conductor.stepCrochet * animDuration / 1000;
            returnToIdleTimer.Start();

            await ToSignal(returnToIdleTimer, "timeout");
            isSinging = false;
        }
        else
        {
            returnToIdleTimer.Stop();
            returnToIdleTimer.WaitTime = Conductor.stepCrochet * animDuration / 1000;
            returnToIdleTimer.Start();
        }
    }

    public void playAnim(string anim) {
        isSinging = true;
        CharacterAnimation animdata = getAnimationData(character, anim);

        Vector2 haxeOffsets = new Vector2(animdata.offsets[0], animdata.offsets[1]);
        animation.play(anim);

        returnToIdle();

        haxeOffsets.X *= startingScale;
        haxeOffsets.Y *= startingScale;

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
}