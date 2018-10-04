using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PatternUnityEvent : UnityEvent<TouchPatternInput.UniquePatterns> { }

public class IntUnityEvent : UnityEvent<int> { }

public class TouchPatternInput : MonoBehaviour
{
	public PatternUnityEvent onFinishedPattern = new PatternUnityEvent();
	public IntUnityEvent onNodeAddedToPattern = new IntUnityEvent();

	public enum UniquePatterns
	{
		None,
		VerticalLine,
		HorizontalLine,
		HighHorizontalLine
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
		{new Pattern(UniquePatterns.VerticalLine, new int[] {2, 5, 8})},
		{new Pattern(UniquePatterns.HorizontalLine, new int[] {4, 5, 6})},
		{new Pattern(UniquePatterns.HighHorizontalLine, new int[] {1, 2, 3})}
	};

	private List<int> currentPattern;
	public List<int> getCurrentPattern()
	{
		//return a copy of the list
		return new List<int>(currentPattern);
	}

	private bool finishedPatternFrame = false;

	private void Start () {
		currentPattern = new List<int>();
	}

	private void Update ()
	{
		if (finishedPatternFrame) finishedPatternFrame = false;
		if (Input.touchCount <= 0) return;

		if (Input.touches[0].phase == TouchPhase.Ended)
		{
			UniquePatterns pattern = checkPattern(currentPattern.ToArray());
			Debug.Log(pattern);

			string output = "";
			foreach (int i in currentPattern)
			{
				output += i + ", ";
			}
			Debug.Log(output);

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
		foreach (Pattern knownPattern in knownPatterns)
		{
			if (knownPattern.PatternArrayMatches(numberSequence))
			{
				return knownPattern.UniquePattern;
			}
		}

		return UniquePatterns.None;
	}
}
