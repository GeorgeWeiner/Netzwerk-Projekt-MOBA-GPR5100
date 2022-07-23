using System.Collections.Generic;
using Mirror;
using UnityEngine;

public enum AudioFileType
{
    walking,
    ability
}
public class NetworkAudioManager : NetworkBehaviour
{
    [SerializeField] private GameObject audioPrefab;
    [SerializeField] private List<AudioFile> audioFiles;

    /// <summary>
    /// Plays a audioFile on the server by taking in the index and the AudioFileType and the index. The index is
    /// responsible if you've got several attack sounds you can tag them with the index to find the one you want to play
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
    private void CreateAudioObject(Vector3 position, AudioFileType audioFileType, int index)
    {
        foreach (var audioFile in audioFiles)
        {
            if (audioFile.type == audioFileType && audioFile.CanPlayAudioFile() && audioFile.Index == index)
            {
                Debug.Log("HEEYPLAYERD");
                var go = Instantiate(audioPrefab,position,Quaternion.identity);
                NetworkServer.Spawn(go); 
                PlayAudioOnServer(go,audioFileType,audioFile.Index);
                return;
            }
        }

    }

    [ClientRpc]
    private void PlayAudioOnServer(GameObject audioObject, AudioFileType audioFileType, int index)
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
    [Tooltip("This is the solution I came up with if you've got several attacks sounds or things tagged with the same enum")]
    [SerializeField] private int index;
    public int Index => index;
    public AudioFileType type;
    public AudioClip audioClip;
    private float _lastTimePlayed;
    
    public void SetTimer()
    {
        _lastTimePlayed = Time.time;
        _lastTimePlayed += audioClip.length;
    }

    public bool CanPlayAudioFile()
    {
        return Time.time > _lastTimePlayed;
    }

}
