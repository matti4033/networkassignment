using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;

public class ShootBall : NetworkBehaviour
{
    [SerializeField] private GameObject ball;
    [SerializeField] private Transform shot;

    [SerializeField] private List<GameObject> spawnedBalls = new List<GameObject>();

    void Update()
    {
        if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.H))
        {
            ShootServerRpc();
        }
    }

    [ServerRpc]
    private void ShootServerRpc()
    {
        GameObject shotBall = Instantiate(ball, shot.position, shot.rotation);
        spawnedBalls.Add(shotBall);
        shotBall.GetComponent<MoveBall>().parent = this;
        shotBall.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc(RequireOwnership = false)]
    public void DestroyBallServerRpc()
    {
        GameObject toDestroy = spawnedBalls[0];
        toDestroy.GetComponent<NetworkObject>().Despawn();
        spawnedBalls.Remove(toDestroy);
        Destroy(toDestroy);
    }
}
