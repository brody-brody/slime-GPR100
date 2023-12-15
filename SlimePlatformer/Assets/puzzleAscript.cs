using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class puzzleAscript : MonoBehaviour
{
    public GameObject nodeA;
    public GameObject nodeB;
    public GameObject nodeC;

    public AudioClip lowPitch;
    public AudioClip midPitch;
    public AudioClip highPitch;

    public Sprite normalSprite;
    public Sprite lowSprite;
    public Sprite midSprite;
    public Sprite highSprite;

    public GameObject player;

    bool puzzleStarted = false;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(puzzleStarted == false && player.transform.position.x > 11 )
        {
            puzzleStarted = true;
            StartCoroutine(PlaySoundAfterDelay(2f));
            Debug.Log("waiting");
        }
    }
    IEnumerator PlaySoundAfterDelay(float delaySeconds)
    {
        Debug.Log("PLAYING");
        // Wait for the specified number of seconds
        yield return new WaitForSeconds(delaySeconds);

        // Play the assigned sound clip
        if (audioSource != null && lowPitch != null)
        {
            audioSource.clip = lowPitch;
            audioSource.Play();
        }
        else
        {
            Debug.Log("AudioSource or soundClip not assigned!");
        }
    }
}
