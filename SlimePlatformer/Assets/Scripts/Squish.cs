using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squish : MonoBehaviour
{
    public GameObject player;
    bool isLeft;

    private UpdatedPlayerMovement playerMovement;

    private void Start() {
        playerMovement = player.GetComponent<UpdatedPlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
	{

        isLeft = (player.transform.position.x < transform.position.x);

        if (other.transform != player.transform) 
            return;

        playerMovement.enabled = false;
        player.transform.localScale = new Vector3(1, 0.5f, 1);

        if (isLeft)
        {
            if (player.transform.position.y < transform.position.y)
            {
                player.transform.position += new Vector3(0, -0.2515f, 0);
            }
            else
            {
                player.transform.position += new Vector3(0, 0.2515f, 0);
            }
        }
        else
        {
            player.transform.position += new Vector3(0, -0.2515f, 0);
        }

        StartCoroutine(moveThru());
        Invoke(nameof(ResetPM), 1.1f);
    }

    IEnumerator moveThru()
    {
        float elapsedTime = 0;
        float waitTime = 1f;
        Vector3 targetPos = player.transform.position;
        Vector3 startPos = player.transform.position;

        if (isLeft)
        {
            targetPos = player.transform.position + new Vector3(6f, 0, 0);
        }
        else
        {
            targetPos = player.transform.position + new Vector3(-6f, 0, 0);
        }
        while (elapsedTime < waitTime)
        {
            player.transform.position = Vector3.Lerp(startPos, targetPos, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        player.transform.position = targetPos;
        player.transform.localScale = new Vector3(1, 1, 1);
        yield return null;
    }
    
    void ResetPM() => playerMovement.enabled = true;
}
