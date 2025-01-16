using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private float movementSpeed = 7f;
    [SerializeField] private float rotationSpeed = 500f;
    [SerializeField] private float positionRange = 3f;

    [SerializeField] GameObject smileyFace;
    [SerializeField] Transform emote;

    [SerializeField] private List<GameObject> smileyObjects = new List<GameObject>();

    public override void OnNetworkSpawn()
    {
        UpdatePositionServerRpc();
    }

    void Update()
    {
        if (!IsOwner) return;
        Movement();

        if (Input.GetKeyDown(KeyCode.B))
        {
            SmileyEmoteServerRpc();
        }
    }

    private void Movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput);
        moveDirection.Normalize();

        transform.Translate(moveDirection * movementSpeed * Time.deltaTime, Space.World);

        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdatePositionServerRpc()
    {
        transform.position = new Vector3(Random.Range(positionRange, -positionRange), 1, Random.Range(positionRange, -positionRange));
        transform.rotation = new Quaternion(0, 180, 0, 0);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SmileyEmoteServerRpc()
    {
        StartCoroutine(SmileyTimer());

        GameObject smiley = Instantiate(smileyFace, emote.position, emote.rotation);
        smileyObjects.Add(smiley);
        smiley.GetComponent<NetworkObject>().Spawn();
    }

    IEnumerator SmileyTimer()
    {
        yield return new WaitForSeconds(1);

        SmileyDestroyServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void SmileyDestroyServerRpc()
    {
        GameObject toDestroy = smileyObjects[0];
        toDestroy.GetComponent<NetworkObject>().Despawn();
        smileyObjects.Remove(toDestroy);
        Destroy(toDestroy);
    }

}
