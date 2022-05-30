using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenSpawner : MonoBehaviour
{
    #region PARAMETERS

    [SerializeField]
    private ChickenBehavior chickenPrefab;
    [SerializeField]
    private float cooldown = 8.5f;
    [SerializeField]
    private LayerMask spawnableLayer;

    #endregion

    #region CACHES

    private PlayerController player;

    #endregion

    #region STATES

    private float currentCooldown;

    #endregion

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
    }

    private void Start()
    {
        currentCooldown = cooldown;
    }

    private void Update()
    {
        if(currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
            return;
        }
        else
        {
            StartCoroutine(SpawnChicken());
            currentCooldown = cooldown;
        }
    }

    private IEnumerator SpawnChicken()
    {
        bool canSpawn = false;
        Vector3 spawnPoint = default;
        while (!canSpawn)
        {
            Vector3 direction = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(0f, 1f));
            spawnPoint = player.transform.position + direction * Random.Range(15, 25);
            RaycastHit hit;
            if(Physics.Raycast(spawnPoint + Vector3.up, Vector3.down, out hit, 3, spawnableLayer))
            {
                canSpawn = true;
            }
            Debug.DrawRay(spawnPoint + Vector3.up, Vector3.down * 3, Color.red, 5);
            yield return null;
        }
        Instantiate(chickenPrefab, spawnPoint, Quaternion.identity);
    }
}
