using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class feet : MonoBehaviour
{
    public SpriteRenderer spr;

    public void flipFeet(bool truth)
    {
        spr.flipX = truth;
    
    }
}
