using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class Scaler : MonoBehaviour
{

    [SerializeField] Vector3 m_StartScale;
    [SerializeField] Vector3 m_EndScale;
    [SerializeField] float m_ScaleDuration;
    [SerializeField] Ease m_ScaleEase;
    Tween m_ScaleTween;

    [SerializeField] UnityEvent OnAnimationStart;
    [SerializeField] UnityEvent OnAnimationEnd;

    public void OnEnable()
    {
        OnAnimationStart.Invoke();
        transform.localScale = m_StartScale;
        m_ScaleTween?.Kill(true);
        m_ScaleTween = transform.DOScale(m_EndScale, m_ScaleDuration).SetEase(m_ScaleEase).OnComplete(()=>OnAnimationEnd.Invoke());
    }
}
