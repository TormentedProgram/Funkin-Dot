
using System.Collections.Generic;
using System.Xml;


public class SparrowParser
{
    public static List<SpriteMeta> ParseAsset(string _path)
    {
        var formattedPath = formatPath(_path);
        Texture2D asset = GD.Load(_path + ".png") as Texture2D;

        if (!File.Exists(formattedPath + ".xml"))
        {
            GD.PrintErr($"XML file not found: {formattedPath}.xml");
            return loadDefault(asset);
        }

		string xmlString = File.ReadAllText(formattedPath + ".xml");

        XmlDocument doc = new XmlDocument();
        doc.LoadXml(xmlString);

        XmlNodeList subTextures = doc.SelectNodes("//SubTexture");
        List<SpriteMeta> spriteSheet = new List<SpriteMeta>();

        foreach (XmlNode node in subTextures)
        {
            string name = GetAttribute(node, "name");

            float x = GetFloatAttribute(node, "x");
            float y = GetFloatAttribute(node, "y");
            float width = GetFloatAttribute(node, "width");
            float height = GetFloatAttribute(node, "height");

            if (width < 0 || height < 0)
            {
                GD.PrintErr($"Invalid dimensions detected for sprite '{name}' (width={width}, height={height}). Import has to be aborted. Please check the XML file content.");
                return null;
            }

            SpriteMeta smd = new SpriteMeta();
            smd.name = name;
            smd.alignment = 9;

            float frameX = 0;
            float frameY = 0;

            if (width != 0 && HasAttribute(node, "frameX"))
            {
                frameX = GetFloatAttribute(node, "frameX");
            }
            if (height != 0 && HasAttribute(node, "frameY"))
            {
                frameY = GetFloatAttribute(node, "frameY");
            }

            smd.rect = new Rect2(x, y, width, height);  
            smd.offset = new Vector2(-frameX , -frameY);

            spriteSheet.Add(smd);
        }

        if (spriteSheet.Count != 0)
        {
            return spriteSheet;
        }

        return null;
    }

    private static List<SpriteMeta> loadDefault(Texture2D texture)
    {
        // You can define your default SpriteMeta here
        Vector2 textureSize = texture.GetSize();
        SpriteMeta defaultSprite = new SpriteMeta
        {
            name = "DefaultSprite",
            rect = new Rect2(Vector2.Zero, textureSize), // Set default dimensions
            offset = Vector2.Zero,
            alignment = 9 // Set default alignment
        };

        return new List<SpriteMeta> { defaultSprite };
    }

    private static float GetFloatAttribute(XmlNode node, string name, float defaultValue = 0)
    {
        XmlNode attribute = node.Attributes.GetNamedItem(name);
        if (attribute == null)
            return defaultValue;

        return float.Parse(attribute.Value);
    }

    private static string GetAttribute(XmlNode node, string name, string defaultValue = "")
    {
        XmlNode attribute = node.Attributes.GetNamedItem(name);
        if (attribute == null)
            return defaultValue;
        return attribute.Value;
    }

    private static bool HasAttribute(XmlNode node, string name)
    {
        return node.Attributes.GetNamedItem(name) != null;
    }
}

public class SpriteMeta
{
	public string name;
	public Rect2 rect;
    public Vector2 offset = new Vector2(0,0);
	public int alignment;
}