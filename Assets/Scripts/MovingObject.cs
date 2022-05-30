using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : EnvironmentObject
{
    #region PARAMETERS

    [SerializeField]
    protected bool useVelocity = false;

    #endregion

    #region CACHES

    Rigidbody rb;

    #endregion

    #region STATES

    //[HideInInspector]
    public float MoveSpeed;
    //[HideInInspector]
    public float Lifetime;
    [HideInInspector]
    public Spawner Spawner;

    #endregion

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
    }

    protected void Start()
    {
        renderDistanceLimit = GameManager.Instance.MovingObjectRenderDistanceLimit;
    }

    protected override void Update()
    {
        base.Update();
        if(!useVelocity)
        {
            transform.position += transform.right * MoveSpeed * Time.deltaTime;
        }
        Lifetime -= Time.deltaTime;
        if(Lifetime <= 0)
        {
            Spawner.Enqueue(this);
        }
    }

    protected void FixedUpdate()
    {
        if (!useVelocity) { return; }
        //rb.velocity = transform.right * MoveSpeed;
        rb.MovePosition(transform.position + transform.right * MoveSpeed * Time.deltaTime);
    }
}
