using UnityEngine;

public class SoundOnBirth : MonoBehaviour {
	public AudioClip[] Clips;

	void Start () {
		AudioSource audio = GetComponent<AudioSource>();
		Debug.Assert(audio != null);
		AudioClip clip = Clips[Random.Range(0, Clips.Length)];

		audio.clip = clip;
		audio.Play();

		Destroy(this);
	}
}
