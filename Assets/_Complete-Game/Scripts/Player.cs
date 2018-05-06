using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEditorInternal;

//Allows us to use UI.
using UnityEngine.SceneManagement;

namespace Completed
{
	//Player inherits from MovingObject, our base class for objects that can move, Enemy also inherits from this.
	public class Player : MovingObject, IComponent
	{
		public float restartLevelDelay = 0f;
		//Delay time in seconds to restart level.
		public int pointsPerFood = 10;
		//Number of points to add to player food points when picking up a food object.
		public int pointsPerSoda = 20;
		//Number of points to add to player food points when picking up a soda object.
		public int wallDamage = 1;
		//How much damage a player does to a wall when chopping it.
		public Text foodText;
        //UI Text to display current player food total.
        public Text timeText;
        public System.DateTime dt;
        public System.TimeSpan ts;
        public bool isChestOpen;
        //UI Text to display current player food total.
        public AudioClip moveSound1;
		//1 of 2 Audio clips to play when player moves.
		public AudioClip moveSound2;
		//2 of 2 Audio clips to play when player moves.
		public AudioClip eatSound1;
		//1 of 2 Audio clips to play when player collects a food object.
		public AudioClip eatSound2;
		//2 of 2 Audio clips to play when player collects a food object.
		public AudioClip drinkSound1;
		//1 of 2 Audio clips to play when player collects a soda object.
		public AudioClip drinkSound2;
		//2 of 2 Audio clips to play when player collects a soda object.
		public AudioClip gameOverSound;
        //Audio clip to play when player dies.
        public float speed = .1f;
        
		public float lastAttackTime;
		private Animator animator;
		//Used to store a reference to the Player's animator component.
		private int direction;
		public int health;
        //Used to store player food points total during level.	
        private bool hasKey;
        //Used to store if the player has a key during the level
        public static Player instance = null;	

        //dependency injections, etc. for unit testing
        IComponent componentProvider;
        public IGameManager gameManagerService;
        public RigidBodyWrapper rigidBody;
        public bool attackTracker;
		
		//Start overrides the Start function of MovingObject
		protected override void Start ()
		{
            if(componentProvider == null)
            {
                //Get a component reference to the Player's animator component
                componentProvider = this;
            }
            
            if(gameManagerService == null) {
                gameManagerService = GameManager.instance;
            }

            rigidBody = new RigidBodyWrapper(this);

            attackTracker = false;

            animator = componentProvider.GetComponent<Animator> ();

			//Get the current food point total stored in GameManager.instance between levels.
			health = gameManagerService.playerFoodPoints;

            if (foodText != null)
            {
                //Set the foodText to reflect the current player food total.
                foodText.text = "Health: " + health;
            }

            if (GameManager.instance != null)
            {

                //checks if player has a key from the GameManager between levels
                hasKey = GameManager.instance.playerHasKey;

                //gets the starting time of the game
                dt = GameManager.instance.startingTime;
            }
            isChestOpen = false;
            
            updateTime();
            //sets the timeText to reflect the currents time
            //timeText.text = "ASDASDASDSAD"; //for testing purposes
            lastAttackTime = -5;

            //Call the Start function of the MovingObject base class.
            base.Start();
        }

        private void updateTime()
        {
            ts = System.DateTime.Now.ToUniversalTime() - dt;
            timeText.text = "Time: " + ((int)(ts.TotalSeconds)).ToString();
        }


        //This function is called when the behaviour becomes disabled or inactive.
        private void OnDisable ()
		{
			//When Player object is disabled, store the current local food total in the GameManager so it can be re-loaded in next level.
			GameManager.instance.playerFoodPoints = health;
		}

		private void DoPlayerMovement ()
		{
			float horizontal = 0;  	//Used to store the horizontal move direction.
			float vertical = 0;		//Used to store the vertical move direction.

			if (InputWrapper.GetKey (KeyCode.W)) {
				vertical = 1;
			} else if (InputWrapper.GetKey (KeyCode.S)) {
				vertical = -1;
			}

			if (InputWrapper.GetKey (KeyCode.A)) {
				horizontal = -1;
			} else if (InputWrapper.GetKey (KeyCode.D)) {
				horizontal = 1;
			}

			float totalDis = Mathf.Sqrt (Mathf.Pow (horizontal, 2) + Mathf.Pow (vertical, 2));
			float ratio = totalDis / speed;

			float newX = transform.position.x + horizontal / ratio;
			float newY = transform.position.y + vertical / ratio;

			BoxCollider2D boxCollider = componentProvider.GetComponent <BoxCollider2D> ();

			if (float.IsNaN (newX)) {
				newX = transform.position.x;
			}

			if (float.IsNaN (newY)) {
				newY = transform.position.y;
			}

			//Store start position to move from, based on objects current transform position.
			Vector2 start = transform.position;

			// Calculate end position based on the direction parameters passed in when calling Move.
			Vector2 end = new Vector2 (newX, newY);

            bool hitAWall = false;
            if (boxCollider != null)
            {

                //Disable the boxCollider so that linecast doesn't hit this object's own collider. Shouldn't matter with LinecastAll though
                boxCollider.enabled = false;

                //Cast a line from start point to end point checking collision on blockingLayer.
                RaycastHit2D[] hits = Physics2D.LinecastAll(start, end, blockingLayer);

                //Re-enable boxCollider after linecast
                boxCollider.enabled = true;

                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.transform.tag == "Wall")
                    {
                        hitAWall = true;
                        break;
                    }
                }
            }

