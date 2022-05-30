using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryTrigger : MonoBehaviour
{
    public GameObject VictoryUI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            VictoryUI.SetActive(true);
            FindObjectOfType<GameManager>().Victory();
        }
    }
}
