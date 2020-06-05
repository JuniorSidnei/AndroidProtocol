using System.Collections;
using System.Collections.Generic;
using GameToBeNamed.Utils;
using UnityEngine;

namespace GameToBeNamed.Character {

    public abstract class CharacterState<T> : MonoBehaviour {

        public abstract void OnConfigure(T character);
        public abstract void Enter(T character);
        public abstract void Run(T character);
        public abstract void Exit(T character);

        public abstract void Update(T character);
        
    }
}