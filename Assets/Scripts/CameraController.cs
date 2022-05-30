using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region PARAMETERS

    [SerializeField]
    private Transform target;
    [SerializeField]
    private LayerMask nonPlayerLayer;

    #endregion

    #region CACHES


    #endregion

    #region STATES

    private List<Transform> modifiedRenderers = new List<Transform>();

    #endregion

    private void Update()
    {
        RaycastHit[] inBetween = Physics.RaycastAll(transform.position, target.position - transform.position, Vector3.Distance(transform.position, target.position) * .9f, nonPlayerLayer);
        List<int> foundIndexes = new List<int>();
        if(inBetween.Length > 0)
        {
            foreach(RaycastHit hit in inBetween)
            {
                Transform hitTransform = hit.collider.transform;
                MeshRenderer rdr = hit.collider.GetComponent<MeshRenderer>();
                if (modifiedRenderers.Contains(hitTransform))
                {
                    foundIndexes.Add(modifiedRenderers.IndexOf(hitTransform));
                }
                else
                {
                    //rdr.material.color = new Color(rdr.material.color.r, rdr.material.color.g, rdr.material.color.b, .1f);
                    DeactivateObjectRenderer(hitTransform);
                    modifiedRenderers.Add(hitTransform);
                    foundIndexes.Add(modifiedRenderers.Count - 1);
                }
            }
        }
        for(int i = modifiedRenderers.Count - 1; i >= 0; i--)
        {
            if (!foundIndexes.Contains(i))
            {
                Transform tr = modifiedRenderers[i];
                ReactivateObjectRenderer(tr);
                modifiedRenderers.RemoveAt(i);
            }
        }
    }

    private void ReactivateObjectRenderer(Transform tr)
    {
        MeshRenderer[] renderers = tr.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer rdr in renderers)
        {
            rdr.enabled = true;
        }
    }

    private void DeactivateObjectRenderer(Transform tr)
    {
        MeshRenderer[] renderers = tr.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer rdr in renderers)
        {
            rdr.enabled = false;
        }
    }
}
