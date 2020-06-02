using System;
using UnityEngine;

namespace GameToBeNamed.Character {
    public class SelectImplementation : PropertyAttribute {
        public Type FieldType;
        public bool Expanded;

        public SelectImplementation(Type fieldType) {
            FieldType = fieldType;
        }
    }
}