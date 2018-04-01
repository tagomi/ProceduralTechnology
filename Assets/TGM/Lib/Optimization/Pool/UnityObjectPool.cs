// ***********************************************************************
// Assembly         : Assembly-CSharp
// Author           : ただのごみ
// Created          : 04-01-2018
//
// Last Modified By : ただのごみ
// Last Modified On : 04-01-2018
// ***********************************************************************
// <copyright file="UnityObjectPool.cs" company="">
//     Copyright (c) ただのごみ. Please read LICENSE file. If it is nothing, all rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Linq;
using UniRx;
using UnityEngine;

namespace TGM.Lib.Optimization.Pool
{
	/// <summary>
	/// Unityオブジェクトプール
	/// </summary>
	/// <typeparam name="T">管理したいUnityオブジェクト型</typeparam>
	/// <seealso cref="TGM.Lib.Optimization.Pool.IPool{T}" />
	public class UnityObjectPool<T> : IPool<T> where T : UnityEngine.Object
	{
		/// <summary>
		/// オブジェクトプール本体
		/// </summary>
		/// <seealso cref="TGM.Lib.Optimization.Pool.ObjectPool{System.Reference{T}}" />
		protected readonly ObjectPool<Object.Reference<T>> objectPool;

		/// <summary>
		/// 取得可能なオブジェクト数
		/// </summary>
		public int AvailableCount
		{
			get
			{
				return this.objectPool.AvailableCount;
			}
		}

		/// <summary>
		/// プール可能な最大数
		/// </summary>
		public int Capacity
		{
			get
			{
				return this.objectPool.Capacity;
			}
		}

		/// <summary>
		/// プールされているオブジェクト数
		/// </summary>
		public int Count
		{
			get
			{
				return this.objectPool.Count;
			}
		}

		/// <summary>
		/// コンストラクタ <see cref="UnityObjectPool{T}" /> class.
		/// </summary>
		/// <param name="capacity">プール可能数</param>
		/// <param name="initCount">最初にプールするオブジェクト数</param>
		/// <param name="createDelegate">プールオブジェクトを作る処理</param>
		/// <param name="collectingPredicate">オブジェクトの回収条件</param>
		/// <param name="preparingToGetAction">プールされているオブジェクトを取得する前に行う処理</param>
		/// <param name="settlingAfterCollectingAction">オブジェクトの改修後に行う処理</param>
		/// <param name="settlingAfterRemovingAction">オブジェクトプールから取り除いた後に行う処理</param>
		public UnityObjectPool(int capacity, int initCount, ObjectPool<T>.CreateDelegate createDelegate, Predicate<T> collectingPredicate, Action<T> preparingToGetAction = null, Action<T> settlingAfterCollectingAction = null, Action<T> settlingAfterRemovingAction = null)
		{
			this.objectPool = new ObjectPool<Object.Reference<T>>(capacity, initCount,
				this.ProcessCreateDalegate(createDelegate),
				UnityObjectPool<T>.ProcessPredicate(collectingPredicate, true),
				UnityObjectPool<T>.ProcessAction(preparingToGetAction),
				UnityObjectPool<T>.ProcessAction(settlingAfterCollectingAction),
				UnityObjectPool<T>.ProcessAction(settlingAfterRemovingAction));
		}

		/// <summary>
		/// Objectを引数に取るActionをObjectへの参照を引数に取るActionに加工する
		/// </summary>
		/// <param name="action">Objectを引数に取るAction</param>
		/// <returns>Objectへの参照を引数に取るAction</returns>
		protected static Action<Object.Reference<T>> ProcessAction(Action<T> action)
		{
			return reference =>
			{
				T obj = reference.target;
				// 参照先のObjectがDestroy()されていないか
				if (obj != null)
				{
					action?.Invoke(obj);
				}
			};
		}

