using System.Collections;
using UnityEngine;

public class PlatformTile : MonoBehaviour
{
    [SerializeField] float breakingDuration = 1.25f;
    [SerializeField] float animationDuration = 0.5f;
    
    [SerializeField] private Renderer _renderer;
    private MaterialPropertyBlock block;
    private bool isBreaking = false;

    public bool IsBreaking => isBreaking;
    void Awake()
    {
        if (_renderer == null)
            _renderer = GetComponent<Renderer>();

        block = new MaterialPropertyBlock();
        _renderer.GetPropertyBlock(block);
    }
    public void CharacterStepped()
    {
        isBreaking = true;
        StartCoroutine(PlaySteppedAnimation());
        StartCoroutine(StartBreaking());
    }

    private IEnumerator StartBreaking()
    {
        float elapsedTime = 0f;
        Color tileColor = block.GetColor("_BaseColor");
        while (elapsedTime < breakingDuration)
        {
            Color currentColor = Color.Lerp(tileColor, Color.white, elapsedTime);
            SetPlatformTileColor(currentColor);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
    public void SetPlatformTileColor(Color color)
    {
        if (block != null)
        {
            block.SetColor("_BaseColor", color);
            _renderer.SetPropertyBlock(block);
        }
    }
    private IEnumerator PlaySteppedAnimation()
{
    Vector3 originalScale = transform.localScale;
    Vector3 targetScale = originalScale * 0.9f;

    float halfDuration = animationDuration * 0.5f;
    float elapsed = 0f;

    // Scale down to 90%
    while (elapsed < halfDuration)
    {
        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / halfDuration);

        transform.localScale = Vector3.Lerp(originalScale,targetScale,t);
        yield return null;
    }

    // Scale back to 100%
    elapsed = 0f;

    while (elapsed < halfDuration)
    {
        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / halfDuration);

        transform.localScale = Vector3.Lerp(targetScale,originalScale,t);
        yield return null;
    }

    transform.localScale = originalScale;
}
}
