using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPlayer : MonoBehaviour
{
    public Color gmSurfaceColor;
    public Color gmJointsColor;
    public Color playerSurfaceColor;
    public Color playerJointsColor;

    private Player playerInfo;
    public void Awake()
    {
        playerInfo = GetComponentInParent<Player>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SkinnedMeshRenderer[] skins = GetComponentsInChildren<SkinnedMeshRenderer>();
        
        if (playerInfo.isGameMaster)
        {
            skins[0].material.color = gmJointsColor;
            skins[1].material.color = gmSurfaceColor;
        }
        else
        {
            skins[0].material.color = playerJointsColor;
            skins[1].material.color = playerSurfaceColor;
        }
    }


}
