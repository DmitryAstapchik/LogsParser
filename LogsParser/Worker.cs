using System.Text.RegularExpressions;

namespace LogsParser;

internal abstract partial class Worker
{
    protected static GclidVisit ParseGclidVisit(string line, string? siteName)
    {
        var ip = Ipv4Regex().Match(line).Value;
        var dateTime = DateTimeRegex().Match(line).Value;
        var date = DateOnly.Parse(dateTime.Substring(1, 11));
        var time = TimeOnly.Parse(dateTime.Substring(13, 8));
        return new GclidVisit
        {
            Ip = ip,
            Date = date,
            Time = time,
            SiteName = siteName
        };
    }

    internal IEnumerable<GclidVisit> GetGclidVisits(string filePath, string? siteName)
    {
        var regex = GclidVisitRegex();
        var visits = regex.Matches(File.ReadAllText(filePath)).Select(m => ParseGclidVisit(m.Value, siteName));
        return visits;
    }

    internal abstract void WriteData(string filePath, IEnumerable<GclidVisit> visits);

    [GeneratedRegex("\\[.+\\]")]
    private static partial Regex DateTimeRegex();

    [GeneratedRegex(@"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b")]
    private static partial Regex Ipv4Regex();

    [GeneratedRegex(@"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b - - \[.+\](?=.+gclid=)", RegexOptions.Multiline)]
    private static partial Regex GclidVisitRegex();
}