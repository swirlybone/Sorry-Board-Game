/**
 * Allows for use of background music and sound effects.
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static AudioClip draw, playcard, next, move, victory;
    static AudioSource audioSrc;

    // Load required resources at runtime.
    void Start()
    {
            draw = Resources.Load<AudioClip>("draw");
            playcard = Resources.Load<AudioClip>("playcard");
            next = Resources.Load<AudioClip>("next");
            move = Resources.Load<AudioClip>("move");
            victory = Resources.Load<AudioClip>("victory");

            audioSrc = GetComponent<AudioSource>();

    }

    void Update()
    {
        

        
    }

    // Play sound on certain conditions.
    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "draw":
                audioSrc.PlayOneShot(draw);
                break;
            case "playcard":
                audioSrc.PlayOneShot(playcard);
                break;
            case "next":
                audioSrc.PlayOneShot(next);
                break;
            case "move":
                audioSrc.PlayOneShot(move);
                break;
            case "victory":
                audioSrc.PlayOneShot(victory);
                break;
        }
    }
}
