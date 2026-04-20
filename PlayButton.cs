using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    [SerializeField] private string _gameSceneName = "Game";

    public void PlayGame()
    {
        SceneManager.LoadScene(_gameSceneName);
    }
}