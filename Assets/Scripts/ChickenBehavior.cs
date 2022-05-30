using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChickenBehavior : MonoBehaviour
{
    #region PARAMETERS

    [SerializeField]
    private float minTimeBeforeMove = 2f;
    [SerializeField]
    private float maxTimeBeforeMove = 10f;

    [SerializeField]
    private float minMoveDistance = 1f;
    [SerializeField]
    private float maxMoveDistance = 5f;

    [SerializeField]
    private float distanceBeforeRun = 3f;

    [SerializeField]
    private float minRunDistance = 5f;
    [SerializeField]
    private float maxRunDistance = 10f;

    [SerializeField]
    private AudioClip[] deathClips;

    [SerializeField]
    private Brain brainPrefab;

    #endregion

    #region CACHES

    private NavMeshAgent agent;
    private PlayerController player;
    private AudioSource source;

    #endregion

    #region STATES

    private bool moving;
    private bool running;
    private float timeBeforeMove;

    #endregion

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<PlayerController>();
        source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        timeBeforeMove = Random.Range(minTimeBeforeMove, maxTimeBeforeMove);
    }

    private void Update()
    {
        if (!moving && !running)
        {
            timeBeforeMove -= Time.deltaTime;
            if(timeBeforeMove <= 0)
            {
                float randomX = Random.Range(-1f, 1f);
                float randomZ = Random.Range(-1f, 1f);
                Vector3 randomDirection = new Vector3(randomX, 0, randomZ).normalized;
                agent.destination = transform.position + randomDirection * Random.Range(minMoveDistance, maxMoveDistance);
                moving = true;
            }
        }
        if (Vector3.Distance(transform.position, player.transform.position) <= distanceBeforeRun)
        {
            moving = false;
            running = true;
            Vector3 destinationVector = (transform.position - player.transform.position);
            destinationVector = new Vector3(destinationVector.x, 0, destinationVector.z).normalized;
            agent.destination = new Vector3(transform.position.x, 0, transform.position.z) + destinationVector * Random.Range(minRunDistance, maxRunDistance);
        }
        if (moving || running)
        {
            if (Vector3.Distance(transform.position, agent.destination) < .1f)
            {
                moving = false;
                running = false;
                timeBeforeMove = Random.Range(minTimeBeforeMove, maxTimeBeforeMove);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponent<Car>())
        {
            Die();
        }
    }

    private void Die()
    {
        source.PlayOneShot(deathClips[Random.Range(0, deathClips.Length)]);
        Collider[] colliders = GetComponentsInChildren<Collider>();
        SkinnedMeshRenderer[] renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach(Collider collider in colliders)
        {
            collider.enabled = false;
        }
        foreach(SkinnedMeshRenderer renderer in renderers)
        {
            renderer.enabled = false;
        }
        Instantiate(brainPrefab, new Vector3(transform.position.x, .65f, transform.position.z), Quaternion.identity);
        Destroy(gameObject, 1.5f);
    }
}
