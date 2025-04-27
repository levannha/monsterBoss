using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
  public static SoundManager Instance { get; set; }
    private AudioSource infantryAttackChannel;
    public AudioClip infantryAttackClip;
    public AudioClip infantryAttackClipShoot;
    public AudioClip bulletexplosiveBullet;
    public AudioClip explosionmachineExplosion;
    public AudioClip shootRocket;
    public AudioClip buildSystem;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        } else
        {
            Instance = this;
        }
        infantryAttackChannel = gameObject.GetComponent<AudioSource>();
        infantryAttackChannel.volume = 1f;
        infantryAttackChannel.playOnAwake = false;
    }
    public void PlayInfantryAttackSound()
    {
        if (!infantryAttackChannel.isPlaying)
        {
            infantryAttackChannel.PlayOneShot(infantryAttackClip);
        }
    }
    public void PlayInfantryAttackShootSound()
    {
        if (!infantryAttackChannel.isPlaying)
        {
            infantryAttackChannel.PlayOneShot(infantryAttackClipShoot);
        }
    }
    public void PlayBulletexplosiveBullet()
    {
        if (!infantryAttackChannel.isPlaying)
        {
            infantryAttackChannel.PlayOneShot(bulletexplosiveBullet);
        }
    }
    public void PlayExplosionmachineExplosion ()
    {
        if (!infantryAttackChannel.isPlaying)
        {
            infantryAttackChannel.PlayOneShot(explosionmachineExplosion);
        }
    }
    public void PlayShootRocket()
    {
        if (!infantryAttackChannel.isPlaying)
        {
            infantryAttackChannel.PlayOneShot(shootRocket);
        }
    }
    public void PlayBuilding()
    {
        if (!infantryAttackChannel.isPlaying)
        {
            infantryAttackChannel.PlayOneShot(buildSystem);
        }
    }
    public void Play(AudioClip music,float volume)
    {
        if (!infantryAttackChannel.isPlaying)
        {
            infantryAttackChannel.volume = volume;
            infantryAttackChannel.PlayOneShot(music);
        }
    }
}
