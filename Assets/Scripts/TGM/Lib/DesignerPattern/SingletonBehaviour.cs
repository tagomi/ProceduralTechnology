// ***********************************************************************
// Assembly         : Assembly-CSharp
// Author           : ただのごみ
// Created          : 04-30-2018
//
// Last Modified By : ただのごみ
// Last Modified On : 04-30-2018
// ***********************************************************************
// <copyright file="SingletonBehaviour.cs" company="">
//     Copyright (c) ただのごみ. Please read LICENSE file. If it is nothing, all rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using UnityEngine;

namespace TGM.Lib.DesignerPattern
{
	/// <summary>
	/// シングルトンなBehaviour
	/// </summary>
	/// <typeparam name="T">型</typeparam>
	/// <seealso cref="UnityEngine.MonoBehaviour" />
	public class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
	{
		/// <summary>
		/// 既存のインスタンス
		/// </summary>
		public static T Instance
		{
			get;
			private set;
		}

		/// <summary>
		/// インスタンスが存在するか
		/// </summary>
		/// <returns><c>true</c>ならインスタンスが存在する</returns>
		public static bool Exists()
		{
			return SingletonBehaviour<T>.Instance != null;
		}

		/// <summary>
		/// インスタンスを作る
		/// </summary>
		/// <returns>作られたインスタンス</returns>
		public static T Create()
		{
			if (SingletonBehaviour<T>.Exists())
			{
				Debug.LogWarning("既にインスタンスは存在します");
				return SingletonBehaviour<T>.Instance;
			}

			// GameObjectを作ってアタッチ
			var gameObject = new GameObject(typeof(T).FullName);
			return gameObject.AddComponent<T>();
		}

		/// <summary>
		/// インスタンスがなければ作り、インスタンスを取得する
		/// </summary>
		/// <returns>インスタンス</returns>
		public static T GetOrCreate()
		{
			if (SingletonBehaviour<T>.Exists())
			{
				return SingletonBehaviour<T>.Instance;
			}

			return SingletonBehaviour<T>.Create();
		}

		protected virtual void Awake()
		{
			if (SingletonBehaviour<T>.Exists())
			{
				GameObject.Destroy(this.gameObject);
				return;
			}

			// 制約条件からキャストしても問題ない
			SingletonBehaviour<T>.Instance = this as T;
		}
	}
}
