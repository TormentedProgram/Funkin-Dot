public partial class Playfield : Node2D
{
	public Object createNoteObject() {
		return loadObject(Paths.objects("Arrow"), GetNode<Node2D>("Notes"));
	}
}
