using UnityEngine;
using System.Collections;

public class TweenPosition : MonoBehaviour
{
    public Vector3 from = Vector3.zero;
    public Vector3 to = Vector3.zero;
    public float duration = 1f;
    public float startDelay = 0f;

    private bool _started = false;
    private float _factor = 0;
    float mDuration = 0f;
    float mAmountPerDelta = 1000f;
    float _startTime = 0f;

    private float AmountPerDelta
    {
        get
        {
            if (mDuration != duration)
            {
                mDuration = duration;
                mAmountPerDelta = Mathf.Abs((duration > 0f) ? 1f / duration : 1000f);
            }
            return mAmountPerDelta;
        }
    }

    private bool _dirForward = true;
    public void PlayAuto()
    {
        Play(_dirForward);
        _dirForward = !_dirForward;
    }

    public void Play(bool forward)
    {
        mAmountPerDelta = Mathf.Abs(AmountPerDelta);
        if (!forward) mAmountPerDelta = -mAmountPerDelta;
        enabled = true;
        Update();
    }

    void Update()
    {
        float delta = Time.deltaTime;
        float time = Time.time;

        if (!_started)
        {
            _started = true;
            _startTime = time + startDelay;
        }
        if (time < _startTime)
            return;

        _factor += AmountPerDelta * delta;

        if (duration == 0f || _factor > 1f || _factor < 0f)
        {
            _factor = Mathf.Clamp01(_factor);
            if (duration == 0f || (_factor == 1f && mAmountPerDelta > 0f || _factor == 0f && mAmountPerDelta < 0f))
                enabled = false;
        }
        float val = Mathf.Clamp01(_factor);
        const float pi2 = Mathf.PI * 2f;
        val = val - Mathf.Sin(val * pi2) / pi2;
        transform.localPosition = from * (1f - val) + to * val;
    }

    [ContextMenu("Set 'From' to current value")]
    public void SetStartToCurrentValue() { from = transform.localPosition; }

    [ContextMenu("Set 'To' to current value")]
    public void SetEndToCurrentValue() { to = transform.localPosition; }

    [ContextMenu("Assume value of 'From'")]
    void SetCurrentValueToStart() { transform.localPosition = from; }

    [ContextMenu("Assume value of 'To'")]
    void SetCurrentValueToEnd() { transform.localPosition = to; }
}
