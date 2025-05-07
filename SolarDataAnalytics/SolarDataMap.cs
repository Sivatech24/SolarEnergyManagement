using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using SolarEnergyAnalytics;

public class SolarDataMap : ClassMap<SolarData>
{
    public SolarDataMap()
    {
        // Map CSV headers to class properties
        Map(m => m.Date).Name("DATE_TIME").TypeConverterOption.Format("dd-MM-yyyy HH:mm");
        Map(m => m.PlantId).Name("PLANT_ID");
        Map(m => m.SourceKey).Name("SOURCE_KEY");
        Map(m => m.DcPower).Name("DC_POWER");
        Map(m => m.AcPower).Name("AC_POWER");
        Map(m => m.DailyYield).Name("DAILY_YIELD");
        Map(m => m.TotalYield).Name("TOTAL_YIELD");
    }
}
