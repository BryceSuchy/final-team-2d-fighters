using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections.Generic;
using Completed;
using UnityEngine.UI;

public class Playertest {
    public class MockComponentProvider : IComponent
    {
        public T GetComponent<T>()
        {
            return default(T);
        }
    }

    public class MockGameManager : IGameManager
    {
        public bool gameOverCalled;

        public MockGameManager()
        {
            playerFoodPoints = 100;
            gameOverCalled = false;
            enemies = new List<Enemy>();
        }

        public override void GameOver()
        {
            gameOverCalled = true;
        }
    }

    Player player;
    MockGameManager gameManager;

    [SetUp]
    public void Initialize()
    {
        GameObject gm = new GameObject();
        GameObject gm2 = new GameObject();
        gameManager = gm2.AddComponent<MockGameManager>();
        gameManager.playerFoodPoints = 100;
        player = gm.AddComponent<Player>();
        player.setComponentProvider(new MockComponentProvider());
        player.setGameManagerService(gameManager);
        
    }

    //1
    [Test]
	public void PlayerCanBeCreated() {
        player.PublicStart();

        Assert.IsTrue(true);
	}

    //2
    [Test]
    public void PlayerMovesOnPressingW()
    {
        //can't see player movement without passing to the next frame, so just want to make sure the move method is called
        player.PublicStart();
        player.rigidBody.startSpying();
        InputWrapper.SetKey(KeyCode.W);
        player.Update();
        Assert.IsTrue(player.rigidBody.movePositionCalled);
    }

    //3
    [Test]
    public void PlayerTakesDamageFromLoseFood()
    {
        player.PublicStart();
        int health = player.health;
        player.LoseFood(10);
        Assert.IsTrue(health != player.health);
    }

    //4
    [Test]
    public void GameOverWhenPlayerRunsOutOfHealth()
    {
        player.PublicStart();
        player.LoseFood(100);
        Assert.IsTrue(gameManager.gameOverCalled);
    }

    //5
    [Test]
    public void PlayerAttacksOnHittingArrowKey()
    {
        player.PublicStart();
        player.attackTracker = false;
        InputWrapper.SetKey(KeyCode.UpArrow);
        player.rigidBody.startSpying();
        player.Update();
        Assert.IsTrue(player.attackTracker);
    }

    //6
    [Test]
    public void PlayerDoesNotAttackDuringDelay()
    {
        player.PublicStart();
        player.attackTracker = false;
        InputWrapper.SetKey(KeyCode.UpArrow);
        player.rigidBody.startSpying();
        player.Update();
        player.attackTracker = false;
        //do two attacks as fast as possible, the code should run fast enough that the second one is during the delay
        player.Update();
        Assert.IsFalse(player.attackTracker);
    }

    //7
    [Test]
    public void PlayerTakesDamageFromEnemyCollision()
    {
        player.PublicStart();
        int currentHealth = player.health;
        GameObject go = new GameObject();
        Enemy mockEnemy = go.AddComponent<Enemy>();
        mockEnemy.lastAttackTime = -5;
        BoxCollider2D collider = go.AddComponent<BoxCollider2D>();
        collider.tag = "Enemy";
        gameManager.enemies.Add(mockEnemy);
        player.OnTriggerEnter2D(collider);
        Assert.AreNotEqual(currentHealth, player.health);
    }

    //8
    [Test]
    public void FoodTextChangesAfterLosingHealth()
    {
        GameObject textOb = new GameObject();
        player.foodText = textOb.AddComponent<Text>();
        player.PublicStart();
        string currentText = player.foodText.text;
        player.LoseFood(10);
        Assert.AreNotEqual(currentText, player.foodText.text);
    }

    //9
    [Test]
    public void PlayerMovesCorrectDistanceDiagonally()
    {
        player.PublicStart();
        player.rigidBody.startSpying();
        InputWrapper.SetKey(KeyCode.W);
        InputWrapper.SetKey(KeyCode.A);
        Vector2 oldPos = player.transform.position;
        player.Update();
        Vector2 newPos = player.rigidBody.lastPosition;
        float distanceTraveled = Vector2.Distance(oldPos, newPos);
        Assert.AreEqual(distanceTraveled, player.speed);
    }

    //10
    [Test]
    public void EnemyInRangeIsKilled()
    {
        player.PublicStart();
        player.rigidBody.startSpying();
        GameObject go = new GameObject();
        Enemy mockEnemy = go.AddComponent<Enemy>();
        mockEnemy.transform.position = new Vector2(0, .5f);
        BoxCollider2D collider = go.AddComponent<BoxCollider2D>();
        collider.tag = "Enemy";
        collider.enabled = true;
        go.layer = player.blockingLayer;
        
        InputWrapper.SetKey(KeyCode.UpArrow);
        player.Update();
        
        Assert.IsFalse(go.activeSelf); //test that the enemy was "killed"
    }
}