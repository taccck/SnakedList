using UnityEngine;

public class PauseMenu : Buttons
{
    [SerializeField] private GameObject pauseMenu;
    
    private bool paused;

    public override void Quit()
    {
        Time.timeScale = 1f;
        TickManager.current.running = false;
        base.Quit();
    }

    public override void StartGame()
    {
        Time.timeScale = 1f;
        TickManager.current.running = false;
        base.StartGame();
    }

    public override void StartScreen()
    {
        Time.timeScale = 1f;
        TickManager.current.running = false;
        base.StartScreen();
    }
    
    private void OnPause()
    {
        paused = !paused;
        pauseMenu.SetActive(paused);
        Time.timeScale = paused ? 0f : 1f;
    }
}
