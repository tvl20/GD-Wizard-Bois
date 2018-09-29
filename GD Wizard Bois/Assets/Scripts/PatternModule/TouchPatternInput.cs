using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PatternUnityEvent : UnityEvent<TouchPatternInput.UniquePatterns>
{
}

public class NodeAddedEvent : UnityEvent<int>
{
}

public class TouchPatternInput : MonoBehaviour
{
    public PatternUnityEvent onFinishedPattern = new PatternUnityEvent();
    public NodeAddedEvent onNodeAddedToPattern = new NodeAddedEvent();

    public enum UniquePatterns
    {
        None,
        Attack,
        Heal,
        PartyHeal
    }

    private class Pattern
    {
        public readonly UniquePatterns UniquePattern;
        private readonly int[] PatternArray;

        public Pattern(UniquePatterns uniquePattern, int[] patternArray)
        {
            UniquePattern = uniquePattern;
            PatternArray = patternArray;
        }

        public bool PatternArrayMatches(int[] otherPatternArray)
        {
            if (otherPatternArray == null) return false;
            return PatternArray.SequenceEqual(otherPatternArray);
        }
    }

    private readonly List<Pattern> knownPatterns = new List<Pattern>()
    {
        {new Pattern(UniquePatterns.Attack, new int[] {1, 2, 3, 5, 7, 8, 9})},
        {new Pattern(UniquePatterns.Heal, new int[] {7, 4, 1, 5, 9, 6, 3})},
        {new Pattern(UniquePatterns.PartyHeal, new int[] {1, 2, 3, 6, 9, 8, 7, 4, 5})}
    };

    private List<int> currentPattern;
//	public List<int> getCurrentPattern()
//	{
//		//return a copy of the list
//		return new List<int>(currentPattern);
//	}

    private bool finishedPatternFrame = false;

    private void Start()
    {
        currentPattern = new List<int>();
    }

    private void Update()
    {
        if (finishedPatternFrame) finishedPatternFrame = false;
//        if (Input.touchCount <= 0) return;

//        if (Input.touches[0].phase == TouchPhase.Ended)
        if(Input.GetMouseButtonUp(0))
        {
            UniquePatterns pattern = checkPattern(currentPattern.ToArray());
//            Debug.Log(pattern);

//            string output = "";
//            foreach (int i in currentPattern)
//            {
//                output += i + ", ";
//            }
//
//            Debug.Log(output);

            onFinishedPattern.Invoke(pattern);
            currentPattern.Clear();
            finishedPatternFrame = true;
        }
    }

    public void addNodeToCurrentPattern(int node)
    {
        if (!currentPattern.Contains(node) && !finishedPatternFrame)
        {
            currentPattern.Add(node);
            onNodeAddedToPattern.Invoke(node);
//			Debug.Log(node + " , added to the current pattern");
        }
    }

    private UniquePatterns checkPattern(int[] numberSequence)
    {
        for (var i = 0; i < knownPatterns.Count; i++)
        {
            Pattern knownPattern = knownPatterns[i];
            if (knownPattern.PatternArrayMatches(numberSequence))
            {
                return knownPattern.UniquePattern;
            }
        }

        return UniquePatterns.None;
    }
}