public partial class EngineUtil : Node
{	
	public static string formatPath(string path) {
		//finally works outta editor (you need move assets into the export dir)
		var _Path = path;
		if (OS.HasFeature("editor")) {
			_Path = ProjectSettings.GlobalizePath(path);
		}else{
			if (path.Contains("res:")) {
				_Path = OS.GetExecutablePath().GetBaseDir() + (path);
				_Path = _Path.Replace("res://", "/");
			}
		}
		return _Path;
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
