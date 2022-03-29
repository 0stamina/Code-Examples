using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public Rigidbody2D rb;
    public Collider2D hurtbox;
    public SpriteRenderer sprite;
    public AudioSource sound;
    public Animator animator;



    public void PlayAnimation(string name)
    {
        animator.Play(name);
    }

    public void PlaySound(string name, ulong delay)
    {
        sound.clip = Resources.Load<AudioClip>(name);
        sound.Play(delay);
    }
}
