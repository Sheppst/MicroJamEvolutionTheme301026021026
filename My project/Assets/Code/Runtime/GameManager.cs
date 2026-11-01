using UnityEngine;
using System;
using System.Collections.Generic;
public class GameManager : MonoBehaviour
{
    GameManager GM;
    public Seasons season { get; private set; }
    public Months month { get; private set; }
    public Intensty weatherIntensty { get; private set; }

    [Header("SeasonsWeatherProperties")]
    //[Tooltip("Percentage of repartition for each season to change, \n Seasons Order : Winter, Spring, Summer, Autumn")]
    //public Vector4 SeasonsMonthsRepartition = new(0.25f, 0.25f, 0.25f, 0.25f);
    public float MonthsDuration;
    public float ActionUnitDuration = 30;

    public float Globaltimer { get; private set; }
    public Dictionary<Seasons, Months[]> SeasonsMonthsRepartitionStatic;

    //Vector4 SeasMontReparValid = new(0.25f, 0.25f, 0.25f, 0.25f);

    private void Awake()
    {
        if (GM)
        {
            Destroy(gameObject);
            return;
        }
        GM = this;
        SeasonsInitialize();
    }

    private void OnValidate()
    {
        //if (SeasonsMonthsRepartition.x + SeasonsMonthsRepartition.y + SeasonsMonthsRepartition.z + SeasonsMonthsRepartition.w != 1)
        //    SeasonsMonthsRepartition = SeasMontReparValid;
        //SeasMontReparValid = SeasonsMonthsRepartition;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Globaltimer += Time.deltaTime;
        WeatherSeasonsUpdate();
    }


    #region WeatherSeasons

    void SeasonsInitialize()
    {
        SeasonsMonthsRepartitionStatic = new()
        {
            [Seasons.Winter] = new Months[] { Months.Dec, Months.Jan, Months.Feb },
            [Seasons.Spring] = new Months[] { Months.Mar, Months.Apr, Months.May },
            [Seasons.Summer] = new Months[] { Months.Jun, Months.Jul, Months.Aug },
            [Seasons.Autumn] = new Months[] { Months.Sep, Months.Oct, Months.Nov },
        };

        season = Seasons.Winter;
        month = Months.Feb;
        weatherIntensty = Intensty.Calm;
        Globaltimer = 50; // Like the player can see that he was in winter and after 5 second leave it
    }

    void WeatherSeasonsUpdate()
    {
        SeasonsUp();
        WeatherUp();
    }

    void SeasonsUp()
    {
        if (Globaltimer > (int)month * ActionUnitDuration + ActionUnitDuration)
        {

        }
    }

    void WeatherUp()
    {

    }


    #endregion

    #region ModifProperties
    public void SetSeason(Seasons newSeason) => season = newSeason;
    
    public void SetMonth(Months newMonth) => month = newMonth;
    
    public void SetWeatherIntensity(Intensty newIntensity) => weatherIntensty = newIntensity;
    
    #endregion

    //#region Logic
    //int GeIterWithQuantityPercentage(int iterElement)
    //{
    //    float i = iterElement;
    //    int L = Enum.GetValues(typeof(Months)).Length;

    //    return (int)i;
    //}
    //#endregion
}

