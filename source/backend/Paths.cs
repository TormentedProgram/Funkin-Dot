using Godot;

using System;
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
		string fullPath = $"res://assets/{path}";
		fullPath = ProjectSettings.GlobalizePath(fullPath);

		switch (assetType)
		{
			case AssetType.IMAGE:
				if (!File.Exists(fullPath))
				{
					GD.Print($"Image not found at path: {fullPath}");
					fullPath = "res://assets/images/godot.png"; // fallback image
				}
				break;
		}
		
		return fullPath;
	}

	public static Texture2D image(string path) {
		Texture2D bitmap = (Texture2D)GD.Load(getPath($"images/{path}.png", AssetType.IMAGE));
		return bitmap;
	}

	public static string json(string path) {
		return getPath($"songs/{path}.json",AssetType.TEXT);
	}

	public static string atlas(string path) {
		return getPath($"images/{path}",AssetType.ATLAS);
	}
}
