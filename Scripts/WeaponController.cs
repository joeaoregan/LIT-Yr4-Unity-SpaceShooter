using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {

    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate;
    public float delay;                                             // Don't want enemy firing straight away

    private AudioSource audioSource;

	void Start () {
        audioSource = GetComponent<AudioSource>();                  // Finding the audio source on the same GameObject as the script
        InvokeRepeating("Fire", delay, fireRate);                   // Invoke a method by its method name
	}

    void Fire()
    {
        Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        audioSource.Play();
    }
}
