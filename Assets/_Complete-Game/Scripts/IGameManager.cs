using System;
using System.Collections.Generic;
using UnityEngine;

namespace Completed
{
    public abstract class IGameManager : MonoBehaviour
    {
        public int playerFoodPoints;
        public List<Enemy> enemies;

        public abstract void GameOver();
    }
}