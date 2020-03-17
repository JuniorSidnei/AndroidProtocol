using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameToBeNamed.Utils
{
    public class GameManager : Singleton<GameManager> {
        public QueuedEventDispatcher GlobalDispatcher = new QueuedEventDispatcher();
    }
}