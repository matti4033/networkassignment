using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using System.Collections;
using System.Collections.Generic;

public class PlayerSettings : NetworkBehaviour
{
    public MeshRenderer mR;

    public List<Color> colors = new List<Color>();

    private void Awake()
    {
        //mR = GetComponent<MeshRenderer>();
    }

    public override void OnNetworkSpawn()
    {
        mR.material.color = colors[(int)OwnerClientId];
    }

}
