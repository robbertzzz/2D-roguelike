using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using Completed;

public class DataTracker {
	private int currentLevel;
	private int levelWidth;
	private int levelHeight;
	private int enemies;
	private int playerHits = 0;
	private int foodLost = 0;
	private int foodGained = 0; //amount of food gained from pickups
	private int foodPickups = 0;//amount of food pickups picked up
	private bool gameOver = false;

	/// <summary>
	/// Initialize all the proper function callbacks
	/// </summary>
	public void Init() {
		currentLevel = GameManager.instance.level;
		enemies = GameManager.instance.enemies.Count;
		levelWidth = GameManager.instance.boardScript.columns;
		levelHeight = GameManager.instance.boardScript.rows;

		GameManager.instance.player.FoodLost += OnFoodLost;
		GameManager.instance.player.PlayerHit += OnPlayerHit;
		GameManager.instance.player.FoodGainedAmount += OnFoodGained;
		GameManager.instance.player.LevelEnded += OnLevelEnded;

		//This is the only function called when game is over
		GameManager.instance.GameOverEvent = OnGameOver;
	}

	/// <summary>
	/// Called every time the player loses food
	/// The amount of times food was lost minus the amount of player hits is the amount of steps per level
	/// </summary>
	public void OnFoodLost() {
		foodLost++;
	}

	/// <summary>
	/// Called every time the player picks up (gains) food
	/// </summary>
	public void OnFoodGained(int amount) {
		foodGained += amount;
		foodPickups++;
	}

	/// <summary>
	/// Called every time the player was hit by an enemy
	/// </summary>
	public void OnPlayerHit() {
		playerHits++;
	}

	/// <summary>
	/// Called when the player lost the game
	/// </summary>
	public void OnGameOver() {
		gameOver = true;
		SendEvent();
	}

	/// <summary>
	/// Player finished the level and wasn't game over
	/// </summary>
	public void OnLevelEnded() {
		// Send the event
		SendEvent();
	}

	private void SendEvent() {
		// Sends a new event. The title contains the current level, to keep level stats separate
		AnalyticsEvent.Custom("Level " + currentLevel, new Dictionary<string, object> {
			{ "PlayerHits", playerHits },
			{ "EnemiesCount", enemies },
			{ "GameOver", gameOver },
			{ "LevelWidth", levelWidth },
			{ "LevelHeight", levelHeight },
			{ "FoodPickedUp", foodPickups },
			{ "FoodGained", foodGained },
			{ "FoodLost", foodLost }
		});
	}
}
