using UnityEngine;

public class UIBlink : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float blinkSpeed = 2f;

    private void OnEnable()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        // Pulsierender Alpha-Wert zwischen 0.2 und 1
        float alpha = Mathf.PingPong(Time.time * blinkSpeed, 0.8f) + 0.2f;
        canvasGroup.alpha = alpha;
    }
}