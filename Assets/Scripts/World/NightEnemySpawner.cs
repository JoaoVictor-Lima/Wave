using UnityEngine;

namespace World
{
    public class NightEnemySpawner : MonoBehaviour
    {
        [Header("References")]
        public Transform player;
        public GameObject enemyPrefab;

        [Header("Spawn Settings")]
        public float spawnRadius = 15f;
        public int maxEnemies = 5;
        public float spawnInterval = 3f;

        private float spawnTimer;
        private int currentEnemies;

        private void Update()
        {
            if (!WorldManager.Instance.IsNight())
            {
                currentEnemies = 0;
                spawnTimer = 0f;
                return;
            }

            spawnTimer += Time.deltaTime;

            if (spawnTimer >= spawnInterval && currentEnemies < maxEnemies)
            {
                SpawnEnemy();
                spawnTimer = 0f;
            }
        }

        private void SpawnEnemy()
        {
            Vector3 randomPos = GetRandomPositionAroundPlayer();
            Instantiate(enemyPrefab, randomPos, Quaternion.identity);
            currentEnemies++;
        }

        private Vector3 GetRandomPositionAroundPlayer()
        {
            Vector2 randomCircle = Random.insideUnitCircle.normalized * spawnRadius;
            return player.position + new Vector3(randomCircle.x, 0f, randomCircle.y);
        }
    }
}
