namespace Api.Services;

public class CsvParserService
{
    // insideQuotes:
    // - character is ", nextCharacter is " => add character to currentField, i++ (skip)
    // - character is ", nextCharacter is NOT " => set insideQuotes to false
    // - else => add character to currentField
    // !insideQuotes:
    // - character is " => set insideQuotes to true
    // - character is , => add currentField to list and reset it (string.Empty)
    // - else => add character to currentField
    private static List<string> ParseLine(string line)
    {
        List<string> list = [];

        bool insideQuotes = false;
        string currentField = string.Empty;

        for (int i = 0; i < line.Length; i++)
        {
            char character = line[i];

            if (insideQuotes)
            {
                if (character == '"')
                {
                    if (i + 1 < line.Length && line[i + 1] == '"')
                    {
                        currentField += character;
                        i++;
                    }
                    else
                        insideQuotes = false;
                }
                else
                    currentField += character;
            }
            else
            {
                if (character == '"')
                    insideQuotes = true;
                else if (character == ',')
                {
                    list.Add(currentField);
                    currentField = string.Empty;
                }
                else
                    currentField += character;
            }
        }

        list.Add(currentField);

        return list;
    }

    public ParseResult Parse(string content)
    {
        string[] lines = content.Split("\n");

        List<string>? headers = null;
        List<Dictionary<string, string>> rows = [];

        foreach (string line in lines)
        {
            string trimmedLine = line.TrimEnd('\r');
            if (string.IsNullOrWhiteSpace(trimmedLine))
                continue;

            if (headers == null)
                headers = ParseLine(trimmedLine);
            else
            {
                List<string> values = ParseLine(trimmedLine);
                Dictionary<string, string> row = [];

                for (int i = 0; i < headers.Count; i++)
                {
                    if (i < values.Count)
                        row[headers[i]] = values[i];
                    else
                        row[headers[i]] = "";
                }

                rows.Add(row);
            }
        }

        return new ParseResult(rows.Count, rows);
    }
}