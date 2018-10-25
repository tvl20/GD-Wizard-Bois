using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SpellbookSwiping : MonoBehaviour
{
    public float SWIPE_THRESHOLD = 20f; // const
    public bool DetectSwipeOnlyAfterRelease = false;
    public bool LoopSpellBook = true;
    public Sprite[] SpellPatterns;
    public Image SpellBookPage;

    private RectTransform spellBook;
    private Vector2 fingerDown;
    private Vector2 fingerUp;
    private int swipeIndex = 0;


    private void Start()
    {
        spellBook = this.GetComponent<RectTransform>();
        UpdateSpellbook(swipeIndex);
    }

    // Update is called once per frame
    private void Update()
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
                if (!DetectSwipeOnlyAfterRelease)
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

    private void CheckSwipe()
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(spellBook, fingerDown))
        {
            //Check if Horizontal swipe
            if (HorizontalValMove() > SWIPE_THRESHOLD && HorizontalValMove() > VerticalMove())
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
        }
    }

    private float VerticalMove()
    {
        return Mathf.Abs(fingerDown.y - fingerUp.y);
    }

    private float HorizontalValMove()
    {
        return Mathf.Abs(fingerDown.x - fingerUp.x);
    }

    private void UpdateSpellbook(int changedIndex)
    {
        SpellBookPage.sprite = SpellPatterns[changedIndex];
    }

    //////////////////////////////////CALLBACK FUNCTIONS/////////////////////////////

    private void OnSwipeLeft()
    {
        //Debug.Log("Swipe Left");
        if (swipeIndex > 0)
        {
            swipeIndex = swipeIndex - 1;
        } else
        {
            if (LoopSpellBook)
            {
                swipeIndex = (SpellPatterns.Length - 1);
            }
        }
//        Debug.Log(swipeIndex);
        UpdateSpellbook(swipeIndex);
    }

    private void OnSwipeRight()
    {
        //Debug.Log("Swipe Right");
        if (swipeIndex < (SpellPatterns.Length - 1))
        {
            swipeIndex = swipeIndex + 1;
        }
        else
        {
            if (LoopSpellBook)
            {
                swipeIndex = 0;
            }
        }
//        Debug.Log(swipeIndex);
        UpdateSpellbook(swipeIndex);
    }
}
