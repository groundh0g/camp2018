using System;
using System.Collections.Generic;

namespace MoreOnCode.Lib.Util
{
	public static class CollectionUtil
	{
		public static void SafeAdd<TEnum>(List<TEnum> list, TEnum item) {
			if (!list.Contains (item)) {
				list.Add (item);
			}
		}

		public static void SafeAdd<TEnum, TObj>(Dictionary<TEnum, TObj> dictionary, TEnum key, TObj item) {
			if (!dictionary.ContainsKey (key)) {
				dictionary.Add (key, item);
			}
		}

		public static void SafeIncrement<TEnum>(Dictionary<TEnum, int> list, TEnum item) {
			if (!list.ContainsKey (item)) {
				list.Add (item, 1);
			} else {
				list [item] = list [item] + 1;
			}
		}

		public static void SafeMax<TEnum>(Dictionary<TEnum, int> list, TEnum item, int value) {
			if (!list.ContainsKey (item)) {
				list.Add (item, value);
			} else if(value > list [item]) {
				list [item] = value;
			}
		}

	}
}

