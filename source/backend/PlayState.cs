public partial class PlayState : MusicBeatState
{
	public SwagSong SONG;
	public static PlayState instance;

	public AudioStreamPlayer2D vocals;
	public AudioStreamPlayer2D inst;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		instance = this;
        Input.MouseMode = Input.MouseModeEnum.Hidden;

		var songName = "test";
		SONG = Song.loadFromJson(Paths.json($"{songName}/chart"));

		vocals = GetNode<AudioStreamPlayer2D>("Vocals");
		vocals.Stream = Paths.song($"{songName}/Voices");
		vocals.Play(0);

		inst = GetNode<AudioStreamPlayer2D>("Inst");
		inst.Stream = Paths.song($"{songName}/Inst");
		inst.Play(0);

		Note.initChart(SONG);

		foreach (arrowClass arrow in Note.loadedNotes) {
			//GD.Print(arrow.strumTime);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
