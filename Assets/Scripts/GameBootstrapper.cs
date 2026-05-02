using Assets.Scripts.UI;
using UnityEngine;

public class GameBootstrapper : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private HUDController hud;

    private void Start()
    {
        hud.Initialize(player);
    }
}
