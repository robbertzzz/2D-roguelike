using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect : MonoBehaviour {

	/// <summary>
	/// Play the particle effect
	/// </summary>
	public void Play() {
		GetComponent<ParticleSystem>().Play(true);
	}
}
