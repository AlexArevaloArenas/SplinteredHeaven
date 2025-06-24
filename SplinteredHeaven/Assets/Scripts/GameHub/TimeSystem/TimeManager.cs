using System;
using Unity.VisualScripting;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public const int hoursInDay = 24, minutesInHour = 60;
    const float hoursToDegrees = 360f / 12, minutesToDegrees = 360f / 60;
    public float dayDuration = 30f; // Duration of a day in seconds


    public float startTime;
    public float maxDayTime = 86400f;

    public WeekDays currentDay = WeekDays.Monday; // Default day
    public int currentDayNumber = 1;

    public WeekDays[] week;
    public int currentWeekDay = 0; // Default week day
    public int currentWeek = 0; // Default week

    public DayMoments currentMoment = DayMoments.Morning; // Default moment of the day
    public DayMoments previousMoment = DayMoments.Morning;

    private float totalTime; // Total time in seconds
    private float dayTime; // Time of the day in seconds
    private float currentTime; // Current time of the day
    

    public TMPro.TextMeshProUGUI timeText; // Reference to the UI text element
    public TMPro.TextMeshProUGUI dayText;
    public TMPro.TextMeshProUGUI momentText;
    public GameObject timePanel; // Reference to the UI panel
    public RectTransform clockHand;
    public float secondsPerCycle = 60f; // Full rotation per 60 seconds (like a clock)

    public static TimeManager Instance;

    //Day time Events
    public event Action<DayMoments> OnDayTimeChanged;
    public event Action<int> OnDayChange;

    public bool StopTime = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeText = GameObject.Find("TimeText").GetComponent<TMPro.TextMeshProUGUI>();
        week = (WeekDays[])Enum.GetValues(typeof(WeekDays));
        dayText = GameObject.Find("DayText").GetComponent<TMPro.TextMeshProUGUI>();
        dayText.text = week[(int)currentDay].ToString();
        momentText = GameObject.Find("MomentText").GetComponent<TMPro.TextMeshProUGUI>();

        FindDayMoment();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateClock();
        timeText.text = string.Format("{0:D2}:{1:D2}", Mathf.FloorToInt(GetHour()), Mathf.FloorToInt(GetMinute()));
        MoveClockHand();
    }

    public void UpdateClock()
    {
        if(StopTime) return; // If time is stopped, do not update

        totalTime += Time.deltaTime;
        dayTime += Time.deltaTime;
        currentTime =  totalTime % dayDuration;
        if (dayTime > dayDuration)
        {
            dayTime = 0f;
            currentWeekDay++;
            currentDayNumber++;
            OnDayChange?.Invoke(currentDayNumber);
            if (currentWeekDay >= week.Length)
            {
                currentWeekDay = 0; // Reset to the first week
                currentWeek++;
            }
            dayText.text = week[currentWeekDay].ToString();
        }
        FindDayMoment();
        momentText.text = currentMoment.ToString();
    }
    public bool AdvanceTime(float time)
    {
        if(currentTime + time > dayDuration)
        {
            return false; // Time exceeds 24 hours
        }
        currentTime += time;
        return true; // Time successfully advanced
    }

    public void MoveClockHand()
    {
        clockHand.rotation = Quaternion.Euler(0, 0, -GetHour()*hoursToDegrees); // Z-axis rotation
    }

    private float GetHour()
    {
        return currentTime * hoursInDay / dayDuration;
    }

    private float GetMinute()
    {
        return (currentTime * hoursInDay  * minutesInHour / dayDuration) % minutesInHour;
    }

    private void FindDayMoment()
    {
        if (GetHour() > 21)
        {
            currentMoment = DayMoments.Night;
            if (previousMoment != DayMoments.Night)
            {
                OnDayTimeChanged?.Invoke(currentMoment);
                Debug.Log(currentMoment.ToString());
                previousMoment = currentMoment;
            }
        }
        else if (GetHour() > 18)
        {
            currentMoment = DayMoments.Evening;
            if (previousMoment != DayMoments.Evening)
            {
                OnDayTimeChanged?.Invoke(currentMoment);
                Debug.Log(currentMoment.ToString());
                previousMoment = currentMoment;
            }
        }
        else if (GetHour() > 12)
        {
            currentMoment = DayMoments.Afternoon;
            if (previousMoment != DayMoments.Afternoon)
            {
                OnDayTimeChanged?.Invoke(currentMoment);
                Debug.Log(currentMoment.ToString());
                previousMoment = currentMoment;
            }
        }
        else if (GetHour() > 6)
        {
            currentMoment = DayMoments.Morning;
            if (previousMoment != DayMoments.Morning)
            {
                OnDayTimeChanged?.Invoke(currentMoment);
                Debug.Log(currentMoment.ToString());
                previousMoment = currentMoment;
            }
        }
        else
        {
            currentMoment = DayMoments.Midnight;
            if (previousMoment != DayMoments.Midnight)
            {
                OnDayTimeChanged?.Invoke(currentMoment);
                Debug.Log(currentMoment.ToString());
                previousMoment = currentMoment;
            }
        }
        
    }

    public void Stop()
    {
        StopTime = true;
    }
    public void Play()
    {
        StopTime = false;
    }

}

public enum WeekDays
{
    Monday,
    Tuesday,
    Wednesday,
    Thursday,
    Friday,
    Saturday,
    Sunday
}

public enum DayMoments
{
    Morning,
    Afternoon,
    Evening,
    Night,
    Midnight,
}
