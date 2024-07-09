using UnityEngine;
using UnityEngine.SceneManagement;
public class StageChangeManager : MonoBehaviour
{
    private static StageChangeManager _instance;

    public PlayerStat savePlayer;

    public static StageChangeManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("StageChangeManager").AddComponent<StageChangeManager>();
            }
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance != null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (_instance == this)
            {
                Destroy(gameObject);
            }
        }
    }


    public void SaveScene()
    {
        savePlayer = CharacterManager.Instance.Player.stats;
    }

    public void SceneChange()
    {
        SceneManager.LoadScene(2);
    }






}