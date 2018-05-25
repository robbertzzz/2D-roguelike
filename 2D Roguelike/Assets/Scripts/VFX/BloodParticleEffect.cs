using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Completed;

public class BloodParticleEffect : ParticleEffect {
	// Initialize delegate
	private void Start () {
		GameManager.instance.player.PlayerHit += Play;
	}
}
