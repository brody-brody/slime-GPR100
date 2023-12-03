using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchRandomizer : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private Vector2 pitchRange;

    private void Awake()
    {
        source.pitch = Random.Range(pitchRange.x, pitchRange.y);
    }
}
