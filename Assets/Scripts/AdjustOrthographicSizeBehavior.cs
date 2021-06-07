using System.Collections.Generic;
using UnityEngine;

public class AdjustOrthographicSizeBehavior : MonoBehaviour
{
    private bool _adjusted;

    public Renderer[] Targets;

    void Update()
    {
        if (!_adjusted)
        {
            var pointVisible = true;
            foreach (var target in Targets)
            {
                if (!target.isVisible)
                {
                    pointVisible = false;
                    break;
                }
            }

            if (!pointVisible)
                Camera.main.orthographicSize += 0.1f;
            else
                _adjusted = true;
        }
    }
}