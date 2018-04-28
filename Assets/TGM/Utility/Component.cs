// ***********************************************************************
// Assembly         : Assembly-CSharp
// Author           : ただのごみ
// Created          : 04-29-2018
//
// Last Modified By : ただのごみ
// Last Modified On : 04-29-2018
// ***********************************************************************
// <copyright file="Component.cs" company="">
//     Copyright (c) ただのごみ. Please read LICENSE file. If it is nothing, all rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using UnityEngine;

namespace TGM.Utility
{
	/// <summary>
	/// コンポーネントのユーティリティクラス
	/// </summary>
	public static class Component
	{
		/// <summary>
		/// コンポーネントが付いていればそれを取得する
		/// そうでなければ、付けてから返す
		/// </summary>
		/// <typeparam name="T">コンポーネントの型</typeparam>
		/// <param name="behavior">操作対象</param>
		/// <returns>コンポーネント</returns>
		public static T GetOrAddComponent<T>(this MonoBehaviour behavior) where T : UnityEngine.Component
		{
			return behavior.gameObject.GetOrAddComponent<T>();
		}

		/// <summary>
		/// コンポーネントが付いていればそれを取得する
		/// そうでなければ、付けてから返す
		/// </summary>
		/// <typeparam name="T">コンポーネントの型</typeparam>
		/// <param name="gameObject">操作対象</param>
		/// <returns>コンポーネント</returns>
		public static T GetOrAddComponent<T>(this GameObject gameObject) where T : UnityEngine.Component
		{
			var component = gameObject.GetComponent<T>();
			// コンポーネントが付いていなければ付ける
			if (component == null)
			{
				component = gameObject.AddComponent<T>();
			}

			return component;
		}
	}
}
