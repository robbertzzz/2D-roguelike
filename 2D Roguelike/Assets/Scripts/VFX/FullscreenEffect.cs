using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class FullscreenEffect : MonoBehaviour {
	/// <summary>
	/// The name of the animation, used to identify in the animation callback event
	/// </summary>
	[SerializeField] private string animationName;

	/// <summary>
	/// Shorthand for the animation attached to the gameObject
	/// </summary>
	private Animator animator {
		get {
			return GetComponent<Animator>();
		}
	}

	/// <summary>
	/// This function plays the fullscreen effect
	/// </summary>
	public void Play() {
		gameObject.SetActive(true);
		// Play animation from the start, in case it was already playing
		animator.Play(0, -1, 0);
	}

	/// <summary>
	/// This function is called by an animation event
	/// </summary>
	public void OnAnimationEnded(string animationName) {
		// Now, let's figure out if this is our animation. If so, stop it
		if(animationName == this.animationName) {
			// Yep, it's ours!
			Stop();
		}
	}

	/// <summary>
	/// Hide the animation effect
	/// </summary>
	public void Stop() {
		gameObject.SetActive(false);
	}
}
