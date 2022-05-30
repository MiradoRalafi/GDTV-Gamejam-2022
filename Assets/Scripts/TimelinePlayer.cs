using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelinePlayer : MonoBehaviour
{
    #region PARAMETERS

    [SerializeField]
    private GameObject canvas;

    #endregion

    #region CACHES

    private PlayableDirector director;
    private PlayerController player;

    #endregion

    #region STATES



    #endregion

    private void Awake()
    {
        director = GetComponent<PlayableDirector>();
        player = FindObjectOfType<PlayerController>();
        director.played += Director_Played;
        //director.stopped += Director_Stopped;
    }

    private void Director_Played(PlayableDirector obj)
    {
        canvas.SetActive(false);
        player.enabled = false;
    }

    //private void Director_Stopped(PlayableDirector obj)
    //{
    //    throw new NotImplementedException();
    //}

    public void StartTimeline()
    {
        director.Play();
    }
}
