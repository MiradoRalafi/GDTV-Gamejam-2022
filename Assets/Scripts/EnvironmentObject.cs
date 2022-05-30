using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentObject : MonoBehaviour
{
    #region PARAMETERS



    #endregion

    #region CACHES

    protected PlayerController player;
    protected MeshRenderer[] renderers;
    protected float renderDistanceLimit;

    #endregion

    #region STATES

    protected bool rendered = true;

    #endregion

    protected virtual void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        renderers = GetComponentsInChildren<MeshRenderer>();
    }

    protected virtual void Update()
    {
        if(!player) { return; }
        float distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
        if(rendered && distanceFromPlayer > renderDistanceLimit)
        {
            foreach(MeshRenderer renderer in renderers)
            {
                renderer.enabled = false;
            }
            rendered = false;
        }
        else if(!rendered && distanceFromPlayer <= renderDistanceLimit)
        {
            foreach (MeshRenderer renderer in renderers)
            {
                renderer.enabled = true;
            }
            rendered = true;
        }
    }
}
