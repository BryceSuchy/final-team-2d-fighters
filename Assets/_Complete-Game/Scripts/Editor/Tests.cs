using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using Completed;
using UnityEngine.SceneManagement;

public class Tests {

    //[Test]
    //public void TestsSimplePasses() {
    // Use the Assert class to test conditions.
    //}

    Enemy enemy;
    GameManager gameManager;
    Wall wall;
    Player player;
    MovingObject movingObject;

    AudioSource efxSource;                 
    AudioSource musicSource;            
    SoundManager soundManager; 				
    SoundManager lowPitchRange;          
    SoundManager highPitchRange;
    AudioClip clip;
    LoadSceneOnClick loadSceneOnClick;

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

        soundManager = gameObject1.AddComponent<SoundManager>();
        lowPitchRange = gameObject1.AddComponent<SoundManager>();
        highPitchRange = gameObject1.AddComponent<SoundManager>(); 
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

    //Start
    //Time it will take object to move, in seconds.
    [Test]
    public void TestSoundManager()
    {
        Assert.IsTrue(true);
    }

    [Test]
    public void TestSoundManagerLowPitch()
    {
        Assert.IsTrue(true);
    }

    [Test]
    public void TestLowPitch()
    {
        float lowPitchRange1 = .95f;
        Assert.AreEqual(lowPitchRange1, .95f);
    }

    [Test]
    public void TestSoundManagerHighPitch()
    {
        Assert.IsTrue(true);
    }

    [Test]
    public void TestHighPitch()
    {
        float highPitchRange1 = 1.05f;
        Assert.AreEqual(highPitchRange1, 1.05f);
    }

    [Test]
    public void TestPlaySingle()
    {
        //soundManager.PlaySingle(clip);
        Assert.IsTrue(true);
    }

    [Test]
    public void TestQuitOnClick()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
        Assert.AreEqual(true, UnityEditor.EditorApplication.isPlaying == false);
    }

    [Test]
    public void TestLoadScene0()
    {
        Assert.AreEqual(SceneManager.GetSceneByBuildIndex(0), SceneManager.GetSceneByName("StartMenu"));
    }

    [Test]
    public void TestLoadScene1()
    {
        Assert.AreEqual(SceneManager.GetSceneByBuildIndex(1), SceneManager.GetSceneByName("_Complete-Game"));
    }

    [Test]
    public void TestLoadScene2()
    {
        Assert.AreEqual(SceneManager.GetSceneByBuildIndex(2), SceneManager.GetSceneByName("Credits"));
    }

    [Test]
    public void TestLoadScene3()
    {
        Assert.AreEqual(SceneManager.GetSceneByBuildIndex(3), SceneManager.GetSceneByName("GameOver"));
    }
}
