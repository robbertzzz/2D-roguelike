using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Completed;

public class ScreenOverlay : MonoBehaviour {
	[SerializeField] private float maxOpacity = .7f;
	[SerializeField] private int foodThreshold = 10;

	private float _opacity = 0f;
	private float opacity {
		get {
			return _opacity;
		}

		set {
			_opacity = value;
			//Update actual opacity
			GetComponent<Image>().color = new Color(GetComponent<Image>().color.r, GetComponent<Image>().color.g, GetComponent<Image>().color.b, value);
		}
	}

	public void Init() {
		GameManager.instance.player.FoodLost += OnFoodChanged;
		GameManager.instance.player.FoodGained += OnFoodChanged;
	}

	/// <summary>
	/// Apply the effect, if threshold has been reached
	/// </summary>
	public void OnFoodChanged() {
		if(GameManager.instance.player.food <= foodThreshold) {
			opacity = Mathf.Lerp(maxOpacity, 0f, GameManager.instance.player.food / (foodThreshold + 1f));
		} else {
			// If the player regained food
			opacity = 0f;
		}
	}
}
