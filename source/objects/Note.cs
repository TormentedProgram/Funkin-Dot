


public class arrowClass {
	public Sprite2D Sprite { get; set; }

	public int noteData { get; set; }
	public bool isSustain { get; set; }
	public float strumTime { get; set; }
	public arrowClass lastNote { get; set; }
	public string noteType { get; set; }

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
	public static arrowClass createNote(float strumTime, int noteData, arrowClass lastNote, bool isSustain, string noteType) {
		arrowClass note = new arrowClass(strumTime, noteData, lastNote, isSustain, noteType);
		return note;
	}
}
