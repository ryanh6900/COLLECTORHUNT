using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreciseFootstep : MonoBehaviour
{
    public AudioSource groundSoundSource;
    public AudioClip[] groundSounds;
    private bool step = true;
    public float audioStepLength = 0.45f;
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        CharacterController controller = GetComponent<CharacterController>();
        if (controller.isGrounded && controller.velocity.magnitude < 7 && controller.velocity.magnitude > 5 && hit.gameObject.tag == "Ground" && step == true)
            WalkOnGround();
    }
    
   void WalkOnGround()
    {
        step = false;
        groundSoundSource.volume = 0.1f;
        groundSoundSource.PlayOneShot(groundSounds[Random.Range(0, groundSounds.Length)]);
        //yield WaitForSeconds(audioStepLength);
        step = true;
    }
    
}
