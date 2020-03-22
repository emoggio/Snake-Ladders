using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI PlayerDiceRoll;
    public TextMeshProUGUI CpuDiceRoll;

    private GameObject Manager;
    MovingManager movingManager;

    private void Awake()
    {
        Manager = GameObject.FindGameObjectWithTag("manager");
        if (Manager != null)
            movingManager = Manager.GetComponent<MovingManager>();
    }
    public void UpdateDiceRollPlayer()
    {
        PlayerDiceRoll.text = movingManager.diceRoll.ToString();
    }

    public void UpdateDiceRollCpu()
    {
        CpuDiceRoll.text = movingManager.diceRoll.ToString();
    }
}
