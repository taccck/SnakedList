using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public virtual void Quit()
    {
        Application.Quit();
    }

    public virtual void StartScreen()
    {
        SceneManager.LoadScene("StartScreen");
    }

    public virtual void StartGame()
    {
        SceneManager.LoadScene("SamDemo");
    }
    
    public virtual void SkillTree()
    {
        SceneManager.LoadScene("SkillTreeScene");
    }
}
