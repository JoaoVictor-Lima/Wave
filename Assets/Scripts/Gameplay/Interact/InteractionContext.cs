using UnityEngine;

namespace Assets.Scripts.Gameplay.Interact
{
    public struct InteractionContext
    {
        public InteractionType Type;
        public float Power;
        public Capability[] Capabilities;
        public GameObject Source;
    }
}
