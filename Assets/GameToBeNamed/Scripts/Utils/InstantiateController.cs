using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameToBeNamed.Utils {

    public class InstantiateController : Singleton<InstantiateController> {
        
        public void InstantiateDirectionalEffect(GameObject obj, Vector3 position, float direction) {
            obj.transform.localScale = new Vector3(direction, 1, 1);
            var tempObj = Instantiate(obj, position, Quaternion.identity);
        }

        public void InstantiateEffect(GameObject obj, Vector3 position) {
            var tempObj = Instantiate(obj, position, Quaternion.identity);
        }
    }
}