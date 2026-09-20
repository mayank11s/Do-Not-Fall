using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject joystick;
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private GameObject MenuPanel;
    [SerializeField] private EndGamePopup endGamePopup;
    [SerializeField] private TextMeshProUGUI counterText;

    public Action onCountDownFinish = null;
    void Awake()
    {
        Instance = this;
        SetupUI();
    }
    void SetupUI()
    {
        joystick.SetActive(false);
        loadingPanel.SetActive(true);
        MenuPanel.SetActive(false);
        endGamePopup.gameObject.SetActive(false);
        counterText.text = null;
    }
    void Start()
    {
        onCountDownFinish += CountdownFinish;
    }
    public void ShowMenuPanel()
    {
        loadingPanel.SetActive(false);
        MenuPanel.SetActive(true);
    }
    private void CountdownFinish()
    {
        MenuPanel.SetActive(false);
        joystick.SetActive(true);
    }
    public void ShowEndGamePopup(bool playerWon)
    {
        endGamePopup.gameObject.SetActive(true);
        endGamePopup.SetResultText(playerWon);
    }
    public void UpdateCounterText(int remainingPlayers, int totalPlayers)
    {
        counterText.text = $"{remainingPlayers}/{totalPlayers}";
    }
}