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

    [Test]
    public void Game_Object_CreatedWithGiven_WillHaveTheName()
    {
        var go = new GameObject("MyGameObject");
        Assert.AreEqual("MyGameObject", go.name);
    }

    [Test]
    public void TestLevelStartDelay()
    {
        GameManager gm = new GameManager();
        gm.levelStartDelay = 0f;
        Assert.AreEqual(0f, gm.levelStartDelay);
    }

    [Test]
    public void TestPlayerFoodPoints()
    {
        GameManager gm = new GameManager();
        gm.playerFoodPoints = 100;
        Assert.AreEqual(100, gm.playerFoodPoints);
    }

    /*[Test]
    public void TestEnemyStart()
    {
        GameManager gm = new GameManager();
        Enemy enemy = new Enemy();
        GameManager.instance.AddEnemyToList(enemy);
        Animator animator = new Animator();
        animator = animator.GetComponent<Animator>();
        Transform target;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        
    }*/

    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    //[UnityTest]
    //public IEnumerator TestsWithEnumeratorPasses() {
    // Use the Assert class to test conditions.
    // yield to skip a frame
    //	yield return null;
    //}
}
