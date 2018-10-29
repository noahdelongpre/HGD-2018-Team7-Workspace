using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkScript : MonoBehaviour
{

    // Use this for initialization
    CharacterController cc;
    public AudioSource audioSource;
    public AudioClip[] audioClips;
    public AudioClip currentClip;
    private bool coroutineOn;
    public float stepdelay;

    void Start()
    {
        cc = this.gameObject.GetComponent<CharacterController>();
        stepdelay = .25f;
        coroutineOn = true;
        
        audioSource.clip = audioClips[0];
        StartCoroutine(Walking());

    }

    // Update is called once per frame
    void Update()
    {
        if (cc.isGrounded == true && cc.velocity.magnitude > 2f && GetComponent<AudioSource>().isPlaying == false)
        {
            GetComponent<AudioSource>().Play();
            
        }
    }

    IEnumerator Walking()
    {
        while (coroutineOn == true){

            if (cc.velocity.magnitude > 2f) //Check to see if the player is walking?
            {
                int rand = Random.Range(0, 12);

                switch(rand)
                {
                    case 0:
                        currentClip = audioClips[0];
                        break;
                    case 1:
                        currentClip = audioClips[1];
                        break;

                    case 2:
                        currentClip = audioClips[2];
                        break;

                    case 3:
                        currentClip = audioClips[3];
                        break;

                    case 4:
                        currentClip = audioClips[4];
                        break;

                    case 5:
                        currentClip = audioClips[5];
                        break;

                    case 6:
                        currentClip = audioClips[6];
                        break;

                    case 7:
                        currentClip = audioClips[7];
                        break;

                    case 8:
                        currentClip = audioClips[8];
                        break;

                    case 9:
                        currentClip = audioClips[9];
                        break;

                    case 10:
                        currentClip = audioClips[10];
                        break;

                }

                audioSource.clip = currentClip;

                audioSource.Play();


            }

            else
            {
                audioSource.Stop();
            
            }

            yield return new WaitForSeconds(stepdelay);

        }
    }
}
