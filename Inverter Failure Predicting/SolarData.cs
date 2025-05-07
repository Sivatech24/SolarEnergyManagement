using System;
public class InverterData
{
    public DateTime DATE_TIME { get; set; }
    public int PLANT_ID { get; set; }
    public string SOURCE_KEY { get; set; }
    public double DC_POWER { get; set; }
    public double AC_POWER { get; set; }
    public double DAILY_YIELD { get; set; }
    public double TOTAL_YIELD { get; set; }
    public string FailureReason { get; set; }
}
