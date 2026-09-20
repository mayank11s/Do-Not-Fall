using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject botPrefab;
    [SerializeField] private int botCount = 3;
    [SerializeField] private PlatformGenerator platformGenerator;
    [SerializeField] private List<GameObject> activeBots = new List<GameObject>();
    private void Awake()
    {
        Instance = this;
        platformGenerator.Init();
        Time.timeScale = 0;
    }

    private void Start()
    {
        SetupCharacters();
        AssignPlatformCenter();
        UIManager.Instance.onCountDownFinish += StartGame;
        StartCoroutine(nameof(HideLoadingPanel));
    }
    private IEnumerator HideLoadingPanel()
    {
        yield return new WaitForSecondsRealtime(1f);
        if (UIManager.Instance != null)
            UIManager.Instance.ShowMenuPanel();
    }
    private void SetupCharacters()
    {
        int totalCharacters = 1 + botCount;
        List<Vector3> spawnPositions = platformGenerator.SetupSpawnTiles(totalCharacters);

        // Position player at first tile position
        if (player != null && spawnPositions.Count > 0)
        {   
            CharacterController controller = player.GetComponent<CharacterController>();
            controller.enabled = false;
            player.transform.position = spawnPositions[0];
            controller.enabled = true;
        }

        // Spawn and position bots at remaining tile positions
        for (int i = 0; i < botCount; i++)
        {
            Vector3 botSpawnPos = spawnPositions[i + 1];
            GameObject newBot = Instantiate(botPrefab, botSpawnPos, Quaternion.identity);

            activeBots.Add(newBot);
        }
    }
    private void AssignPlatformCenter()
    {
        // Assign center of platform to all bots with randomness
        
        foreach (GameObject bot in activeBots)
        {
            Vector3 platformCenter = platformGenerator.GetPlatformCenter();
            bot.GetComponent<BotInput>().SetPlatformCenter(platformCenter);
        }
    }

    private void StartGame()
    {
        Time.timeScale = 1;
        UIManager.Instance.UpdateCounterText(activeBots.Count+1,botCount+1);
    }

    public void CharacterEliminate(GameObject character)
    {
        if (character.CompareTag("Player"))
        {
            EndGame(false);
        }
        else if (character.CompareTag("Bot"))
        {
            if (!activeBots.Contains(character)) return;

            activeBots.Remove(character);
            character.SetActive(false);
            if (activeBots.Count <= 0)
            {
                EndGame(true);
            }
        }
        UIManager.Instance.UpdateCounterText(activeBots.Count+1,botCount+1);

    }
    private void EndGame(bool playerWon)
    {
        Time.timeScale = 0;
        UIManager.Instance.ShowEndGamePopup(playerWon);
    }
}