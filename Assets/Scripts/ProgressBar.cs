using System.Timers;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField]
    private Image ProgressImage;
    [SerializeField]
    private float DefaultSpeed = 1f;
    [SerializeField]
    private UnityEvent<float> OnProgress;
    [SerializeField]
    private UnityEvent OnCompleted;

    private Coroutine AnimationCoroutine;

    void Start()
    {
        if(ProgressImage.type != Image.Type.Filled)
        {
            Debug.LogError($"{name}'s Progress Image isn't filled. Disabling progress bar.");
            this.enabled = false;
        }
    }

    public void SetProgress(float progress)
    {
        SetProgress(progress, DefaultSpeed);
    }

    public void SetProgress(float progress, float speed)
    {
        if(progress < 0 || progress > 1)
        {
            Debug.LogWarning("Progress not in accepted range");
            progress = Mathf.Clamp01(progress);
        }
        if(progress != ProgressImage.fillAmount)
        {
            if(AnimationCoroutine != null)
            {
                StopCoroutine(AnimationCoroutine);
            }

            AnimationCoroutine = StartCoroutine(AnimateProgress(progress, speed));
        }
    }

    private IEnumerator AnimateProgress(float progress, float speed)
    {
        float _time = 0;
        float initialProgress = ProgressImage.fillAmount;

        while(_time < 1)
        {
            ProgressImage.fillAmount = Mathf.Lerp(initialProgress, progress, _time);
            _time += Time.deltaTime * speed;

            OnProgress?.Invoke(ProgressImage.fillAmount);
            yield return null;
        }

        ProgressImage.fillAmount = progress;
        OnProgress?.Invoke(progress);
        OnCompleted?.Invoke();
    }
}