			if (!hitAWall) {
				//If nothing was hit, move player to destination
				rigidBody.MovePosition (new Vector2 (newX, newY));
				return;
			}

			//can't move diagonally into walls, must move straight vertical or horizontal, so check if that's possible and do it

			//test vertical first
			if (vertical > 0) {
				newY = transform.position.y + speed;
			} else if (vertical < 0) {
				newY = transform.position.y - speed;
			}
			newX = transform.position.x;
			end = new Vector2 (newX, newY);
			boxCollider.enabled = false;
			RaycastHit2D[] hits2 = Physics2D.LinecastAll (start, end, blockingLayer);
			hitAWall = false;
			foreach (RaycastHit2D hit in hits2) {
				if (hit.transform.tag == "Wall") {
					hitAWall = true;
					break;
				}
			}
			boxCollider.enabled = true;
			if (!hitAWall) {
				rigidBody.MovePosition (new Vector2 (newX, newY));
				return;
			}

			//test horizontal second
			if (horizontal > 0) {
				//newX is already set to the current position
				newX += speed;
			} else if (horizontal < 0) {
				newX -= speed;
			}
			newY = transform.position.y;
			end = new Vector2 (newX, newY);
			boxCollider.enabled = false;
			hits2 = Physics2D.LinecastAll (start, end, blockingLayer);
			hitAWall = false;
			foreach (RaycastHit2D hit in hits2) {
				if (hit.transform.tag == "Wall") {
					hitAWall = true;
					break;
				}
			}
			boxCollider.enabled = true;
			if (!hitAWall) {
				rigidBody.MovePosition (new Vector2 (newX, newY));
				return;
			}
		}

		private void AttemptAttack (string direction)
		{
			float currentTime = Time.time;
			float delay = 1.5f;
			float range = .75f;
			if (currentTime - lastAttackTime < delay) {
				return; //don't allow attacks before delay is over
			}
            attackTracker = true;
			lastAttackTime = currentTime;
			//do an attack by finding all enemies in range and killing them
            RaycastHit2D[] inRange = Physics2D.CircleCastAll(new Vector2(transform.position.x, transform.position.y), range, new Vector2(0, 0), 0);

			foreach (RaycastHit2D hit in inRange) {
				if (hit.transform.tag == "Enemy") {
					//check angle
					float vDiff = hit.transform.position.y - transform.position.y;
					float hDiff = hit.transform.position.x - transform.position.x;
					bool canAttack = false;

					if (direction == "Up" && vDiff >= 0 && vDiff >= Mathf.Abs(hDiff)) {
						canAttack = true;
					} else if (direction == "Down" && vDiff <= 0 && vDiff * -1 >= Mathf.Abs(hDiff)) {
						canAttack = true;
					} else if (direction == "Right" && hDiff >= 0 && hDiff >= Mathf.Abs(vDiff)) {
						canAttack = true;
					} else if (direction == "Left" && hDiff <= 0 && hDiff * -1 >= Mathf.Abs(vDiff)) {
						canAttack = true;
					}

					if (canAttack) {
						Enemy thisEnemy = hit.transform.gameObject.GetComponent<Enemy> ();
						thisEnemy.Kill ();
					}
				}
			}
		}

		public void Update ()
		{
			//do player attacks
			if (InputWrapper.GetKey (KeyCode.UpArrow)) {
				AttemptAttack ("Up");
			} else if (InputWrapper.GetKey (KeyCode.RightArrow)) {
				AttemptAttack ("Right");
			} else if (InputWrapper.GetKey (KeyCode.DownArrow)) {
				AttemptAttack ("Down");
			} else if (InputWrapper.GetKey (KeyCode.LeftArrow)) {
				AttemptAttack ("Left");
			}
            updateTime();
			
			DoPlayerMovement ();
		}		

		
		//OnCantMove overrides the abstract function OnCantMove in MovingObject.
		//It takes a generic parameter T which in the case of Player is a Wall which the player can attack and destroy.
		protected override void OnCantMove <T> (T component)
		{
			//Set hitWall to equal the component passed in as a parameter.
			Wall hitWall = component as Wall;
			
			//Call the DamageWall function of the Wall we are hitting.
			hitWall.DamageWall (wallDamage);
			
			//Set the attack trigger of the player's animation controller in order to play the player's attack animation.
			animator.SetTrigger ("playerChop");
		}
		
		
		//OnTriggerEnter2D is sent when another object enters a trigger collider attached to this object (2D physics only).
		public void OnTriggerEnter2D (Collider2D other)
		{
            //Check if the tag of the trigger collided with is Exit.
            if (other.tag == "Exit" && hasKey == true) {
                //Invoke the Restart function to start the next level with a delay of restartLevelDelay (default 1 second).
                //animator.SetTrigger("OpenChest");
                isChestOpen = true;
				Invoke ("Restart", restartLevelDelay);

				//Disable the player object since level is over.
				enabled = false;
			}

			//Check if the tag of the trigger collided with is Food.
			else if (other.tag == "Food") {
				//Add pointsPerFood to the players current food total.
				health += pointsPerFood;

				//Update foodText to represent current total and notify player that they gained points
				foodText.text = "+" + pointsPerFood + " Health: " + health;

				//Call the RandomizeSfx function of SoundManager and pass in two eating sounds to choose between to play the eating sound effect.
				SoundManager.instance.RandomizeSfx (eatSound1, eatSound2);

				//Disable the food object the player collided with.
				other.gameObject.SetActive (false);
			}

			//Check if the tag of the trigger collided with is Soda.
			else if (other.tag == "Soda") {
				//Add pointsPerSoda to players food points total
				health += pointsPerSoda;

				//Update foodText to represent current total and notify player that they gained points
				foodText.text = "+" + pointsPerSoda + " Health: " + health + "\tYou picked up a key!";
                hasKey = true;

				//Call the RandomizeSfx function of SoundManager and pass in two drinking sounds to choose between to play the drinking sound effect.
				SoundManager.instance.RandomizeSfx (drinkSound1, drinkSound2);

				//Disable the soda object the player collided with.
				other.gameObject.SetActive (false);
			} else if (other.tag == "Enemy") {
				bool enemyReadyToAttack = false;
				//find which enemy
				foreach (Enemy enemy in gameManagerService.enemies) {
					if (enemy.GetComponent<BoxCollider2D> ().Equals (other)) {
						enemyReadyToAttack = enemy.isReadyToAttack ();
						break;
					}
				}

				if (enemyReadyToAttack) {
					LoseFood (20);
				}
			}
		}
		
		
		//Restart reloads the scene when called.
		private void Restart ()
		{
			//Load the last scene loaded, in this case Main, the only scene in the game. And we load it in "Single" mode so it replace the existing one
			//and not load all the scene object in the current scene.
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex, LoadSceneMode.Single);
		}


		//LoseFood is called when an enemy attacks the player.
		//It takes a parameter loss which specifies how many points to lose.
		public void LoseFood (int loss)
		{
            if (animator != null)
            {
                //Set the trigger for the player animator to transition to the playerHit animation.
                animator.SetTrigger("playerHit");
            }

			//Subtract lost food points from the players total.
			health -= loss;

            if (foodText != null)
            {
                //Update the food display with the new total.
                foodText.text = "Losing Health!" + " Health: " + health;
            }

			//Check to see if game has ended.
			CheckIfGameOver ();
		}

		public void PublicStart(){
			Start ();
		}

        public void setComponentProvider(IComponent provider)
        {
            componentProvider = provider;
        }

        public void setGameManagerService(IGameManager service)
        {
            gameManagerService = service;
        }

        /*public void setRB2DAndBoxCollider()
        {
            gameObject.AddComponent<Rigidbody2D>();
            gameObject.AddComponent<BoxCollider2D>();
        }*/


		//CheckIfGameOver checks if the player is out of food points and if so, ends the game.
		public void CheckIfGameOver ()
		{
            //ts = System.DateTime.Now.ToUniversalTime() - dt;
            //timeText.text = "Time: " + ts.TotalSeconds.ToString();
			//Check if food point total is less than or equal to zero.
			if (health <= 0) {
                if (SoundManager.instance != null)
                {
                    //Call the PlaySingle function of SoundManager and pass it the gameOverSound as the audio clip to play.
                    SoundManager.instance.PlaySingle(gameOverSound);

                    //Stop the background music.
                    SoundManager.instance.musicSource.Stop();
                }

				//Call the GameOver function of GameManager.
				gameManagerService.GameOver ();
			}
		}
	}
}