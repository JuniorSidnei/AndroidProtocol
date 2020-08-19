using System;
using UnityEngine;

namespace GameToBeNamed.Character {
    
    [Flags]
    public enum InputAction {
        Button1 = 1 << 1, //Jump
        Button2 = 1 << 2, //Right
        Button3 = 1 << 3, //Left
        Button4 = 1 << 4, //Atack
        Button5 = 1 << 5, //Block
        Button6 = 1 << 6, //Dash
        Button7 = 1 << 7, //change class
        Button8 = 1 << 8, //talk with npc
        Button9 = 1 << 9, //cancel talk with npc
        Button10 = 1 << 10, //Look up
        Button11 = 1 << 11, //Look down
        Button12 = 1 << 12, //Sprint
    }

    public interface IInputSource {
        void Configure(Character2D character);
        void Update();

        bool HasAction(InputAction action);
        bool HasActionDown(InputAction action);
        bool HasActionUp(InputAction action);
    }

    public abstract class InputSource : IInputSource {
        
        protected InputAction Action { private get; set; }
        protected InputAction ActionDown { private get; set; }
        protected InputAction ActionUp { private get; set; }

        public abstract void Configure(Character2D character);
        
        public abstract void Update();

        public bool HasAction(InputAction action) {
            return (Action & action) != 0;
        }

        public bool HasActionDown(InputAction action) {
            return (ActionDown & action) != 0;
        }

        public bool HasActionUp(InputAction action) {
            return (ActionUp & action) != 0;
        }

        public void SetAction(InputAction action) {
            Action |= action;
        }

        public void UnsetAction(InputAction action) {
            Action &= ~action;
        }

        public void SetActionDown(InputAction action) {
            ActionDown |= action;
        }

        public void UnsetActionDown(InputAction action) {
            ActionDown &= ~action;
        }

        public void SetActionUp(InputAction action) {
            ActionUp |= action;
        }

        public void UnsetActionUp(InputAction action) {
            ActionUp &= ~action;
        }
    }
}