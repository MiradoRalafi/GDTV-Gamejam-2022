using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MovingObject
{
    #region PARAMETERS

    [SerializeField]
    private Transform[] tires;
    [SerializeField]
    private float tireRotationSpeed = -15f;


    #endregion

    #region CACHES



    #endregion

    #region STATES



    #endregion

    protected override void Update()
    {
        base.Update();
        foreach(Transform tire in tires)
        {
            tire.Rotate(tire.forward, tireRotationSpeed * Time.deltaTime);
        }
    }
}
