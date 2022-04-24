using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CreateAssetMenu(fileName = "New Movement Sound Collection", menuName = "Create New Movement Sound Container")]
public class MovementSoundContainer : ScriptableObject
{
    public List<AudioClip> footstepSounds = new List<AudioClip>();
    public AudioClip jumpSound;
    public AudioClip landSound;
    public AudioClip crouchSound;
}
