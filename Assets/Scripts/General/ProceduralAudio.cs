using UnityEngine;

public class ProceduralAudio : MonoBehaviour {
	AudioSource[] audioSources;

	void Awake () {
		audioSources = GetComponentsInChildren<AudioSource>();
	}

	public void PlayRandom () {
		int i;
		int j = 0;
		do {
			i = Random.Range(0, audioSources.Length);
			if (j++ > 10) {
				Debug.LogError("Could not find a free audio source");
				return;
			}
		} while (audioSources[i].isPlaying);
		audioSources[i].Play();
	}

	public void Noop () {
		// Nonce to make sure the animation does not end prematurely
	}
}
