using UnityEngine;

public class OpeningDoor : MonoBehaviour
{
    public float waitTime = 3f;
    public float animationTime = 0.5f;

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
        StartCoroutine(SquishLoop());
    }

    IEnumerator SquishLoop()
    {
        while (true)
        {
            // Wait in normal state
            yield return new WaitForSeconds(waitTime);

            // Squish
            yield return ScaleTo(new Vector3(originalScale.x, 0f, originalScale.z));

            // Optional: wait while squished
            yield return new WaitForSeconds(1f);

            // Grow back
            yield return ScaleTo(originalScale);
        }
    }

    IEnumerator ScaleTo(Vector3 targetScale)
    {
        Vector3 startScale = transform.localScale;
        float elapsed = 0f;

        while (elapsed < animationTime)
        {
            transform.localScale = Vector3.Lerp(startScale, targetScale, elapsed / animationTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
    }
}
