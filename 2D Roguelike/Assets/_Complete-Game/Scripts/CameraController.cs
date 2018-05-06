using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Completed {
	public class CameraController : MonoBehaviour {
		// The active player
		private Player player {
			get {
				return GameManager.instance.player;
			}
		}

		// The current board
		private BoardManager boardScript {
			get {
				return GameManager.instance.boardScript;
			}
		}

		// The camera's orthographic size
		private float size {
			get {
				return GetComponent<Camera>().orthographicSize;
			}
		}

		// In LateUpdate, to make sure it's always called after the player has moved
		private void LateUpdate() {
			if(player) {
				float camX, camY;
				camX = Mathf.Clamp(player.transform.position.x + .5f, .5f * size + 1f, boardScript.columns - .5f * size - 2);
				camY = Mathf.Clamp(player.transform.position.y + .5f, .5f * size + 1f, boardScript.rows - .5f * size - 2);
				transform.position = new Vector3(camX, camY, transform.position.z);
			}
		}
	}
}