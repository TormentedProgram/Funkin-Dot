public partial class CoolUtil : Node
{
    public static float roundAccuracy(float input) {
        float roundedValue = Mathf.Round(input * 10000) / 10000f;
        return Mathf.Clamp(roundedValue * 100, 0f, 100f);
    }

    public static string formatMemory(int num)
    {
        float size = num;
        var data = 0;
        string[] dataTexts = new string[] { "B", "KB", "MB", "GB" };
        while (size > 1024 && data < dataTexts.Length - 1)
        {
            data++;
            size = size / 1024;
        }

        size = Mathf.Round(size * 100) / 100;
        string formatSize = formatAccuracy(size);
        return $"{formatSize} {dataTexts[data]}";
    }

    public static string formatAccuracy(float value)
    {
        string stringVal = value.ToString("0.##"); // Limit decimal places to two
        string converVal = stringVal.Replace(".", ""); // Remove decimal point for count

        Dictionary<string, string> conversion = new Dictionary<string, string>
        {
            {"0", "0.00"},
            {"00", "00.00"},
            {"000", "000.00"}
        };

        string wantedConversion = conversion.ContainsKey(converVal.PadLeft(3, '0')) ? conversion[converVal.PadLeft(3, '0')] : string.Empty;

        string convertedValue = wantedConversion.PadRight(stringVal.Length, '0');

        // Check if the convertedValue is empty or only contains zeros after formatting
        bool containsNonZero = convertedValue.Trim('0').Length > 0;

        if (!containsNonZero)
            return stringVal; // Return the original value if the conversion yields only zeros

        return convertedValue.Insert(stringVal.IndexOf('.') + 1, "."); // Reinsert decimal point
    }

    public static float BoundTo(float value, float min, float max)
    {
        return Mathf.Max(min, Mathf.Min(max, value));
    }
}
