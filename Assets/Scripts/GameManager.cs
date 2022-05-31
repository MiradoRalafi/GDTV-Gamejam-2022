using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    #region PARAMETERS

    public float EnvironmentRenderDistanceLimit = 15f;
    public float MovingObjectRenderDistanceLimit = 40f;
    public Transform spawnPoint;
    [SerializeField]
    private GameObject warning;

    [SerializeField]
    private bool cursorVisible = true;

    #endregion

    #region CACHES

    [HideInInspector]
    public PlayerController Player;

    #endregion

    #region STATES
    


    #endregion

    private void Awake()
    {
        if (Instance && Instance != this)
        {
            Instance.Player = Player;
            Instance.spawnPoint = spawnPoint;
            Instance.warning = warning;
            Destroy(gameObject);
            return;
        }
        Cursor.visible = cursorVisible;
        Player = FindObjectOfType<PlayerController>();
        if (Player)
        {
            Player.transform.position = spawnPoint.position;
            Player.transform.rotation = Quaternion.identity;
        }
        Instance = this;
    }

    public void ReloadLevel()
    {
        Player.transform.position = spawnPoint.position;
        Player.transform.rotation = Quaternion.identity;
        Player.gameObject.SetActive(true);
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ShowWarning()
    {
        if (!warning.activeSelf)
        {
            warning.SetActive(true);
        }
    }

    public void HideWarning()
    {
        if (warning.activeSelf)
        {
            warning.SetActive(false);
        }
    }

    public void LoadLevel(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void Victory()
    {
        StartCoroutine(GoBackToMenu());
    }

    private IEnumerator GoBackToMenu()
    {
        yield return new WaitForSeconds(3);
        LoadLevel(0);
    }
}
