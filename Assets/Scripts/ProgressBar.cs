using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image progressImage;
    [SerializeField] private float defaultSpeed = 1f;
    [SerializeField] private UnityEvent<float> onProgress;
    [SerializeField] private UnityEvent onCompleted;

    private Coroutine animationCoroutine;

    void Start()
    {
        if (progressImage.type != Image.Type.Filled)
        {
            Debug.LogError($"{name}'s Progress Image isn't filled. Disabling progress bar.");
            enabled = false;
        }
    }

    public void SetProgress(float progress)
    {
        SetProgress(progress, defaultSpeed);
    }

    public void SetProgress(float progress, float speed)
    {
        if (progress < 0 || progress > 1)
        {
            Debug.LogWarning("Progress not in accepted range");
            progress = Mathf.Clamp01(progress);
        }
        if (progress != progressImage.fillAmount)
        {
            if(animationCoroutine != null)
            {
                StopCoroutine(animationCoroutine);
            }

            animationCoroutine = StartCoroutine(AnimateProgress(progress, speed));
        }
    }

    private IEnumerator AnimateProgress(float progress, float speed)
    {
        float _time = 0;
        float initialProgress = progressImage.fillAmount;

        while (_time < 1)
        {
            progressImage.fillAmount = Mathf.Lerp(initialProgress, progress, _time);
            _time += Time.deltaTime * speed;

            onProgress?.Invoke(progressImage.fillAmount);
            yield return null;
        }

        progressImage.fillAmount = progress;
        onProgress?.Invoke(progress);
        onCompleted?.Invoke();
    }
}