using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRoll : MonoBehaviour
{
    private GameObject Manager;
    SwipeEventManager swipeEventManager;

    //spawn object and location
    public Rigidbody Dice;
    Rigidbody DiceClone;
    private Transform SpawnLocation;

    //variables
    private float SwipeSpeed;
    private float MinSpeed = 0.1f;

    private void Awake()
    {
        Manager = GameObject.FindGameObjectWithTag("manager");
        if (Manager != null)
            swipeEventManager = Manager.GetComponent<SwipeEventManager>();

        SwipeSpeed = swipeEventManager.swipeDist;

        SpawnLocation = this.transform;
    }

    public void Roll()
    {
        if(SwipeSpeed<300)
            SwipeSpeed =300;

        // Create an instance of the dice and store a reference to it's rigidbody.
        DiceClone = Instantiate(Dice, SpawnLocation.position, SpawnLocation.rotation) as Rigidbody;
        DiceClone.useGravity = true;

        // Set the Dice velocity and rotation to the launch force in the fire position's forward direction.
        DiceClone.velocity = SwipeSpeed/100 * (SpawnLocation.forward+ SpawnLocation.right);
        DiceClone.angularVelocity = SwipeSpeed / 50 * SpawnLocation.forward;

        // Reset the swipe Speed.(just a precaution)
        SwipeSpeed = MinSpeed;
    }
}
