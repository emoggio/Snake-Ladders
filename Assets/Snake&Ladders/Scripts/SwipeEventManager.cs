using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SwipeEventManager : MonoBehaviour
{
    //swipe variables
    [Range(0.1f, 10f)]
    public float minTime;
    [Range(0.1f, 10f)]
    public float maxTime;
    [Range(100, 1000)]
    public int minSwipeDist;

    float startTime;
    float endTime;

    Vector3 startPos;
    Vector3 endPos;

    float swipeDist;
    float swipeTime;

    //events
    public UnityEngine.Events.UnityEvent TopToBottom;
    public UnityEngine.Events.UnityEvent BottomToTop;
    public UnityEngine.Events.UnityEvent LeftToRight;
    public UnityEngine.Events.UnityEvent RightToLeft;
    void Start()
    {
        minTime = 0.2f;
        maxTime = 1f;
        minSwipeDist = 50;
    }

    //constantly check for touches
    void Update()
    {
        //check if I am touching the screen
        if(Input.touchCount>0)
        {
            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began)
            {
                startTime = Time.time;
                startPos = touch.position;

            }
             else if (touch.phase == TouchPhase.Ended)
            {
                endTime = Time.time;
                endPos = touch.position;

                swipeDist = (endPos - startPos).magnitude;
                swipeTime = endTime - startTime;

                //if it was not an accidental touch/swipe (swipeTime>minTime)
                //and if I am not holding the screen either (swipeTime < maxTime)
                //and if I have swiped for a long enough distance (swipeDist > minSwipeDist)
                if (swipeTime>minTime && swipeTime < maxTime && swipeDist > minSwipeDist)
                {
                    Swipeing();
                }
            }
        }
    }


    //finger swiping method
    void Swipeing()
    {
        //check if my swipe is horizontal or vertical
        Vector2 distance = endPos - startPos;
        if(Mathf.Abs(distance.x) > Mathf.Abs(distance.y))
        {
            //Debug.Log("horizontal Swipe");
            //I want to know if the swipe was left to right or right to left
            if(distance.x>0)
            {
                Debug.Log("Right Swipe");
                Invoke("RightToLeft", 0);
            }
            else if (distance.x < 0)
            {
                Debug.Log("Left Swipe");
                Invoke("LeftToRight", 0);
            }
        }
        else if (Mathf.Abs(distance.x) < Mathf.Abs(distance.y))
        {
            //Debug.Log("vertical Swipe");
            //I want to know if the swipe was top to bottom or bottom to top
            if (distance.y > 0)
            {
                Debug.Log("Up Swipe");
                Invoke("BottomToTop", 0);
            }
            else if (distance.y < 0)
            {
                Debug.Log("Down Swipe");
                Invoke("TopToBottom", 0);
            }
        }
    }
}
