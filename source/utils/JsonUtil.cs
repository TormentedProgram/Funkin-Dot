public partial class JsonUtil:Node
{
	// written by Bekwnn, 2015
	// contributed by Guney Ozsan, 2016
	public static string fetchObject(string jsonString, string handle)
    {
        string pattern = "\"" + handle + "\"\\s*:\\s*\\{";

        Regex regx = new Regex(pattern);

        Match match = regx.Match(jsonString);
        
        if (match.Success)
        {
            int bracketCount = 1;
            int i;
            int startOfObj = match.Index + match.Length;
            for (i = startOfObj; bracketCount > 0; i++)
            {
                if (jsonString[i] == '{') bracketCount++;
                else if (jsonString[i] == '}') bracketCount--;
            }
            return "{" + jsonString.Substring(startOfObj, i - startOfObj);
        }

        return null;
    }

	public static string formatJson(string _json)
    {
        JObject data = JObject.Parse(_json);
        JArray notesData = (JArray)data["song"]["notes"];
        JArray newNotesData = new JArray();
        foreach (JObject section in notesData)
        {
            JArray sectionNotes = (JArray)section["sectionNotes"];
            JObject noteshit;

            JArray newSectionNotes = new JArray(sectionNotes.Select(note =>
                noteshit = new JObject
                {
                    { "strumTime", (int)note[0] >= 0 ? note[0] : 0 },
                    { "noteData", (int)note[1] >= 0 ? note[1] : -1},
                    { "sustainLength", double.TryParse(note.Count() >= 3 && note[2] is string ? (string)note[2] : note[2]?.ToString(), out var parsedSustain) ? parsedSustain : 0 },
                    { "noteType" , note.Count() >= 4 ? note[3] : "default"}
                }));

            JObject newSection = new JObject
            {
                { "typeOfSection", section.Value<int>("typeOfSection") },
                { "lengthInSteps", section.Value<int>("lengthInSteps") },
                { "sectionNotes", newSectionNotes },
                { "mustHitSection", section.Value<bool>("mustHitSection") }
            };
            
            newNotesData.Add(newSection);
        }
        data["song"]["notes"] = newNotesData;
        return data.ToString();
    }
}
