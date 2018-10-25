using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SpellbookSwiping : MonoBehaviour {

    private Vector2 fingerDown;
    private Vector2 fingerUp;
    public bool detectSwipeOnlyAfterRelease = false;
    private int swipeIndex;
    public Sprite[] spellPatterns;
    public Image spellBookPage;
    public RectTransform spellBook;

    public float SWIPE_THRESHOLD = 20f;

    private void Start()
    {
        swipeIndex = 0;
        UpdateSpellbook(swipeIndex);
    }

    // Update is called once per frame
    void Update()
    {

        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                fingerUp = touch.position;
                fingerDown = touch.position;
            }

            //Detects Swipe while finger is still moving
            if (touch.phase == TouchPhase.Moved)
            {
                if (!detectSwipeOnlyAfterRelease)
                {
                    fingerDown = touch.position;
                    CheckSwipe();
                }
            }

            //Detects swipe after finger is released
            if (touch.phase == TouchPhase.Ended)
            {
                fingerDown = touch.position;
                CheckSwipe();
            }
        }
    }

    void CheckSwipe()
    {

        if (RectTransformUtility.RectangleContainsScreenPoint(
            spellBook, fingerDown
            ))
        {
            //Check if Vertical swipe
            if (VerticalMove() > SWIPE_THRESHOLD && VerticalMove() > HorizontalValMove())
            {
                //Debug.Log("Vertical");
                if (fingerDown.y - fingerUp.y > 0)//up swipe
                {
                    OnSwipeUp();
                }
                else if (fingerDown.y - fingerUp.y < 0)//Down swipe
                {
                    OnSwipeDown();
                }
                fingerUp = fingerDown;
            }

            //Check if Horizontal swipe
            else if (HorizontalValMove() > SWIPE_THRESHOLD && HorizontalValMove() > VerticalMove())
            {
                //Debug.Log("Horizontal");
                if (fingerDown.x - fingerUp.x > 0)//Right swipe
                {
                    OnSwipeRight();
                }
                else if (fingerDown.x - fingerUp.x < 0)//Left swipe
                {
                    OnSwipeLeft();
                }
                fingerUp = fingerDown;
            }

            //No Movement at-all
            else
            {
                //Debug.Log("No Swipe!");
            }
        }
    }

    float VerticalMove()
    {
        return Mathf.Abs(fingerDown.y - fingerUp.y);
    }

    float HorizontalValMove()
    {
        return Mathf.Abs(fingerDown.x - fingerUp.x);
    }

    //////////////////////////////////CALLBACK FUNCTIONS/////////////////////////////
    void OnSwipeUp()
    {
        //Debug.Log("Swipe UP");
    }

    void OnSwipeDown()
    {
        //Debug.Log("Swipe Down");
    }

    void OnSwipeLeft()
    {
        //Debug.Log("Swipe Left");
        if (swipeIndex > 0)
        {
            swipeIndex = swipeIndex - 1;
        } else
        {
            swipeIndex = (spellPatterns.Length - 1);
        }
        Debug.Log(swipeIndex);
        UpdateSpellbook(swipeIndex);
    }

    void OnSwipeRight()
    {
        //Debug.Log("Swipe Right");
        if (swipeIndex < (spellPatterns.Length - 1))
        {
            swipeIndex = swipeIndex + 1;
        }
        else
        {
            swipeIndex = 0;
        }
        Debug.Log(swipeIndex);
        UpdateSpellbook(swipeIndex);
    }

    void UpdateSpellbook(int changedIndex)
    {
        spellBookPage.sprite = spellPatterns[changedIndex];
    }

}
