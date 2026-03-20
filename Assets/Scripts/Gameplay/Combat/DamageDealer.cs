using Assets.Scripts.Gameplay.Interact;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    private Collider _collider;
    private float _currentPower;
    private Capability[] _currentCapabilities;
    private GameObject _owner;

    private readonly HashSet<IInteractable> _hitTargets = new();

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;
        Disarm();
    }

    public void Arm(float power, Capability[] caps, GameObject owner)
    {
        _currentPower = power;
        _currentCapabilities = caps;
        _owner = owner;
        _collider.enabled = true;
    }

    public void Disarm()
    {
        _collider.enabled = false;
        _hitTargets.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<IInteractable>(out var interactable))
            return;

        if (_hitTargets.Contains(interactable))
            return;

        InteractionContext context = new InteractionContext();
        context.Type = InteractionType.Impact;
        context.Power = _currentPower;
        context.Capabilities = _currentCapabilities;
        context.Source = _owner;

        interactable.Interact(context);

        _hitTargets.Add(interactable);
    }
}