using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private GameObject player;
    [SerializeField] private PlatformGenerator platformGenerator;
    void Awake()
    {
        Instance = this;
        platformGenerator.Init();
    }
    void Start()
    {
        SetupPlayerPosition();
    }
    private void SetupPlayerPosition()
    {
        player.transform.position = platformGenerator.SetupPlayerStandTile();
    }
}
