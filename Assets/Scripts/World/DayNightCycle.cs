using System;
using UnityEngine;

namespace World
{
    public class DayNightCycle : MonoBehaviour
    {
        [Header("Time Settings")]
        public float dayDuration = 120f;

        [Range(0f, 1f)]
        public float timeOfDay = 0f;

        [Header("References")]
        public Light sun;

        public event Action OnDayPassed;

        private void Update()
        {
            if (sun == null) return;

            timeOfDay += Time.deltaTime / dayDuration;

            if (timeOfDay >= 1f)
            {
                timeOfDay = 0f;
                OnDayPassed?.Invoke();
            }

            UpdateSun();
        }

        private void UpdateSun()
        {
            float sunAngle = timeOfDay * 360f;
            sun.transform.rotation = Quaternion.Euler(sunAngle - 90f, 170f, 0f);
        }
    }
}
