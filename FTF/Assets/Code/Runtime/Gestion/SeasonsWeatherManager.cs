using System;
using static GameManager;

public class SeasonsWeatherManager
{
    public void Initialize()
    {
        GM.SeasonsMonthsRepartitionStatic = new()
        {
            [Seasons.Winter] = new Months[] { Months.Dec, Months.Jan, Months.Feb },
            [Seasons.Spring] = new Months[] { Months.Mar, Months.Apr, Months.May },
            [Seasons.Summer] = new Months[] { Months.Jun, Months.Jul, Months.Aug },
            [Seasons.Autumn] = new Months[] { Months.Sep, Months.Oct, Months.Nov },
        };

        GM.SetGlobaltimer((int)Months.Mar * GM.ActionUnitDuration * GM.MonthsDuration - 15); // 15 seconds of winter before spring
        GM.SetWeatherIntensity(Intensity.Calm);
        SeasonsMonthsUpdate();
    }

    public void SeasonsMonthsUpdate()
    {
        Months newMonth = (Months)(int)(GM.Globaltimer / (GM.ActionUnitDuration * GM.MonthsDuration));
        if (newMonth == GM.month)
            return;

        GM.SetMonth(newMonth);
        foreach (var (newSeason, months) in GM.SeasonsMonthsRepartitionStatic)
            if (Array.IndexOf(months, newMonth) >= 0)
                GM.SetSeason(newSeason);
    }

    public void WeatherUpdate()
    {

    }
}
