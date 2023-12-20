public partial class EngineUtil : Node
{
	public static string formatPath(string path) {
		//this needs to be fixed for future versions
		return ProjectSettings.GlobalizePath(path);
	}

	public static Object loadObject(string path, Node rootNode) {
		PackedScene scene = GD.Load<PackedScene>(path);
		Node instance = scene.Instantiate();
		rootNode.CallDeferred("add_child", instance);
		return instance;
	}

	public static string checkExistLoc(string path) {
		string regPath = $"res://assets/{path}";
		string fullPath = formatPath(regPath);

		if (File.Exists(fullPath)) {
			return fullPath;
		}else{	
			if (File.Exists(regPath)) {
				return regPath;
			}
		}
		return null;
	}
}
