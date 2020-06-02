using System;
using UnityEngine;

namespace GameToBeNamed.Utils {

	public class MinMaxSliderAttribute : PropertyAttribute {
		public readonly float max;
		public readonly float min;

		public MinMaxSliderAttribute(float min_, float max_) {
			min = min_;
			max = max_;
		}
	}
}