using UnityEngine;

namespace GameToBeNamed.Character {
    public static class ActionStates {
        
        public static PropertyName Dead = new PropertyName("Dead");
        public static PropertyName Blocking = new PropertyName("Blocking");
        public static PropertyName ReceivingDamage = new PropertyName("ReceivingDamage");
        public static PropertyName Jumping = new PropertyName("Jumping");
        public static PropertyName Talking = new PropertyName("Talking");
        public static PropertyName Dashing = new PropertyName("Dashing");
        public static PropertyName Attacking = new PropertyName("Attacking");
    }
}