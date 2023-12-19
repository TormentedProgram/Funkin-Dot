using Godot;

using System;
using System.Collections.Generic;

using Newtonsoft.Json;

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
		string path = EngineUtil.formatPath(_path);

        if (!System.IO.File.Exists(path))
        {
            GD.PrintErr($"JSON file not found: {path}");
            //return loadDefault(asset);
            return null;
        }

		string rawJson = System.IO.File.ReadAllText(path);
        
        while (!rawJson.EndsWith("}"))
        {
            rawJson = rawJson.Substring(0, rawJson.Length - 1);
        }
        
        return parseJSONshit(rawJson);;
    }

	public static SwagSong parseJSONshit(string rawJson)
    {
		SwagSong swagShit = JsonConvert.DeserializeObject<SwagSong>(rawJson);
        swagShit.validScore = true;
        return swagShit;
    }
}
