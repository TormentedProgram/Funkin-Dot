public partial class EngineUtil : Node
{
	public static string formatPath(string path) {
		//this needs to be fixed for future versions
		return ProjectSettings.GlobalizePath(path);
	}
}
