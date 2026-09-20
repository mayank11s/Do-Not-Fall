using System.Collections;
using TMPro;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private GameObject PlayButton;
    [SerializeField] private int countDownTime = 3;
    private Animator animator;
    bool isCountdownStart = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void OnPlayClick()
    {
        if (!isCountdownStart)
        {
            isCountdownStart = true;
            StartCoroutine(StartCountDown());
            if(animator != null)
            {
                animator.SetTrigger("HidePlayButton");
            }
            AudioManager.Instance.PlayClickSfx();
        }
    }
    private IEnumerator StartCountDown()
    {
        float timer = countDownTime;
        while (timer > 0f)
        {
            countdownText.text = Mathf.CeilToInt(timer).ToString();

            yield return null;

            timer -= Time.unscaledDeltaTime;
        }

        countdownText.text = "0";
        yield return new WaitForSecondsRealtime(.1f);
        UIManager.Instance.onCountDownFinish?.Invoke();
    }
}