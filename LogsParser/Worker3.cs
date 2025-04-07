namespace LogsParser;

internal class Worker3 : Worker
{
    internal static IEnumerable<GclidVisit> GetSameDayVisits(IEnumerable<GclidVisit> visits1,
        IEnumerable<GclidVisit> visits2)
    {
        return visits1.Where(v1 => visits2.Any(v2 => v2.Date == v1.Date && v2.Ip == v1.Ip));
    }

    internal override void WriteData(string filePath, IEnumerable<GclidVisit> visits)
    {
        var data = string.Empty;

        foreach (var date in visits.GroupBy(v => v.Date).OrderBy(v => v.Key))
        {
            data += date.Key + "\n";
            data = date.DistinctBy(d => d.Ip).OrderBy(d => d.Ip)
                .Aggregate(data, (current, visit) => current + ("\t\t" + visit.Ip + '\n'));
        }

        File.WriteAllText(filePath, data);
    }
}