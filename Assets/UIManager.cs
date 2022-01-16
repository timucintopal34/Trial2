using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private Transform TapToStartUI;
    [SerializeField] private Transform SuccessUI;

    private bool isGameStarted = false;
    private bool isGameEnded = false;

    public UnityAction OnLevelStart, OnLevelEnd;

    private void Update()
    {
        if(!isGameStarted)
            if(Input.GetMouseButtonDown(0))
                StartLevel();
                
    }

    private void StartLevel()
    {
        isGameStarted = false;
        OnLevelStart?.Invoke();
        TapToStartUI.DOScale(Vector3.zero, .5f).SetEase(Ease.InBack);
    }

    public void LevelEnd()
    {
        if(!isGameEnded)
        {
            isGameEnded = true;
            OnLevelEnd?.Invoke();
        }
    }
}
