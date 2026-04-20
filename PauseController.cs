using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseController : MonoBehaviour
{
    [SerializeField] GameObject restartPanel;

     void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool isActive = restartPanel.activeSelf;
            restartPanel.SetActive(!isActive);
        }
      
    }


    public void Rest()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
