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

    private void Start()
    {
        touchInput = GetComponent<TouchPatternInput>();
        touchInput.onFinishedPattern.AddListener(clearLines);
        touchInput.onNodeAddedToPattern.AddListener(connectLine);

        line = GetComponent<LineRenderer>();
        line.startWidth = 0.2f;
        line.endWidth = 0.2f;
    }

    // TODO: REMOVE PARAMETER IF NOT USED
    private void connectLine(int targetNode)
    {
        // TODO: REMOVE THIS CODE/COMMENT
        // Heb dit geprobeerd, zelfde probleem. ga nu ipv 1 lijnstuk toevoegen hele lijn opnieuw tekenen
        int target = targetNode - 1;
        Vector3 newPosition = nodes[target].transform.position - Camera.current.transform.forward;

        line.positionCount++;
        line.SetPosition(line.positionCount - 1, newPosition);

        // just redraw the entire line
        // TODO: na een hoop geprobeer heb ik nogsteeds hetzelfde probleem :/
//        line.positionCount = 0;
//        List<int> nodeIndexes = touchInput.getCurrentPattern();
//        for (int i = 0; i < nodeIndexes.Count; i++)
//        {
//            line.positionCount++;
//
//            int index = nodeIndexes[i] - 1; // compensate for arrays starting at 0
//            Debug.Log("index: " + index + " total length" + nodes.Count);
//
//            Vector3 targetPosition = nodes[index].transform.position - Camera.current.transform.forward;
//            Debug.Log("TARGET POSITION: " + targetPosition);
//
//            line.SetPosition(i, targetPosition);
//        }

//        StopAllCoroutines();
//        StartCoroutine(drawLinesCoroutine());
    }

    private void clearLines(TouchPatternInput.UniquePatterns pattern)
    {
//        StopAllCoroutines();
        if (pattern == TouchPatternInput.UniquePatterns.None)
        {
            // TODO: add unique "poof" effect?
        }

        line.positionCount = 0;
    }

//    private IEnumerator drawLinesCoroutine()
//    {
//        yield return null;
//        List<int> indexes = touchInput.getCurrentPattern();
//
//        line.positionCount = indexes.Count;
//        for (int i = 0; i < indexes.Count; i++)
//        {
//            indexes = touchInput.getCurrentPattern();
//
//            int index = indexes[i] - 1;
//            Debug.Log(index);
//            if (index >= nodes.Count)
//            {
//                Debug.LogWarning(string.Format("Tried to access index {0}, but list has length {1}", index, nodes.Count));
//                continue;
//            }
//            Vector3 targetPosition = nodes[index].transform.position - Camera.current.transform.forward;
////            Debug.Log("TARGET POSITION: " + targetPosition);
//
//            line.SetPosition(i, targetPosition);
////            yield return null;
//        }
//    }
}
