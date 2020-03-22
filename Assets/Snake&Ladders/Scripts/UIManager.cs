using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    //update UI with the relative DiceRoll
    public TextMeshProUGUI PlayerDiceRoll;
    public TextMeshProUGUI CpuDiceRoll;
    
    //canvas gameobjectreference
    public GameObject YouLose;
    CanvasGroup YouLoseCG;
    public GameObject YouWin;
    CanvasGroup YouWinCG;

    //managerreference
    private GameObject Manager;
    MovingManager movingManager;

    //AnimationSpeed
    [Range(0.1f, 10)]
    public float animationTime;

    private void Awake()
    {
        Manager = GameObject.FindGameObjectWithTag("manager");
        if (Manager != null)
            movingManager = Manager.GetComponent<MovingManager>();

        if (YouLose != null)
        {
            YouLoseCG = YouLose.GetComponent<CanvasGroup>();
            if (YouLoseCG.alpha != 0)
                YouLoseCG.alpha = 0;

            YouLose.SetActive(false);
        }

        if (YouWin != null)
        {
            YouWinCG = YouWin.GetComponent<CanvasGroup>();
            if (YouWinCG.alpha != 0)
                YouWinCG.alpha = 0;

            YouWin.SetActive(false);
        }

    }
    public void UpdateDiceRollPlayer()
    {
        PlayerDiceRoll.text = movingManager.diceRoll.ToString();
    }

    public void UpdateDiceRollCpu()
    {
        CpuDiceRoll.text = movingManager.diceRoll.ToString();
    }

    public void Win()
    {
        YouWin.SetActive(true);
        DOTween.To(() => YouWinCG.alpha, x => YouWinCG.alpha = x, 1, animationTime);
    }

    public void Lose()
    {
        YouLose.SetActive(true);
        DOTween.To(() => YouLoseCG.alpha, x => YouLoseCG.alpha = x, 1, animationTime);
    }

    public void Replay()
    {
        if(YouWinCG.alpha!=0)
            DOTween.To(() => YouWinCG.alpha, x => YouWinCG.alpha = x, 0, animationTime);

        if(YouLoseCG.alpha!=0)
            DOTween.To(() => YouLoseCG.alpha, x => YouLoseCG.alpha = x, 1, animationTime);

        PlayerDiceRoll.text = 0.ToString();
        CpuDiceRoll.text = 0.ToString();

        movingManager.ResetPosition();
    }
}