		/// <summary>
		/// Objectを引数に取るPredicateをObjectへの参照を引数に取るPredicateに加工する
		/// </summary>
		/// <param name="predicate">Objectを引数に取るPredicate</param>
		/// <param name="returnTrueOnBrokenRef"><c>true</c>参照が切れた場合は<c>true</c>を返す</param>
		/// <returns>Objectへの参照を引数に取るPredicate</returns>
		protected static Predicate<Object.Reference<T>> ProcessPredicate(Predicate<T> predicate, bool returnTrueOnBrokenRef)
		{
			return reference =>
			{
				T obj = reference.target;
				// 参照先のObjectがDestroy()されていないか
				if (obj != null)
				{
					return predicate?.Invoke(obj) ?? returnTrueOnBrokenRef;
				}

				// 参照切れ
				return returnTrueOnBrokenRef;
			};
		}

		/// <summary>
		/// オブジェクトプールを空にする
		/// </summary>
		public void Clear()
		{
			this.objectPool.Clear();
		}

		/// <summary>
		/// プールされているオブジェクトを取得する
		/// </summary>
		/// <param name="advancedSettlingAfterCollectingAction">追加の回収後処理</param>
		/// <returns>プールされているオブジェクト</returns>
		public T Get(Action<T> advancedSettlingAfterCollectingAction)
		{
			T obj = null;
			while (obj == null)
			{
				var gottenRef = this.objectPool.Get(UnityObjectPool<T>.ProcessAction(advancedSettlingAfterCollectingAction));
				obj = gottenRef.target;
				if (obj == null)
				{
					this.RemoveObject(gottenRef);
				}
			}

			return obj;
		}

		/// <summary>
		/// プール可能な最大数を設定する
		/// </summary>
		/// <param name="capacity">プール可能な最大数</param>
		/// <returns>変更後のキャパシティ</returns>
		public int SetCapacity(int capacity)
		{
			return this.objectPool.SetCapacity(capacity);
		}

		/// <summary>
		/// Objectを作る処理をObjectへの参照を作る処理に加工する
		/// </summary>
		/// <param name="createDelegate">プールオブジェクトを作る処理(Unity.Object)</param>
		/// <returns>プールオブジェクトを作る処理(Unity.Objectへの参照)</returns>
		protected virtual ObjectPool<Object.Reference<T>>.CreateDelegate ProcessCreateDalegate(ObjectPool<T>.CreateDelegate createDelegate)
		{
			return () =>
			{
				var createdObject = createDelegate();
				var reference = new Object.Reference<T>(createdObject);

				// 監視中のUnity.ObjectがDestroy()された場合には監視から取り除く
				reference
					.ObserveEveryValueChanged(target => target.target == null)
					.Where(brokenReference => brokenReference)
					.Subscribe(brokenReference =>
					{
						this.RemoveObject(reference);
					},
					e =>
					{
						Debug.LogError(e);
						this.RemoveObject(reference);
					},
					() =>
					{
						this.RemoveObject(reference);
					});

				return reference;
			};
		}

		/// <summary>
		/// プールオブジェクトを取り除く
		/// </summary>
		/// <param name="reference">取り除くプールオブジェクトへの参照</param>
		/// <remarks>取り除かれたオブジェクトは破棄されます</remarks>
		protected virtual void RemoveObject(Object.Reference<T> reference)
		{
			var removedRef = this.objectPool.RemoveObject(reference);

			if (removedRef.target != null)
			{
				UnityEngine.Object.Destroy(removedRef.target);
			}
		}

		/// <summary>
		/// プールオブジェクトを取り除く
		/// </summary>
		/// <param name="object">取り除くプールオブジェクト</param>
		/// <returns>取り除かれたオブジェクト</returns>
		/// <remarks>取り除かれたオブジェクトは破棄されません</remarks>
		public virtual T RemoveObject(T @object)
		{
			return this.objectPool.RemoveObject(new Object.Reference<T>(@object))?.target;
		}

		/// <summary>
		/// プールオブジェクトを取り除き、破棄する
		/// </summary>
		/// <param name="object">取り除き、破棄するプールオブジェクト</param>
		public virtual void RemoveAndDestroyObject(T @object)
		{
			T removedObject = this.RemoveObject(@object);
			if (removedObject != null)
			{
				UnityEngine.Object.Destroy(removedObject);
			}
		}
	}

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
	}
}
