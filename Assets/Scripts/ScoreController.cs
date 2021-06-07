using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ScoreController : MonoBehaviour
{
    public Sprite[] Sprites;

    public void Set(int score)
    {
        if (score >= 0 && score <= 11)
            GetComponent<SpriteRenderer>().sprite = Sprites[score];
    }
}
