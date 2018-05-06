using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using Completed;

public class Tests {

    //[Test]
    //public void TestsSimplePasses() {
    // Use the Assert class to test conditions.
    //}

    Enemy enemy;
    GameManager gameManager;
    Wall wall;
    Player player;

    [SetUp]
    public void Initialize()
    {
        GameObject gameObject1 = new GameObject();
        GameObject gameObject2 = new GameObject();
        GameObject gameObject3 = new GameObject();
        GameObject gameObject4 = new GameObject();
        gameManager = gameObject2.AddComponent<GameManager>();
        gameManager.Awake();
        enemy = gameObject1.AddComponent<Enemy>();
        wall = gameObject3.AddComponent<Wall>();
        player = gameObject4.AddComponent<Player>();
        gameObject4.tag = "Player";
    }

    [Test]
    public void Game_Object_CreatedWithGiven_WillHaveTheName()
    {
        var go = new GameObject("MyGameObject");
        Assert.AreEqual("MyGameObject", go.name);
    }

    [Test]
    public void TestLevelStartDelay()
    {
        Assert.AreEqual(0f, gameManager.levelStartDelay);
    }

    [Test]
    public void TestPlayerFoodPoints()
    {
        Assert.AreEqual(100, gameManager.playerFoodPoints);
    }

    [Test]
    public void TestEnemyCreate ()
    {
        Assert.IsTrue(true);
    }

    [Test]
    public void TestWallCreate()
    {
        Assert.IsTrue(true);
    }

    [Test]
    public void TestAddEnemyToList()
    {
        gameManager.AddEnemyToList(enemy);
        Assert.Contains(enemy, gameManager.enemies);
    }

    [Test]
    public void TestEnemyIsReadyToAttack()
    {
        enemy.Start1();
        Assert.True(enemy.isReadyToAttack());
    }

    [Test]
    public void TestRestartLevelDelay()
    {
        Assert.AreEqual(0f, player.restartLevelDelay);
    }

    [Test]
    public void TestPointsPerFood()
    {
        Assert.AreEqual(10, player.pointsPerFood);
    }

    [Test]
    public void TestPointsPerSoda()
    {
        Assert.AreEqual(20, player.pointsPerSoda);
    }
    

    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    //[UnityTest]
    //public IEnumerator TestsWithEnumeratorPasses() {
    // Use the Assert class to test conditions.
    // yield to skip a frame
    //	yield return null;
    //}
}
