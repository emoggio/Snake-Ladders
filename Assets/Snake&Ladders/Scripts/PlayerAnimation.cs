﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerAnimation : MonoBehaviour
{
    //gameobjects
    private GameObject PlayerOne;
    private GameObject FigureOne;

    private GameObject PlayerTwo;
    private GameObject FigureTwo;

    //AnimationVaribale
    [Range(0.01f, 0.1f)]
    public float Gitter;

    [Range(1,10f)]
    public int Rotation;

    //AnimationSpeed
    [Range(0.1f, 10)]
    public float animationTime;


    private void Awake()
    {
        PlayerOne = GameObject.FindGameObjectWithTag("Player");
        PlayerTwo = GameObject.FindGameObjectWithTag("cpu");

        if (PlayerOne!=null)
        {
            for (int i = 0; i < PlayerOne.transform.childCount; i++)
            {
                if (PlayerOne.transform.GetChild(i).gameObject.activeSelf == true)
                    FigureOne = PlayerOne.transform.GetChild(i).gameObject;
            }
        }

        if (PlayerTwo != null)
        {
            for (int i = 0; i < PlayerTwo.transform.childCount; i++)
            {
                if (PlayerTwo.transform.GetChild(i).gameObject.activeSelf == true)
                    FigureTwo = PlayerTwo.transform.GetChild(i).gameObject;
            }
        }

        if (FigureOne!=null)
            FigureOne.transform.DOLocalMoveY(Gitter, animationTime).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);

        if (FigureTwo != null)
            FigureTwo.transform.DOLocalMoveY(Gitter, animationTime).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
    }

    public void PlayerExcitement()
    {
        if (FigureOne != null && PlayerOne!=null)
        {
            Sequence spin = DOTween.Sequence();
            spin.Append(PlayerOne.transform.DOLocalMoveY(Gitter * 3, animationTime).SetEase(Ease.InOutQuad).SetLoops(1, LoopType.Yoyo))
                .Join(FigureOne.transform.DORotate(new Vector3(0, Rotation * 360, 0), animationTime * Rotation).SetEase(Ease.InOutQuad));
        }
    }

    public void CpuExcitement()
    {
        if (FigureTwo != null && PlayerTwo != null)
        {
            Sequence spin = DOTween.Sequence();
            spin.Append(PlayerTwo.transform.DOLocalMoveY(Gitter * 3, animationTime).SetEase(Ease.InOutQuad).SetLoops(1, LoopType.Yoyo))
                .Join(FigureTwo.transform.DORotate(new Vector3(0, Rotation * 360, 0), animationTime * Rotation).SetEase(Ease.InOutQuad));
        }
    }
}
            
