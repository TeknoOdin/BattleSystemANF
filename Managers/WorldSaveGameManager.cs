using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldSaveGameManager : MonoBehaviour
{
    
     public static WorldSaveGameManager Instance;

    public GameObject player;

    [Header("Positions")]
    public Vector3 nextPlayerPosition;
    public Vector3 lastPlayerPosition;//Battle so far

    [Header("scenes")]
    public string sceneToLoad;
    public string lastScene;//Battle so far


    [SerializeField] int worldSceneIndex = 1;

    private void Awake()
    {
        //Instanc THERE CAN BE ONLY ONE!! ( destroy other if one already exists)
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        if (!GameObject.Find("Player"))
        {
            GameObject Player = Instantiate(player,Vector3.one,Quaternion.identity) as GameObject;
            Player.name = "Player";
        }   
    }
    public void LoadNextScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
   
    public IEnumerator LoadNewGame()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);

        yield return null;
    }

    public int GetWorldSceneIndex()
    {
       return worldSceneIndex;
    } 
}




