namespace LogsParser;

internal class Worker1 : Worker
{
    internal static IEnumerable<GclidVisit> GetGeneralVisits(IEnumerable<GclidVisit> visits1,
        IEnumerable<GclidVisit> visits2)
    {
        var visitsArray1 = visits1 as GclidVisit[] ?? visits1.ToArray();
        var visitsArray2 = visits2 as GclidVisit[] ?? visits2.ToArray();
        var generalIps = visitsArray1.Select(v => v.Ip).Intersect(visitsArray2.Select(v => v.Ip)).ToArray();
        var generalVisits = visitsArray1.Concat(visitsArray2).Where(v => generalIps.Contains(v.Ip));
        return generalVisits;
    }

    internal override void WriteData(string filePath, IEnumerable<GclidVisit> visits)
    {
        var data = string.Empty;

        foreach (var date in visits.GroupBy(v => v.Date).OrderBy(v => v.Key))
        {
            data += date.Key + "\n";
            foreach (var ip in date.GroupBy(d => d.Ip).OrderBy(d => d.Key))
            {
                data += "\t\t" + ip.Key + "\n";
                data = ip.OrderBy(i => i.Time).Aggregate(data,
                    (current, time) => current + "\t\t\t\t" + time.Time + "\t" + time.SiteName + "\n");
            }
        }

        File.WriteAllText(filePath, data);
    }
}