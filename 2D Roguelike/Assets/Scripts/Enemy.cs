using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Completed
{
	//Enemy inherits from MovingObject, our base class for objects that can move, Player also inherits from this.
	public class Enemy : MovingObject
	{
		public int playerDamage; 							//The amount of food points to subtract from the player when attacking.
		public AudioClip attackSound1;						//First of two audio clips to play when attacking the player.
		public AudioClip attackSound2;						//Second of two audio clips to play when attacking the player.
		
		
		private Animator animator;							//Variable of type Animator to store a reference to the enemy's Animator component.
		private Transform target;							//Transform to attempt to move toward each turn.
		private delegate void CurrentState();               //Delegate for the AI states
		CurrentState currentState;                          //
		[SerializeField] private int attackDistance = 5;	//The distance to the player from which the enemy starts attacking

		private Player player {
			get {
				return GameManager.instance.player;
			}
		}


		//Start overrides the virtual Start function of the base class.
		protected override void Start ()
		{
			//Register this enemy with our instance of GameManager by adding it to a list of Enemy objects. 
			//This allows the GameManager to issue movement commands.
			GameManager.instance.AddEnemyToList (this);
			
			//Get and store a reference to the attached Animator component.
			animator = GetComponent<Animator> ();
			
			//Find the Player GameObject using it's tag and store a reference to its transform component.
			target = GameObject.FindGameObjectWithTag ("Player").transform;

			//Start roaming around
			currentState = RoamMove;
			
			//Call the start function of our base class MovingObject.
			base.Start ();
		}
		
		
		//MoveEnemy is called by the GameManger each turn to tell each Enemy to try to move towards the player.
		public void MoveEnemy ()
		{
			currentState();
		}

		//Behaviour when the player is out of sight
		private void RoamMove() {
			//Check if the player is near. If so, attack.
			if(Mathf.RoundToInt((player.transform.position - transform.position).magnitude) < attackDistance) {
				currentState = AttackMove;
				currentState();
				return;
			}

			//Perform a random move
			int direction = Random.Range(0, 3);
			AttemptMove<Player>(direction < 2 ? (direction == 0 ? -1 : 1) : 0, direction > 1 ? (direction == 2 ? -1 : 1) : 0);
		}

		//Behaviour when the player is in sight
		private void AttackMove() {
			//Check if the player is still near. If not, roam around.
			if(Mathf.RoundToInt((player.transform.position - transform.position).magnitude) >= attackDistance) {
				currentState = RoamMove;
				currentState();
				return;
			}

			//Move towards the player
			int xDistance = Mathf.RoundToInt(player.transform.position.x - transform.position.x);
			int yDistance = Mathf.RoundToInt(player.transform.position.y - transform.position.y);

			//We don't need the actual distance, just the direction
			int xDir = xDistance != 0 ? (xDistance > 0 ? 1 : -1) : 0;
			int yDir = yDistance != 0 ? (yDistance > 0 ? 1 : -1) : 0;

			//Let's make it a bit random. See if we need to and are able to move in the x direction, else attempt a move in the y direction. If we're stuck in both directions, the enemy won't move.
			Vector2 start = transform.position;
			Vector2 end = transform.position + new Vector3(xDir, 0);

			//Don't check for our own collider
			GetComponent<BoxCollider2D>().enabled = false;
			RaycastHit2D hit = Physics2D.Linecast(start, end, blockingLayer);
			GetComponent<BoxCollider2D>().enabled = true;

			if((Random.Range(0, 2) == 0 || yDir == 0) && xDir != 0 && (hit.transform == null || hit.transform.GetComponent<Player>())) {
				AttemptMove<Player>(xDir, 0);
			} else {
				AttemptMove<Player>(0, yDir);
			}
		}
		
		
		//OnCantMove is called if Enemy attempts to move into a space occupied by a Player, it overrides the OnCantMove function of MovingObject 
		//and takes a generic parameter T which we use to pass in the component we expect to encounter, in this case Player
		protected override void OnCantMove <T> (T component)
		{
			//Declare hitPlayer and set it to equal the encountered component.
			Player hitPlayer = component as Player;
			
			//Call the LoseFood function of hitPlayer passing it playerDamage, the amount of foodpoints to be subtracted.
			hitPlayer.LoseFood (playerDamage);
			
			//Set the attack trigger of animator to trigger Enemy attack animation.
			animator.SetTrigger ("enemyAttack");
			
			//Call the RandomizeSfx function of SoundManager passing in the two audio clips to choose randomly between.
			SoundManager.instance.RandomizeSfx (attackSound1, attackSound2);
		}
	}
}
