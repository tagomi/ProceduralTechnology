﻿// ***********************************************************************
// Assembly         : Assembly-CSharp
// Author           : ただのごみ
// Created          : 04-01-2018
//
// Last Modified By : ただのごみ
// Last Modified On : 04-18-2018
// ***********************************************************************
// <copyright file="ObjectPool.cs" company="">
//     Copyright (c) ただのごみ. Please read LICENSE file. If it is nothing, all rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace TGM.Lib.Optimization.Pool
{
	/// <summary>
	/// オブジェクトをプールし、回収し、再利用できるようにする仕組み
	/// </summary>
	/// <typeparam name="T">プールするオブジェクトの型</typeparam>
	/// <seealso cref="TGM.Lib.Optimization.Pool.IPool{T}" />
	public class ObjectPool<T> : IPool<T> where T : class
	{
		#region Delegates

		/// <summary>
		/// プールオブジェクトを作るためのデリゲート
		/// </summary>
		/// <returns>作成したオブジェクト</returns>
		public delegate T CreateDelegate();

		#endregion Delegates

		#region Variables

		/// <summary>
		/// プールされているオブジェクトをキー、利用可能かを値に持つ辞書
		/// </summary>
		protected readonly Dictionary<T, bool> pooledObjectDictionary;

		/// <summary>
		/// プールオブジェクトを作る処理
		/// </summary>
		protected readonly CreateDelegate createDelegate;

		/// <summary>
		/// プールされているオブジェクトを取得する前に行う処理
		/// </summary>
		protected readonly Action<T> preparingToGetAction;

		/// <summary>
		/// オブジェクトの回収条件
		/// </summary>
		protected readonly Predicate<T> collectingPredicate;

		/// <summary>
		/// オブジェクトの回収後に行う処理
		/// </summary>
		protected readonly Action<T> settlingAfterCollectingAction;

		/// <summary>
		/// オブジェクトプールから取り除いた後に行う処理
		/// </summary>
		protected readonly Action<T> settlingAfterRemovingAction;

		#endregion Variables

		#region Properties

		/// <summary>
		/// プール可能な最大数
		/// </summary>
		public virtual int Capacity
		{
			get;
			protected set;
		} = 0;

		/// <summary>
		/// プールされているオブジェクト数
		/// </summary>
		public virtual int Count => this.pooledObjectDictionary.Count;

		/// <summary>
		/// 取得可能なオブジェクト数
		/// </summary>
		public int AvailableCount => this.pooledObjectDictionary.Values.Count(flag => flag);

		/// <summary>
		/// プールオブジェクトを取得可能か
		/// </summary>
		public bool CanGet => (this.AvailableCount > 0) || (this.Capacity > this.Count);

		#endregion Properties

		#region Methods

		/// <summary>
		/// コンストラクタ <see cref="ObjectPool{T}" /> class.
		/// </summary>
		/// <param name="capacity">プール可能数</param>
		/// <param name="initCount">最初にプールするオブジェクト数</param>
		/// <param name="createDelegate">プールオブジェクトを作る処理</param>
		/// <param name="collectingPredicate">オブジェクトの回収条件</param>
		/// <param name="preparingToGetAction">プールされているオブジェクトを取得する前に行う処理</param>
		/// <param name="settlingAfterCollectingAction">オブジェクトの改修後に行う処理</param>
		/// <param name="settlingAfterRemovingAction">オブジェクトプールから取り除いた後に行う処理</param>
		public ObjectPool(int capacity, int initCount, CreateDelegate createDelegate, Predicate<T> collectingPredicate, Action<T> preparingToGetAction = null, Action<T> settlingAfterCollectingAction = null, Action<T> settlingAfterRemovingAction = null)
		{
			this.pooledObjectDictionary = new Dictionary<T, bool>(capacity);

			this.createDelegate = createDelegate;
			this.preparingToGetAction = preparingToGetAction;
			this.collectingPredicate = collectingPredicate;
			this.settlingAfterCollectingAction = settlingAfterCollectingAction;
			this.settlingAfterRemovingAction = settlingAfterRemovingAction;

			this.SetCapacity(capacity);
			// プールオブジェクトを用意
			this.CreateObjects(initCount);

			Assert.IsNotNull(this.createDelegate);
			Assert.IsNotNull(this.collectingPredicate);
		}

		/// <summary>
		/// オブジェクトプールを空にする
		/// </summary>
		public virtual void Clear()
		{
			var removedObjects = this.pooledObjectDictionary.Keys.ToArray();
			this.pooledObjectDictionary.Clear();
			// 全てのオブジェクトに取り除いた後の後始末処理を行う
			foreach (var removedObject in removedObjects)
			{
				this.settlingAfterRemovingAction?.Invoke(removedObject);
			}
		}

		/// <summary>
		/// オブジェクトプールの破棄処理
		/// </summary>
		public virtual void Destroy()
		{
			this.Clear();
		}

		/// <summary>
		/// プール可能な最大数を設定する
		/// </summary>
		/// <param name="capacity">プール可能な最大数</param>
		/// <returns>変更後のキャパシティ</returns>
		/// <remarks>使用中のオブジェクトが多い場合には、キャパシティを減らせない場合があります</remarks>
		public virtual int SetCapacity(int capacity)
		{
			// プールされているオブジェクトの数よりも大きな値であれば何もしない
			if (capacity > this.Count)
			{
				this.Capacity = capacity;
				return this.Capacity;
			}

			int over = this.Count - capacity;
			// 使われていないオブジェクトを取得する
			var unusedObjects =
				this.pooledObjectDictionary
					.Where(pair => pair.Value)
					.Select(pair => pair.Key);

			// キャパシティを超えなくなるまで、使われていないオブジェクトを取り除く
			foreach (var unusedObject in unusedObjects)
			{
				this.RemoveObject(unusedObject);
				over--;

				// キャパシティを超えなくなったら終了
				if (over <= 0)
				{
					break;
				}
			}

			// 減らしきれなくても、現在のプールオブジェクト数をキャパシティにする
			this.Capacity = this.Count;
			return this.Capacity;
		}

		/// <summary>
		/// プールオブジェクトを作る、管理に登録する
		/// </summary>
		/// <returns>作ったオブジェクト</returns>
		protected virtual T CreateObject()
		{
			var createdObject = this.createDelegate();
			this.AddObject(createdObject, true);

			return createdObject;
		}

		/// <summary>
		/// プールオブジェクトを作り、管理に登録する
		/// </summary>
		/// <param name="count">作る数</param>
		protected virtual void CreateObjects(int count)
		{
			int remainingCapacity = this.Capacity - this.Count;
			int createCount = Mathf.Min(count, remainingCapacity);

			for (int i = 0; i < count; i++)
			{
				this.CreateObject();
			}
		}

		/// <summary>
		/// プールされているオブジェクトを取得する
		/// </summary>
		/// <param name="advancedSettlingAfterCollectingAction">追加の回収後処理</param>
		/// <returns>プールされているオブジェクト</returns>
		public virtual T Get(Action<T> advancedSettlingAfterCollectingAction = null)
		{
			// 利用可能なオブジェクトを探す
			var usableObjects = this.pooledObjectDictionary
				.Where(pair => pair.Value);

			// 利用可能なオブジェクトが1つでもあったなら、それを使う
			if (usableObjects.Any(_ => true))
			{
				T usableObject = usableObjects
					.First()
					.Key;
				return this.PrepareObject(usableObject, advancedSettlingAfterCollectingAction);
			}

			if (this.Count >= this.Capacity)
			{
				Debug.LogError("利用可能なオブジェクトがなく、キャパシティも足りません");
				return null;
			}

			// 1つオブジェクトを作ってプールに追加
			var createdObject = this.CreateObject();

			return this.PrepareObject(createdObject, advancedSettlingAfterCollectingAction);
		}

		/// <summary>
		/// 取得前にオブジェクトの準備を行う
		/// </summary>
		/// <param name="targetObject">準備を行うオブジェクト</param>
		/// <param name="advancedSettlingAfterCollectingAction">追加の回収後処理</param>
		/// <returns>準備したオブジェクト</returns>
		protected virtual T PrepareObject(T targetObject, Action<T> advancedSettlingAfterCollectingAction)
		{
			if (targetObject == null)
			{
				Debug.LogError("nullなオブジェクトに対して準備を行うことはできません");
				return null;
			}

			// 利用不能にする
			this.pooledObjectDictionary[targetObject] = false;

			// 準備処理
			this.preparingToGetAction?.Invoke(targetObject);

			// 回収用の監視処理を設定
			targetObject
				.ObserveEveryValueChanged(_ => this.collectingPredicate(targetObject))
				.Where(result => result)
				.First()
				.Subscribe(_ =>
				{
					// 利用可能に戻す
					this.pooledObjectDictionary[targetObject] = true;

					// 回収後処理
					this.settlingAfterCollectingAction?.Invoke(targetObject);
					advancedSettlingAfterCollectingAction?.Invoke(targetObject);
				});

			return targetObject;
		}

		/// <summary>
		/// プールオブジェクトを追加する
		/// </summary>
		/// <param name="object">プールするオブジェクト</param>
		/// <param name="canUse">使用可能か</param>
		protected void AddObject(T @object, bool canUse)
		{
			this.pooledObjectDictionary.Add(@object, canUse);
		}

		/// <summary>
		/// プールオブジェクトを取り除く
		/// </summary>
		/// <param name="object">取り除くプールオブジェクト</param>
		/// <returns>取り除かれたオブジェクト</returns>
		public virtual T RemoveObject(T @object)
		{
			this.pooledObjectDictionary.Remove(@object);

			// 取り除かれた後の処理
			this.settlingAfterRemovingAction?.Invoke(@object);

			return @object;
		}

		#endregion Methods
	}
}