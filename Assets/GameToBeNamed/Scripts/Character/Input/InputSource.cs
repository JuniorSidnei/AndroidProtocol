using System;
using UnityEngine;

namespace GameToBeNamed.Character {

    //fazer receber o character2D
    [Flags]
    public enum InputAction {
        Button1 = 1 << 1, //Jump
        Button2 = 1 << 2, //Right
        Button3 = 1 << 3, //Left
        Button4 = 1 << 4, //Atack
        Button5 = 1 << 5, //Block
        Button6 = 1 << 6, //Dash
        Button7 = 1 << 7, //change class
        Button8 = 1 << 8, //interacte with npc
        Button9 = 1 << 9,
    }

    public interface IInputSource {
        void Configure();
        void Update();

        bool HasAction(InputAction action);
        bool HasActionDown(InputAction action);
        bool HasActionUp(InputAction action);
    }

    public abstract class InputSource : IInputSource {

        public Character2D m_char;
        protected InputAction Action { private get; set; }
        protected InputAction ActionDown { private get; set; }
        protected InputAction ActionUp { private get; set; }

        public abstract void Configure();

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

        protected void SetAction(InputAction action) {
            Action |= action;
        }

        protected void UnsetAction(InputAction action) {
            Action &= ~action;
        }

        protected void SetActionDown(InputAction action) {
            ActionDown |= action;
        }

        protected void UnsetActionDown(InputAction action) {
            ActionDown &= ~action;
        }

        protected void SetActionUp(InputAction action) {
            ActionUp |= action;
        }

        protected void UnsetActionUp(InputAction action) {
            ActionUp &= ~action;
        }
    }
}