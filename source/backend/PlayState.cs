public partial class PlayState : MusicBeatState
{
	public SwagSong SONG;
	public static PlayState instance;

	public AudioStreamPlayer2D vocals;
	public AudioStreamPlayer2D inst;

	public Character Boyfriend;
	public Character Dad;

	public string songName = "test";
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		instance = this;
        Input.MouseMode = Input.MouseModeEnum.Hidden;

		SONG = Song.loadFromJson(Paths.json($"{songName}/chart"));

		Note.initChart(SONG);

		Boyfriend = GetParent<Node2D>().GetNode<Character>("Boyfriend");
		Dad = GetParent<Node2D>().GetNode<Character>("Dad");

		vocals = GetNode<AudioStreamPlayer2D>("Vocals");
		vocals.Stream = Paths.song($"{songName}/Voices");

		inst = GetNode<AudioStreamPlayer2D>("Inst");
		inst.Stream = Paths.song($"{songName}/Inst");

		vocals.Play(0);
		inst.Play(0);
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
