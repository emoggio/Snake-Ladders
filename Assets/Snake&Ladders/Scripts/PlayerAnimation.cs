using System.Collections;
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

    private GameObject Manager;
    MovingManager movingManager;

    //AnimationVaribale
    [Range(0.01f, 0.1f)]
    public float Gitter;

    [Range(0.1f, 3f)]
    public float ItsMe;

    [Range(1,10f)]
    public int Rotation;

    //AnimationSpeed
    [Range(0.1f, 10)]
    public float animationTime;


    private void Awake()
    {
        Manager = GameObject.FindGameObjectWithTag("manager");
        if (Manager != null)
            movingManager = Manager.GetComponent<MovingManager>();

        PlayerOne = GameObject.FindGameObjectWithTag("Player");
        PlayerTwo = GameObject.FindGameObjectWithTag("cpu");

        if (PlayerOne != null)
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

        if (FigureOne != null)
            FigureOne.transform.DOLocalMoveY(Gitter, animationTime).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);

        if (FigureTwo != null)
            FigureTwo.transform.DOLocalMoveY(Gitter, animationTime).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
    }

    public void Excitement()
    {
        if (Manager != null)
        {
            if (FigureOne != null && PlayerOne != null && movingManager.myTurn)
            {
                var angle = new Vector3(0, Rotation * 360, 0);

                Sequence spin = DOTween.Sequence();
                spin.Append(FigureOne.transform.DOLocalMoveY(ItsMe, animationTime).SetEase(Ease.InOutQuad).SetLoops(2, LoopType.Yoyo))
                    .Join(FigureOne.transform.DORotate(angle, animationTime * Rotation, RotateMode.LocalAxisAdd).SetEase(Ease.InOutQuad))
                    .AppendCallback(movingManager.moveThePlayers);
            }

            if (FigureTwo != null && PlayerTwo != null && !movingManager.myTurn)
            {
                var angle = new Vector3(0, Rotation * 360, 0);

                movingManager.DiceRoll();

                Sequence spin = DOTween.Sequence();
                spin.Append(FigureTwo.transform.DOLocalMoveY(ItsMe, animationTime).SetEase(Ease.InOutQuad).SetLoops(2, LoopType.Yoyo))
                    .Join(FigureTwo.transform.DORotate(angle, animationTime * Rotation, RotateMode.LocalAxisAdd).SetEase(Ease.InOutQuad))
                    .AppendCallback(movingManager.moveThePlayers);
            }
        }
    }
}
            
