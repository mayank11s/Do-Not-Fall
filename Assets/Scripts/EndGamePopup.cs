using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGamePopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resultText;

    public void SetResultText(bool playerWon)
    {
        if (playerWon)
        {
            resultText.text = "Congrats! You win.";
            if(AudioManager.Instance!=null)
                AudioManager.Instance.PlayVictorySfx();
        }
        else
        {
            if(AudioManager.Instance!=null)
                AudioManager.Instance.PlayLosingSfx();
            resultText.text = "You lose. Try Again!";
        }
    }
    public void ReplayOnClick()
    {
        AudioManager.Instance.PlayClickSfx();
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
