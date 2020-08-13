using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CreateAssetMenu(menuName = "Misc/Sound Clip", fileName = "new Sound Clip")]
public class SoundClip : ScriptableObject
{
    public List<AudioClip> clips;

    public AudioClip GetClip()
    {
        if (clips != null && clips.Count != 0)
        {
            int randomIndex = Random.Range(0, clips.Count - 1);
            return clips[randomIndex];
        }
        else
        {
            return null;
        }

    }
}
