public partial class Playfield : Node2D
{
	public Object createNoteObject() {
		Object note = loadObject(Paths.objects("Arrow"), GetNode<Node2D>("Notes"));
		return note;
	}
}
