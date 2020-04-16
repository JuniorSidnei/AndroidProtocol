using System;
using UnityEngine;

namespace GameToBeNamed.Character {
    public class SelectImplementation : PropertyAttribute {
        public Type FieldType;

        public SelectImplementation(Type fieldType) {
            FieldType = fieldType;
        }
    }
}