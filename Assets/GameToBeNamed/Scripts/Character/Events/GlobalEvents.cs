using System;
using System.Collections;
using System.Collections.Generic;
using GameToBeNamed.Character;
using GameToBeNamed.Character.NPC;
using UnityEngine;

namespace GameToBeNamed.Utils {

    
    public class OnGameStart { }
    
    public class OnCameraScreenshake {

        public OnCameraScreenshake(int force, float duration) {
            Shakeforce = force;
            ShakeDuration = duration;
        }

        public int Shakeforce;
        public float ShakeDuration;
    }

    public class OnCameraConfigureOffset {

        public OnCameraConfigureOffset(int offsetOrientation, float offsetUpValue, float offsetDownValue, float originalOffsetValue) {
            OffsetOrientation = offsetOrientation;
            OffsetUpValue = offsetUpValue;
            OffsetDownValue = offsetDownValue;
            OriginalOffsetValue = originalOffsetValue;
        }
        public int OffsetOrientation;
        public float OffsetUpValue;
        public float OffsetDownValue;
        public float OriginalOffsetValue;
    }
    
    //Change char class
    public class OnCharacterChangeClass {
        public OnCharacterChangeClass(Character2D currentCharacter, Vector2 velocity) {
            CurrentCharacter = currentCharacter;
            Velocity = velocity;
        }

        public Character2D CurrentCharacter;
        public Vector2 Velocity;
    }

    public class OnCharacterUpdateClassCooldown {
        public OnCharacterUpdateClassCooldown(float classCooldown) {
            ClassCooldown = classCooldown;
        }

        public float ClassCooldown;
    }
    
    //Do transition
    public class OnCharacterTransition {
        public OnCharacterTransition(Action onTransitionCallBack) {
            OnTransitionCallBack = onTransitionCallBack;
           
        }

        public Action OnTransitionCallBack;
       
    }
    
    //When a char dies
    public class OnCharacterDeath {
        public OnCharacterDeath(Character2D character, Action onDeathCallback = null) {
            Character = character;
            OnDeathCallback = onDeathCallback;
        }

        public Character2D Character;
        public Action OnDeathCallback;
    }

    //To show damage
    public class OnCharacterDamage {
        
        public OnCharacterDamage(int damage, Vector3 position, int currentHealth, int maxHealth, bool isPlayer = true) {

            Damage = damage;
            Position = position;
            CurrentHealth = currentHealth;
            MaxHealth = maxHealth;
            IsPlayer = isPlayer;
        }

        public int Damage;
        public Vector3 Position;
        public int CurrentHealth;
        public int MaxHealth;
        public bool IsPlayer;
    }

    
    public class OnCharacterConfigureConstitution {
        public OnCharacterConfigureConstitution(int maxHealth, int currentHealth, Sprite iconSplash, Sprite lifeSplash) {
            
            MaxHealth = maxHealth;
            CurrentHealth = currentHealth;
            IconSplash = iconSplash;
            LifeSplash = lifeSplash;
        }

        public int MaxHealth { get; set; }
        public int CurrentHealth { get; set; }
        
        public Sprite IconSplash;
        public Sprite LifeSplash;
    }

    //Every attack should use this
    public class OnAttackTriggerEnter {
        
        public OnAttackTriggerEnter(Info info, int damage, Vector2 contact) {
            AttackInfo = info;
            Damage = damage;
            DamageContact = contact;
        }

        public Info AttackInfo;
        public int Damage;
        public Vector2 DamageContact;

        public struct Info {
            
            public GameObject Receiver;
            public GameObject Emiter;
        }
    }
    
    //load scene
    public class OnValidateScene {

        public OnValidateScene(SceneField sceneToLoad, SceneField sceneToUnload, AudioClip areaBgMusic) {
            SceneToLoad = sceneToLoad;
            SceneToUnload = sceneToUnload;
            AreaBgMusic = areaBgMusic;
        }

        public SceneField SceneToLoad;
        public SceneField SceneToUnload;
        public AudioClip AreaBgMusic;
    }
    
    //on collect
    public class OnCollectableJunkPieces {
        public OnCollectableJunkPieces(int junkAmount) {
            JunkAmount = junkAmount;
        }

        public int JunkAmount;
    }

    public class OnUpdateCollectable {
        
        public OnUpdateCollectable(int curretnAmount) {
            CurrentAmount = curretnAmount;
        }

        public int CurrentAmount;
    }

    public class OnTalking {

        public OnTalking(NpcBehavior npc, bool onTalkingNpc) {
            Npc = npc;
            OnTalkingNpc = onTalkingNpc;
        }
        public NpcBehavior Npc;
        public bool OnTalkingNpc;
    }
}