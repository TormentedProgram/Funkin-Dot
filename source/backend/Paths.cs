


using System.IO;

public enum AssetType
{
    IMAGE,
    ATLAS,
    SOUND,
    TEXT
}

public partial class Paths : Node
{
	public static string getPath(string path, AssetType assetType = AssetType.TEXT)
	{
		string regPath = $"res://assets/{path}";
		string fullPath = formatPath(regPath);

		if (checkExistLoc(path) != null) return checkExistLoc(path);
		switch (assetType) {
			case AssetType.IMAGE:
				GD.PrintErr($"RETURNING NULL AT {fullPath} (IMAGE)");
				return "res://assets/images/godot.png";
			case AssetType.ATLAS:
				if (checkExistLoc(path + ".xml") != null && checkExistLoc(path + ".png") != null) {
					return checkExistLoc(path + ".xml").Split('.')[0];
				}
				break;
		}
		GD.PrintErr($"RETURNING NULL AT {fullPath}");
		return null;
	}

	public static Texture2D image(string path) {
		Texture2D bitmap = (Texture2D)GD.Load(getPath($"images/{path}.png", AssetType.IMAGE));
		return bitmap;
	}

	public static string objects(string path) {
		return getPath($"objects/{path}.tscn",AssetType.TEXT);
	}

	public static string json(string path) {
		return getPath($"songs/{path}.json",AssetType.TEXT);
	}

	public static string atlas(string path) {
		return getPath($"images/{path}",AssetType.ATLAS);
	}
}
