using System.Xml;
using System.Text;

public class SparrowParser
{
    public static List<SpriteMeta> ParseAsset(string _path)
    {
        var formattedPath = formatPath(_path);

        if (!File.Exists(formattedPath + ".xml"))
        {
            GD.PrintErr($"XML file not found: {formattedPath}.xml");
            return loadDefault();
        }

		string xmlString = File.ReadAllText(formattedPath + ".xml");
        
        //thanks to Hans Passant, Amit Merin on stackoverflow 
        string _byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
        if (xmlString.StartsWith(_byteOrderMarkUtf8, StringComparison.Ordinal))
        {
            xmlString = xmlString.Remove(0, _byteOrderMarkUtf8.Length);
        }

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
                GD.PrintErr($"Invalid dimensions detected for sprite '{name}' (width={width}, height={height}). Please check the XML file content.");
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

    private static List<SpriteMeta> loadDefault()
    {
        // You can define your default SpriteMeta here

        SpriteMeta defaultSprite = new SpriteMeta
        {
            name = "DefaultSprite",
            rect = new Rect2(Vector2.Zero, new Vector2(100,100)), // Set default dimensions
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