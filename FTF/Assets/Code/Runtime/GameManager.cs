using UnityEngine;
using System;
using System.Collections.Generic;
public class GameManager : MonoBehaviour
{
    public static GameManager GM { get; private set; }
    public Seasons season { get; private set; }
    public Months month { get; private set; }
    public Intensity weatherIntensity { get; private set; }

    static readonly int MonthsCount = Enum.GetValues(typeof(Months)).Length;
    SeasonsWeatherManager seasonsWeatherManager;
    public InputManager inputManager { get; private set; }
    [field: SerializeField] public PlayerManager playerManager { get; private set; }

    [Header("SeasonsWeatherProperties")]
    //[Tooltip("Percentage of repartition for each season to change, \n Seasons Order : Winter, Spring, Summer, Autumn")]
    //public Vector4 SeasonsMonthsRepartition = new(0.25f, 0.25f, 0.25f, 0.25f);
    public float MonthsDuration = 4;
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
        seasonsWeatherManager = new();
        seasonsWeatherManager.Initialize();
        inputManager = new();
        inputManager.Initialize();
        playerManager.Initialize();
    }

    private void OnEnable()
    {
        if (GM != this)
            return;

        inputManager.ClickAction.performed += inputManager.SetClick;
        inputManager.ScrollAction.performed += inputManager.SetScroll;
        inputManager.PointAction.performed += inputManager.SetMousePosition;
        inputManager.ExitAction.performed += inputManager.SetExit;
        inputManager.ExitAction.canceled += inputManager.SetExit;
    }

    private void OnDisable()
    {
        if (GM != this)
            return;

        inputManager.ClickAction.performed -= inputManager.SetClick;
        inputManager.ScrollAction.performed -= inputManager.SetScroll;
        inputManager.PointAction.performed -= inputManager.SetMousePosition;
        inputManager.ExitAction.performed -= inputManager.SetExit;
        inputManager.ExitAction.canceled -= inputManager.SetExit;
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
        float yearDuration = ActionUnitDuration * MonthsDuration * MonthsCount;
        if (Globaltimer >= yearDuration)
            Globaltimer -= yearDuration;

        seasonsWeatherManager.SeasonsMonthsUpdate();
        seasonsWeatherManager.WeatherUpdate();
        playerManager.PlayerUpdate();
    }

    #region ModifProperties
    public void SetGlobaltimer(float newTimer) => Globaltimer = newTimer;

    public void SetSeason(Seasons newSeason) => season = newSeason;
    
    public void SetMonth(Months newMonth) => month = newMonth;
    
    public void SetWeatherIntensity(Intensity newIntensity) => weatherIntensity = newIntensity;
    
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

