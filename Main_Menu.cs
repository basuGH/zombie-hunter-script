
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour
{
    private void Start()
    {
        
    }
    public void PlayBtnClick()
    {
        SceneManager.LoadScene(1);
    }
    public void ExitBtnClick()
    {
        Application.Quit();
    }
    
}
