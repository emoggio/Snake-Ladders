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
    public GameObject Play;
    CanvasGroup PlayCG;
    public GameObject Resume;
    CanvasGroup ResumeCG;

    //managerreference
    private GameObject Manager;
    MovingManager movingManager;
    SwipeEventManager swiping;

    //AnimationSpeed
    [Range(0.1f, 10)]
    public float animationTime;

    //make sure that all the canvase are in the correct state and transparency setting
    private void Awake()
    {
        Manager = GameObject.FindGameObjectWithTag("manager");
        if (Manager != null)
        {
            movingManager = Manager.GetComponent<MovingManager>();
            swiping = Manager.GetComponent<SwipeEventManager>();
        }

        swiping.enabled = false;

        if (Play != null)
        {
            PlayCG = Play.GetComponent<CanvasGroup>();
            if (PlayCG.alpha != 1)
                PlayCG.alpha = 1;

            Play.SetActive(true);
            PlayCG.interactable = true;
            PlayCG.blocksRaycasts = true;
        }

        if (Resume != null)
        {
            ResumeCG = Resume.GetComponent<CanvasGroup>();
            if (ResumeCG.alpha != 0)
                ResumeCG.alpha = 0;

            Resume.SetActive(true);
            ResumeCG.interactable = false;
            ResumeCG.blocksRaycasts = false;
        }

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

    //update UI with Player dice roll
    public void UpdateDiceRollPlayer()
    {
        PlayerDiceRoll.text = movingManager.diceRoll.ToString();
    }

    //update UI with Cpu dice roll
    public void UpdateDiceRollCpu()
    {
        CpuDiceRoll.text = movingManager.diceRoll.ToString();
    }

    //start playing, hide the canvs make sure its your turn
    public void pressPlay()
    {
        DOTween.To(() => PlayCG.alpha, x => PlayCG.alpha = x, 0, animationTime);
        PlayCG.interactable = false;
        PlayCG.blocksRaycasts = false;

        swiping.enabled = true;
    }

    //resume play after pause
    public void ResumeGame()
    {
        DOTween.To(() => ResumeCG.alpha, x => ResumeCG.alpha = x, 0, animationTime);
        ResumeCG.interactable = false;
        ResumeCG.blocksRaycasts = false;

        swiping.enabled = true;
    }

    //pause the game
    public void PauseGame()
    {
        DOTween.To(() => ResumeCG.alpha, x => ResumeCG.alpha = x, 1, animationTime);
        ResumeCG.interactable = true;
        ResumeCG.blocksRaycasts = true;

        swiping.enabled = false;
    }

    public void Win()
    {
        YouWin.SetActive(true);
        DOTween.To(() => YouWinCG.alpha, x => YouWinCG.alpha = x, 1, animationTime);
    }

    //you lose canvas
    public void Lose()
    {
        YouLose.SetActive(true);
        DOTween.To(() => YouLoseCG.alpha, x => YouLoseCG.alpha = x, 1, animationTime);
    }


    //rematch! reset UI, dice rolls and players position
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
