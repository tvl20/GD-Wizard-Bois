using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SpellUnityEvent : UnityEvent<Spell> { }

public class NodeAddedEvent : UnityEvent<int> { }

public class TouchPatternInput : MonoBehaviour
{
    public SpellUnityEvent OnFinishedSpell = new SpellUnityEvent();
    public NodeAddedEvent onNodeAddedToPattern = new NodeAddedEvent();

    [SerializeField] private List<Spell> allSpells;

    private List<int> currentPattern;

    private bool finishedPatternFrame = false;

    private bool gameActive = false;

    private void Start()
    {
        currentPattern = new List<int>();
    }

    private void Update()
    {
        if (finishedPatternFrame) finishedPatternFrame = false;

        if ((Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended) || Input.GetMouseButtonUp(0))
        {
            Spell spell = checkPatternForSpell(currentPattern.ToArray());

            OnFinishedSpell.Invoke(spell);
            currentPattern.Clear();
            finishedPatternFrame = true;
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            OnFinishedSpell.Invoke(allSpells[0]);
            currentPattern.Clear();
            finishedPatternFrame = true;
        }
    }

    public void addNodeToCurrentPattern(int node)
    {
        if (gameActive && !currentPattern.Contains(node) && !finishedPatternFrame)
        {
            currentPattern.Add(node);
            onNodeAddedToPattern.Invoke(node);
        }
    }

    public void startGame()
    {
        gameActive = true;
    }

    private Spell checkPatternForSpell(int[] numberSequence)
    {
        for (var i = 0; i < allSpells.Count; i++)
        {
            Spell knownSpell = allSpells[i];
            if (knownSpell.PatternArrayMatches(numberSequence))
            {
                return knownSpell;
            }
        }

        return null;
    }
}