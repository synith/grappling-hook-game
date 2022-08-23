using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets instance;
    public static GameAssets Instance
    {
        get
        {
            if (instance == null)
                instance = Resources.Load<GameAssets>("GameAssets");
            return instance;
        }
    }

    public Transform pf_CollectableEffect;
    public Transform pf_LandingEffect;
    public Transform pf_GrappleHitEffect;
}
