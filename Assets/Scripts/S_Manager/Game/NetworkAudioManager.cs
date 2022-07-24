using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

public enum AudioFileType
{
    walking,
    ability,
}
public class NetworkAudioManager : NetworkBehaviour
{
    [SerializeField] private GameObject audioPrefab;
    [SerializeField] private List<AudioFile> audioFiles;

    /// <summary>
    /// Plays a audioFile on the server by taking in the index and the AudioFileType and the index. The index is
    /// responsible if you've got several attack sounds you can tag them with the index to find the one you want to play
    /// </summary>
    /// <param name="position"></param>
    /// <param name="type"></param>
    /// <param name="index"></param>
    public void PlayServerAudioFile(Vector3 position, AudioFileType type, int index)
    {
        if (hasAuthority)
        {
            CmdCreateAudioObject(position, type,index);
        }
    }
    
    public void PlayServerAudioFile(Vector3 position, AudioFileType type, AudioClip clip)
    {
        if (hasAuthority)
        {
            foreach (var audioFile in audioFiles)
            {
                if (audioFile.type == type && audioFile.audioClips.Contains(clip))
                {
                    CmdCreateAudioObject(position, type, audioFile.Index);
                }
            }
        }
    }
    /// <summary>
    /// Same as server but just local
    /// </summary>
    /// <param name="type"></param>
    /// <param name="index"></param>
    //public void PlayLocalAudioFile(AudioFileType type,int index)
    //{
    //    var instance = Instantiate(audioPrefab, transform.position, Quaternion.identity);
    //    var audioSource = instance.GetComponent<AudioSource>();
    //    foreach (var audioFile in audioFiles)
    //    {
    //        if (audioFile.type == type && audioFile.CanPlayAudioFile() && audioFile.Index == index)
    //        {
    //            audioSource.PlayOneShot(audioFile.audioClips);
    //            Destroy(instance,audioFile.audioClips.length);
    //            return;
    //        }
    //    }
    //}
    //
    //public void PlayLocalAudioFile(AudioFileType type, AudioClip clip)
    //{
    //    var instance = Instantiate(audioPrefab, transform.position, Quaternion.identity);
    //    var audioSource = instance.GetComponent<AudioSource>();
    //    foreach (var audioFile in audioFiles)
    //    {
    //        if (audioFile.type == type && audioFile.CanPlayAudioFile() && audioFile.audioClips == clip)
    //        {
    //            audioSource.PlayOneShot(audioFile.audioClips);
    //            Destroy(instance, audioFile.audioClips.length);
    //            return;
    //        }
    //    }
    //}

    private int FindIndex(AudioFile audioFile)
    {
        if (audioFile.randomPlayback)
            return Random.Range(0, audioFile.audioClips.Count);
        
        return audioFile.lastClipIndex < audioFile.audioClips.Count ? audioFile.lastClipIndex : 0;
    }
    
    [Command]
    private void CmdCreateAudioObject(Vector3 position, AudioFileType audioFileType, int index)
    {
        foreach (var audioFile in audioFiles)
        {
            if (audioFile.type == audioFileType && audioFile.CanPlayAudioFile() && audioFile.Index == index)
            {
                var go = Instantiate(audioPrefab,position,Quaternion.identity);
                NetworkServer.Spawn(go);
                RpcPlayAudioOnServer(go,audioFileType,audioFile.Index);
                return;
            }
        }
    }

    [ClientRpc]
    private void RpcPlayAudioOnServer(GameObject audioObject, AudioFileType audioFileType, int index)
    {
        foreach (var audioFile in audioFiles)
        {
            if (audioFile.type == audioFileType && audioFile.CanPlayAudioFile() && audioFile.Index == index)
            {
                var audioSource = audioObject.GetComponent<AudioSource>();
                var listIndex = FindIndex(audioFile);
                var audioClip = audioFile.audioClips[listIndex];


                audioFile.lastClipIndex = listIndex + 1;
                audioSource.pitch = Random.Range(audioFile.audioClipPitchMin, audioFile.audioClipPitchMax);
                audioSource.volume = audioFile.volume;
                audioSource.spatialBlend = audioFile.spatialBlend;
                audioSource.PlayOneShot(audioClip);
                audioFile.SetTimer(listIndex);
                
                Destroy(audioObject,audioFile.audioClips[listIndex].length);
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
    
    public AudioFileType type;
    public List<AudioClip> audioClips;
    
    [Range(0f, 2f)] public float audioClipPitchMin = .8f;
    [Range(0f, 2f)] public float audioClipPitchMax = 1.2f;
    [Range(0f, 1f)] public float volume = .5f;
    [Range(0f, 1f)] public float spatialBlend = 1f;
    
    public bool randomPlayback;
    public int Index => index;
    private float _lastTimePlayed;
    [HideInInspector] public int lastClipIndex;
    
    public void SetTimer(int listIndex)
    {
        _lastTimePlayed = Time.time;
        _lastTimePlayed += audioClips[listIndex].length;
    }

    public bool CanPlayAudioFile()
    {
        return Time.time > _lastTimePlayed;
    }
}