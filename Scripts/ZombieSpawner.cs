using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;  // Prefab del zombie
    public Transform player;  // Referencia al jugador
    public float spawnRadius = 10f;  // Radio alrededor del jugador donde aparecerán los zombies
    public float spawnInterval = 10f;  // Tiempo entre cada spawn
    public float minSpawnInterval = 5f;  // Límite mínimo para evitar spawns demasiado rápidos
    public float spawnReductionRate = 1f;  // Cantidad en segundos que se reducirá por cada spawn
    public LayerMask groundLayer;  // Capa del suelo para validación del spawn

    void Start()
    {
        StartCoroutine(SpawnZombies());
    }

    IEnumerator SpawnZombies()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            Vector3 spawnPosition;
            if (GetSpawnPosition(out spawnPosition))
            {
                Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
                Debug.Log("¡Zombie generado en posición válida!");
            }
            else
            {
                Debug.Log("No se encontró una posición válida para el zombie.");
            }

            // Reducir tiempo de spawn para aumentar la dificultad
            spawnInterval = Mathf.Max(minSpawnInterval, spawnInterval - spawnReductionRate);
        }
    }

    bool GetSpawnPosition(out Vector3 spawnPosition)
    {
        for (int i = 0; i < 10; i++)  // Intentar encontrar una posición válida hasta 10 veces
        {
            Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;
            randomDirection.y = 0;  // Mantener la posición en el suelo
            Vector3 potentialPosition = player.position + randomDirection;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(potentialPosition, out hit, 2f, NavMesh.AllAreas))
            {
                spawnPosition = hit.position;
                return true;
            }
        }

        spawnPosition = Vector3.zero;
        return false;
    }
}
