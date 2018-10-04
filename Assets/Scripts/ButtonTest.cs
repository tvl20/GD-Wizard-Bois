using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTest : MonoBehaviour
{
    public Slider[] healthBarSliders;
    public Button[] AnyButton;
    public float AlphaThreshold = 1f;

    void Start()
    {
        AnyButton = GameObject.Find("Canvas").GetComponentsInChildren<Button>();
        foreach (Button UIbutton in AnyButton)
        {
            UIbutton.onClick.AddListener(() => Debug.Log("AnyButtonPressed"));
            UIbutton.GetComponent<Image>().alphaHitTestMinimumThreshold = AlphaThreshold;
        }
    }

    void Update()
    {
        
    }
}
