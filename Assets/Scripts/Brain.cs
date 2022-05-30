using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour
{
    #region PARAMETERS



    #endregion

    #region CACHES

    

    #endregion

    #region STATES



    #endregion

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player)
        {
            player.Eat();
            Destroy(gameObject);
        }
    }
}
