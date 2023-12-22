using static JsonUtil;

public partial class Song : Node
{
	public string song;
    public List<SwagSection> notes;
    public float bpm;
    public bool needsVoices = true;
    public float speed = 1;

	public Song(string song, List<SwagSection> notes, float bpm)
    {
        this.song = song;
        this.notes = notes;
        this.bpm = bpm;
    }

	public static SwagSong loadFromJson(string _path)
    {
		string path = formatPath(_path);

        if (!File.Exists(path))
        {
            GD.PrintErr($"JSON file not found: {path}");
            //return loadDefault(asset);
            return null;
        }

		string rawJson = File.ReadAllText(path);
        rawJson = formatJson(rawJson);
        
        while (!rawJson.EndsWith("}"))
        {
            rawJson = rawJson.Substring(0, rawJson.Length - 1);
        }
        
        return parseJson(fetchObject(rawJson, "song"));
    }

	public static SwagSong parseJson(string rawJson)
    {
		SwagSong swagShit = JsonConvert.DeserializeObject<SwagSong>(rawJson);
        swagShit.validScore = true;
        return swagShit;
    }
}
