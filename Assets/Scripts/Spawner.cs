using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    #region PARAMETERS

    [SerializeField]
    private MovingObject[] spawnPrefabs;
    [SerializeField]
    private float spawnRateMin = 1f;
    [SerializeField]
    private float spawnRateMax = 3f;


    [SerializeField]
    protected float minMoveSpeed;
    [SerializeField]
    protected float maxMoveSpeed;


    #endregion

    #region CACHES



    #endregion

    #region STATES

    private bool isActive = false;
    private float currentCooldown;
    private MovingObject currentSpawnPrefab;
    private float currentMoveSpeed;

    private Queue<MovingObject> pool = new Queue<MovingObject>();

    #endregion

    private void Start()
    {
        currentSpawnPrefab = spawnPrefabs[UnityEngine.Random.Range(0, spawnPrefabs.Length)];
        currentMoveSpeed = UnityEngine.Random.Range(minMoveSpeed, maxMoveSpeed);
        isActive = true;
        currentCooldown = 0;

        CreatePool();
    }

    private void Update()
    {
        if (!isActive) { return; }
        currentCooldown -= Time.deltaTime;
        if(currentCooldown <= 0)
        {
            InstantiateFromPool();
        }
    }

    private void InstantiateFromPool()
    {
        if(pool.Count == 0)
        {
            CreateNewElement();
        }
        MovingObject movingObject = pool.Dequeue();
        movingObject.transform.position = new Vector3(transform.position.x, movingObject.transform.position.y, (movingObject.transform.position.z));
        movingObject.transform.rotation = transform.rotation;
        movingObject.MoveSpeed = currentMoveSpeed;
        movingObject.Lifetime = 50f;
        movingObject.gameObject.SetActive(true);
        currentCooldown = UnityEngine.Random.Range(spawnRateMin, spawnRateMax);
    }

    private void CreateNewElement()
    {
        MovingObject element = Instantiate(currentSpawnPrefab, transform, false);
        element.gameObject.SetActive(false);
        element.Spawner = this;
        pool.Enqueue(element);
    }

    private void CreatePool()
    {
        for(int i = 0; i < 10; i++)
        {
            MovingObject element = Instantiate(currentSpawnPrefab, transform);
            element.gameObject.SetActive(false);
            element.Spawner = this;
            pool.Enqueue(element);
        }
    }

    public void Enqueue(MovingObject element)
    {
        element.gameObject.SetActive(false);
        pool.Enqueue(element);
    }
}
