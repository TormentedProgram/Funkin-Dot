public partial class PlayState : MusicBeatState
{
	public SwagSong SONG;
	public static PlayState instance;

	public AudioStreamPlayer2D vocals;
	public AudioStreamPlayer2D inst;

	public Character Boyfriend;
	public Character Dad;

	bool init = false;

	public string songName = "test";

	public override void _Ready()
	{
		instance = this;
        Input.MouseMode = Input.MouseModeEnum.Hidden;

		SONG = Song.loadFromJson(Paths.json($"{songName}/chart"));

		Conductor.mapBPMChanges(SONG);
        Conductor.set_bpm(SONG.bpm);

		Note.initChart(SONG);

		//my pref for now
		SONG.speed = 3.8f;

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
		base._Process(delta);

		if (init)	{
			for (int i = 0; i <= 3; i++) {
				SparrowAnimation dastrum = GetNode<SparrowAnimation>($"../Main/camHUD/DadPlayfield/Strumline/Strum_{i}/Animation");
				if (dastrum.finished && dastrum.curAnim.Contains("confirm")) {
					Strumline dastrum2 = GetParent<Node2D>().GetNode<Camera2D>("Main").GetNode<Control>("camHUD").GetNode<Node2D>("DadPlayfield").GetNode<Strumline>("Strumline");
					dastrum2.playAnim(i, $"arrow{(corresDir[i].ToUpper())}");
				}
			}
		}

		if (inst != null) Conductor.songPosition = inst.GetPlaybackPosition() * 1000;

		if (Note.loadedNotes.Count > 0) {	
			if (!init)	{
				init = true;
				foreach(arrowClass guh in Note.loadedNotes) {
					guh.Sprite.GetNode<SparrowAnimation>("Animation").play(guh.noteData.ToString());
				}
			}
		}

		if (init) {
			foreach (arrowClass aliveNote in Note.loadedNotes) {
				if (aliveNote.Sprite != null) {
					int strumNum = aliveNote.noteData % 4;

					float noteX = aliveNote.Sprite.GetParent<Node2D>().GetParent<Node2D>().GetNode<Node2D>("Strumline").GetNode<Sprite2D>($"Strum_{strumNum}").Transform.Origin.X;
					float noteY = 0.45f * SONG.speed * (aliveNote.strumTime - Conductor.songPosition);

					Transform2D currentTransform = aliveNote.Sprite.Transform;
					currentTransform.Origin.X = noteX;
					currentTransform.Origin.Y = noteY;
					aliveNote.Sprite.Transform = currentTransform;

					if (aliveNote.strumTime - Conductor.songPosition <= 0) {
						if (!aliveNote.mustPress) {
							opponentNoteHit(aliveNote);
						}else{
							goodNoteHit(aliveNote);
						}
					}
				}
			}
		}
	}

    public override void stepHit()
    {
        base.stepHit();
    }

    public override void beatHit()
    {
		if (curBeat % 2 == 0 && !Dad.isSinging) {
			Dad.playAnim("idle");
		}
		if (curBeat % 2 == 0 && !Boyfriend.isSinging) {
			Boyfriend.playAnim("idle");
		}

        base.beatHit();
    }

    private static List<string> corresDir = new List<string> { "left", "down", "up", "right" };
	public void opponentNoteHit(arrowClass arrow) {
		Strumline dastrum = arrow.Sprite.GetParent<Node2D>().GetParent<Node2D>().GetNode<Strumline>("Strumline");

		dastrum.playAnim(arrow.noteData, $"confirm{(corresDir[arrow.noteData].ToUpper())}");
		Dad.playAnim($"sing{corresDir[arrow.noteData].ToUpper()}");

		Note.loadedNotes.Remove(arrow);
		arrow.Sprite.QueueFree();
	}

	public void goodNoteHit(arrowClass arrow) {
		Boyfriend.playAnim($"sing{corresDir[arrow.noteData].ToUpper()}");
		Note.loadedNotes.Remove(arrow);
		arrow.Sprite.QueueFree();
	}
}
