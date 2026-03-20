using TMPro;
using UnityEngine;
using World;

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance { get; private set; }

    [Header("References")]
    public DayNightCycle dayNightCycle;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI dayText;

    [Header("World State")]
    public WorldStateEnum CurrentState { get; private set; }

    private int currentDay = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        UpdateDayUI();
    }

    private void OnEnable()
    {
        if (dayNightCycle != null)
            dayNightCycle.OnDayPassed += AdvanceDay;
    }

    private void OnDisable()
    {
        if (dayNightCycle != null)
            dayNightCycle.OnDayPassed -= AdvanceDay;
    }

    private void Update()
    {
        UpdateWorldState();
    }

    private void UpdateWorldState()
    {
        if (dayNightCycle == null) return;

        WorldStateEnum newState;

        if (dayNightCycle.timeOfDay < 0.25f)
            newState = WorldStateEnum.Morning;
        else if (dayNightCycle.timeOfDay < 0.5f)
            newState = WorldStateEnum.Day;
        else
            newState = WorldStateEnum.Night;

        if (newState != CurrentState)
        {
            CurrentState = newState;
            OnWorldStateChanged(newState);
        }
    }

    private void OnWorldStateChanged(WorldStateEnum newState)
    {
        Debug.Log($"🌍 World State changed to: {newState}");
    }

    private void AdvanceDay()
    {
        currentDay++;
        UpdateDayUI();

        Debug.Log($"📅 Novo dia: {currentDay}");
    }

    private void UpdateDayUI()
    {
        if (dayText != null)
            dayText.text = $"Dia {currentDay}";
    }

    public bool IsNight() => CurrentState == WorldStateEnum.Night;
}
