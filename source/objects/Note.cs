


public class arrowClass {
	public Sprite2D Sprite { get; set; }

	public int noteData { get; set; }
	public bool isSustain { get; set; }
	public float strumTime { get; set; }
	public arrowClass lastNote { get; set; }
	public string noteType { get; set; }
	
	public bool mustPress { get; set; }
	public float sustainLength { get; set; }

	public arrowClass(float _strumTime, int _noteData, arrowClass _lastNote, bool _isSustain, string _noteType)
    {
		strumTime = _strumTime;
        noteData = _noteData;
		lastNote = _lastNote;
		isSustain = _isSustain;
		noteType = _noteType;
    }
}

public partial class Note : Node
{
	public static List<arrowClass> loadedNotes = new List<arrowClass>();

	public static arrowClass createNote(float strumTime, int noteData, arrowClass lastNote, bool isSustain, string noteType) {
		arrowClass note = new arrowClass(strumTime, noteData, lastNote, isSustain, noteType);
		loadedNotes.Add(note);
		return note;
	}

	public static void initChart(SwagSong SONG) {
        Playfield DadField = (Playfield)PlayState.instance.GetNode("../Main/camHUD/DadPlayfield");
		Playfield BFField = (Playfield)PlayState.instance.GetNode("../Main/camHUD/BoyfriendPlayfield");

		foreach (SwagSection section in SONG.notes)
        {
            foreach (SwagNote songNotes in section.sectionNotes)
            {
                float daStrumTime = songNotes.strumTime;
                string daNoteType = songNotes.noteType;
                int daNoteData = (int)songNotes.noteData % 4;
                bool gottaHitNote = section.mustHitSection;

                if (songNotes.noteData % 8 > 3)
                {
                    gottaHitNote = !gottaHitNote;
                }

                arrowClass oldNote;
                if (loadedNotes.Count > 0)
                {
                    oldNote = loadedNotes[(int)loadedNotes.Count - 1];
                }
                else
                {
                    oldNote = null;
                }

                Playfield field = gottaHitNote ? BFField : DadField;
                arrowClass swagNote = createNote(daStrumTime, daNoteData, oldNote, false, daNoteType);
                swagNote.Sprite = field.createNoteObject() as Sprite2D;
                swagNote.mustPress = gottaHitNote;
                swagNote.sustainLength = songNotes.sustainLength;

                //Calculate and create sustain notes
                float susLength = swagNote.sustainLength / Conductor.stepCrochet;
                for (int susNote = 0; susNote < susLength; susNote++)
                {
                    oldNote = loadedNotes[(int)loadedNotes.Count - 1];
                    float susNoteTime = daStrumTime + (Conductor.stepCrochet * susNote) + Conductor.stepCrochet;
                    arrowClass swagSusNote = createNote(susNoteTime, daNoteData, oldNote, true, daNoteType);
                    swagSusNote.Sprite = field.createNoteObject() as Sprite2D;
                    swagSusNote.mustPress = gottaHitNote;
                }
            }
        }  
	}
}
