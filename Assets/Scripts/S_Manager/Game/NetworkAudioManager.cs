using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public enum AudioFileType
{
    walking
}
public class NetworkAudioManager : NetworkBehaviour
{
    [SerializeField] GameObject audioPrefab;
    [SerializeField] List<AudioFile> audioFiles;

    /// <summary>
    /// Plays a audioFIle on the server by Taking in the index and the AudioFileType and the index the index is responsible if youve got several attack sounds you can tag them with the index to find the one you want to play
    /// </summary>
    /// <param name="type"></param>
    /// <param name="index"></param>
    public void PlayServerAudioFile(AudioFileType type,int index)
    {
        if (hasAuthority)
        {
            CreateAudioObject(transform.position, type,index);
        }
    }
    /// <summary>
    /// Same as server but just local
    /// </summary>
    /// <param name="type"></param>
    /// <param name="index"></param>
    public void PlayLocalAudioFile(AudioFileType type,int index)
    {
        var instance = Instantiate(audioPrefab, transform.position, Quaternion.identity);
        var audioSource = instance.GetComponent<AudioSource>();
        foreach (var audioFile in audioFiles)
        {
            if (audioFile.type == type && audioFile.CanPlayAudioFile() && audioFile.Index == index)
            {
                audioSource.PlayOneShot(audioFile.audioClip);
                Destroy(instance,audioFile.audioClip.length);
                return;
            }
        }
    }
    [Command]
    void CreateAudioObject(Vector3 position,AudioFileType audioFileType,int index)
    {
        foreach (var audioFile in audioFiles)
        {
            if (audioFile.type == audioFileType && audioFile.CanPlayAudioFile() && audioFile.Index == index)
            {
                var go = Instantiate(audioPrefab,position,Quaternion.identity);
                NetworkServer.Spawn(go); 
                PlayAudioOnServer(go,audioFileType,audioFile.Index);
                return;
            }
        }

    }

    [ClientRpc]
    void PlayAudioOnServer(GameObject audioObject,AudioFileType audioFileType,int index)
    {
        foreach (var audioFile in audioFiles)
        {
            if (audioFile.type == audioFileType && audioFile.CanPlayAudioFile() && audioFile.Index == index)
            {
                var audioSource = audioObject.GetComponent<AudioSource>();
                audioSource.PlayOneShot(audioFile.audioClip);
                audioFile.SetTimer();
                Destroy(audioObject,audioFile.audioClip.length);
                return;
            }
        }
       
    }
}
[System.Serializable]
public class AudioFile
{
    [Tooltip("This is the solution icam up with if youve got several attacks sounds or things tagged with the same enum")]
    [SerializeField]int index;
    public int Index{ get => index; }
    public AudioFileType type;
    public AudioClip audioClip;
    float lastTimePLayed;
    
    public void SetTimer()
    {
        lastTimePLayed = Time.time;
        lastTimePLayed += audioClip.length;
    }

    public bool CanPlayAudioFile()
    {
        if (Time.time > lastTimePLayed)
        {
            return true;
        }
        else return false;
    }

}
