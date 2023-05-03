using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluateAshtray : MonoBehaviour
{
    [ContextMenu("Eval")]
    public void Evaluate()
    {
        int trayCount = 0, ashCount = 0, cigCount = 0;
        for(int i = 0; i < transform.childCount; ++i)
        {
            Transform child = transform.GetChild(i);

            if (child.name.StartsWith("Ash"))
            {
                ashCount++;
            }

            else if (child.name.StartsWith("cigarette"))
            {
                cigCount++;
            }

            else if (child.name.StartsWith("tray"))
            {
                trayCount++;
            }
        }

        Debug.Log($"Tray Count: {trayCount}, CigCount: {cigCount}, AshCount: {ashCount}");
    }

    [ContextMenu("Simplify")]
    public void SimplifyMesh()
    {
        int ashCount = 0, cigCount = 0;
        for (int i = 0; i < transform.childCount; ++i)
        {
            Transform child = transform.GetChild(i);

            if (child.name.StartsWith("Ash"))
            {
                if (ashCount % 3 != 0)
                {
                    child.gameObject.SetActive(false);
                }
                ashCount++;
            }

            else if (child.name.StartsWith("cigarette"))
            {
                if (cigCount % 2 == 0)
                {
                    child.gameObject.SetActive(false);
                }
                cigCount++;
            }
        }
    }
}
