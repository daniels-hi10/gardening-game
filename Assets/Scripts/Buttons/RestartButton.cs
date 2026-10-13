using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class RestartButton : MonoBehaviour
{
    
    public TextMeshProUGUI ScoreText; 

    void Start()
    {
        ScoreText.text = "The day is over. Here is your harvest score: " + Player.Score;
    }
    public void Restart()
    {
        SceneManager.LoadScene("Action");
    }
}

