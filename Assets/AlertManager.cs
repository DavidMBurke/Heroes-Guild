using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AlertManager : MonoBehaviour
{
    public static AlertManager instance;

    public GameObject alertPrefab;
    public Transform alertParent;
    public float timeUntilFade = 1f;
    public float timeToFade = 2f;

    private void Awake()
    {
        instance = this;
    }

    public void ShowAlert(string message)
    {
        float duration = timeUntilFade + timeToFade;
        GameObject alert = Instantiate(alertPrefab, alertParent);
        TextMeshProUGUI text = alert.GetComponent<TextMeshProUGUI>();
        text.text = message;
        CanvasGroup cg = alert.AddComponent<CanvasGroup>();
        StartCoroutine(FadeAndDestroy(alert, cg, duration));
    }

    private IEnumerator FadeAndDestroy(GameObject alert, CanvasGroup cg, float duration)
    {
        yield return new WaitForSeconds(duration);
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            cg.alpha = 1 - (t / timeToFade);
            yield return null;
        }
        Destroy(alert);
    }
}
