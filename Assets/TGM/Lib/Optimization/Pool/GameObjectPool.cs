﻿// ***********************************************************************
// Assembly         : Assembly-CSharp
// Author           : ただのごみ
// Created          : 04-17-2018
//
// Last Modified By : ただのごみ
// Last Modified On : 04-17-2018
// ***********************************************************************
// <copyright file="GameObjectPool.cs" company="">
//     Copyright (c) ただのごみ. Please read LICENSE file. If it is nothing, all rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using UnityEngine;

namespace TGM.Lib.Optimization.Pool
{
	/// <summary>
	/// ゲームオブジェクトプール
	/// </summary>
	/// <seealso cref="TGM.Lib.Optimization.Pool.UnityObjectPool{UnityEngine.GameObject}" />
	public class GameObjectPool : UnityObjectPool<GameObject>
	{
		/// <summary>
		/// コンストラクタ <see cref="GameObjectPool" /> class.
		/// </summary>
		/// <param name="capacity">プール可能数</param>
		/// <param name="initCount">最初にプールするオブジェクト数</param>
		/// <param name="createDelegate">プールオブジェクトを作る処理</param>
		/// <param name="collectingPredicate">オブジェクトの回収条件</param>
		/// <param name="preparingToGetAction">プールされているオブジェクトを取得する前に行う処理</param>
		/// <param name="settlingAfterCollectingAction">オブジェクトの改修後に行う処理</param>
		/// <param name="settlingAfterRemovingAction">オブジェクトプールから取り除いた後に行う処理</param>
		public GameObjectPool(int capacity, int initCount, ObjectPool<GameObject>.CreateDelegate createDelegate, Predicate<GameObject> collectingPredicate, Action<GameObject> preparingToGetAction = null, Action<GameObject> settlingAfterCollectingAction = null, Action<GameObject> settlingAfterRemovingAction = null) : base(capacity, initCount, createDelegate, collectingPredicate, preparingToGetAction, settlingAfterCollectingAction, settlingAfterRemovingAction)
		{
		}

		/// <summary>
		/// コンストラクタ <see cref="GameObjectPool" /> class.
		/// </summary>
		/// <param name="capacity">プール可能数</param>
		/// <param name="initCount">最初にプールするオブジェクト数</param>
		/// <param name="original">The original.</param>
		/// <param name="collectingPredicate">オブジェクトの回収条件</param>
		/// <param name="preparingToGetAction">プールされているオブジェクトを取得する前に行う処理</param>
		/// <param name="settlingAfterCollectingAction">オブジェクトの改修後に行う処理</param>
		/// <param name="settlingAfterRemovingAction">オブジェクトプールから取り除いた後に行う処理</param>
		public GameObjectPool(int capacity, int initCount, GameObject original, Predicate<GameObject> collectingPredicate, Action<GameObject> preparingToGetAction = null, Action<GameObject> settlingAfterCollectingAction = null, Action<GameObject> settlingAfterRemovingAction = null) : base(capacity, initCount, GameObjectPool.CreateInstantiateDelegate(original), collectingPredicate, preparingToGetAction, settlingAfterCollectingAction, settlingAfterRemovingAction)
		{
		}

		/// <summary>
		/// GameObjectのコピーを実体化するデリゲートを返す
		/// </summary>
		/// <param name="original">プレハブなどのオリジナル</param>
		/// <returns>GameObjectのコピーを実体化するデリゲート</returns>
		protected static ObjectPool<GameObject>.CreateDelegate CreateInstantiateDelegate(GameObject original)
		{
			return () =>
			{
				return GameObject.Instantiate(original);
			};
		}
	}
}
