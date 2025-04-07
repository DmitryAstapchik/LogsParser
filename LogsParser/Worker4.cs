using System.Text.RegularExpressions;

namespace LogsParser;

internal class Worker4 : Worker
{
    internal static IEnumerable<GclidVisit> GetGclidVisitsByIp(string filePath, string ip)
    {
        Regex gclidVisitFromIp = new(@$"^{ip}\b - - \[.+\](?=.+gclid=)", RegexOptions.Multiline);
        var visits = gclidVisitFromIp.Matches(File.ReadAllText(filePath)).Select(m => ParseGclidVisit(m.Value, null));
        return visits;
    }

    internal override void WriteData(string filePath, IEnumerable<GclidVisit> visits)
    {
        var data = string.Empty;

        foreach (var date in visits.GroupBy(v => v.Date).OrderBy(v => v.Key))
        {
            data += date.Key + "\n";
            data = date.OrderBy(d => d.Ip)
                .Aggregate(data, (current, ip) => current + "\t\t" + ip.Ip + "\t\t" + ip.Time + "\n");
        }

        File.WriteAllText(filePath, data);
    }
}