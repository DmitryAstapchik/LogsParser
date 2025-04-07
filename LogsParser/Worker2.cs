namespace LogsParser;

internal class Worker2 : Worker
{
    internal override void WriteData(string filePath, IEnumerable<GclidVisit> visits)
    {
        var data = string.Empty;

        var visitsArray = visits as GclidVisit[] ?? visits.ToArray();
        foreach (var ip in visitsArray.GroupBy(v => v.Ip).OrderBy(v => v.Key))
        {
            data += ip.Key + "\n";
            data = ip.GroupBy(i => i.Date).OrderBy(i => i.Key).Aggregate(data,
                (current, date) => current + "\t\t" + date.Key + "\t\t" + date.Count() + "\n");
        }

        File.WriteAllText(filePath, data);
        WriteGeneralData(filePath, visitsArray);
    }

    internal static void WriteDataGroupedByDate(string filePath, IEnumerable<GclidVisit> visits)
    {
        var data = string.Empty;

        var visitsArray = visits as GclidVisit[] ?? visits.ToArray();
        foreach (var date in visitsArray.GroupBy(v => v.Date).OrderBy(v => v.Key))
        {
            data += date.Key + "\n";
            data = date.OrderBy(d => d.Ip).GroupBy(d => d.Ip).OrderByDescending(d => d.Count()).Aggregate(data,
                (current, ip) => current + ("\t\t" + ip.Key + "\t\t" + ip.Count() + '\n'));
        }

        File.WriteAllText(filePath, data);
        WriteGeneralData(filePath, visitsArray);
    }

    private static void WriteGeneralData(string filePath, IEnumerable<GclidVisit> visits)
    {
        var data = string.Concat(Enumerable.Repeat('_', 50)) + '\n';
        var dict = new Dictionary<string, int>();

        foreach (var ip in visits.GroupBy(v => v.Ip))
            if (ip.Key != null)
                dict.Add(ip.Key, ip.Count());

        data = dict.OrderBy(kvp => kvp.Key).ThenByDescending(kvp => kvp.Value)
            .Aggregate(data, (current, kvp) => current + (kvp.Value + "\t" + kvp.Key + '\n'));

        File.AppendAllText(filePath, data);
    }
}