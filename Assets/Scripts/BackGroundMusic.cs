using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMusic : MonoBehaviour
{
    [SerializeField] private AudioSource backgroundmusicSource;
    public AudioSource battleMusicSource;
    public AudioSource bossMusicSource;

    public AudioClip[] battleMusics;
    public AudioClip bossMusic;

    // Start is called before the first frame update
    
    public void PlayBattleMusic()
    {
        int index = Random.Range(0, battleMusics.Length);
        battleMusicSource.clip = battleMusics[index];
        battleMusicSource.Play();
    }

    public void StopBattleMusic()
    {
        battleMusicSource.Stop();
    }

}