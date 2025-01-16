using UnityEngine;
using Unity.Netcode;

public class MoveBall : NetworkBehaviour
{

    [SerializeField] private GameObject particleEffect;
    [SerializeField] private float force;
    public ShootBall parent;
    private float timer = 5f;
    private Rigidbody rb;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        rb.linearVelocity = rb.transform.forward * force;

        timer -= 1 * Time.deltaTime;

        if (timer < 0)
        {
            if (!IsOwner) { return; }
            parent.DestroyBallServerRpc();
        }
    }
}