using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(TouchPatternInput), typeof(LineRenderer))]
public class TouchPatternLines : MonoBehaviour
{
    public List<GameObject> nodes;

    private TouchPatternInput touchInput;
    private LineRenderer line;
    private Camera mainCamera;

    private bool gameActive = false;

    private void Start()
    {
        mainCamera = Camera.main;

        touchInput = GetComponent<TouchPatternInput>();
        touchInput.OnFinishedSpell.AddListener(clearLines);
        touchInput.onNodeAddedToPattern.AddListener(connectLine);

        line = GetComponent<LineRenderer>();
    }

    // TODO: REMOVE PARAMETER IF NOT USED
    private void connectLine(int targetNode)
    {
        // TODO: REMOVE THIS CODE/COMMENT
        // Heb dit geprobeerd, zelfde probleem. ga nu ipv 1 lijnstuk toevoegen hele lijn opnieuw tekenen
        int target = targetNode - 1;
        Vector3 newPosition = nodes[target].transform.position - mainCamera.transform.forward;

        line.positionCount++;
        line.SetPosition(line.positionCount - 1, newPosition);
    }

    private void clearLines(Spell spell)
    {
        if (spell == null)
        {
            // TODO: add unique "poof" effect?
        }

        line.positionCount = 0;
    }
}
