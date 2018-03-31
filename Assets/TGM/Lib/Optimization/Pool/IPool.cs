// ***********************************************************************
// Assembly         : Assembly-CSharp
// Author           : ただのごみ
// Created          : 03-22-2018
//
// Last Modified By : ただのごみ
// Last Modified On : 03-31-2018
// ***********************************************************************
// <copyright file="IPool.cs" company="">
//     Copyright (c) ただのごみ. Released under the MIT license.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace TGM.Lib.Optimization.Pool
{
	/// <summary>
	/// オブジェクトプール用のインターフェイス
	/// </summary>
	/// <typeparam name="T">プールするオブジェクトの型</typeparam>
	public interface IPool<T> where T : class
	{
		#region Properties

		/// <summary>
		/// プール可能な最大数
		/// </summary>
		int Capacity
		{
			get;
		}

		/// <summary>
		/// プールされているオブジェクト数
		/// </summary>
		int Count
		{
			get;
		}

		/// <summary>
		/// 取得可能なオブジェクト数
		/// </summary>
		int AvailableCount
		{
			get;
		}

		#endregion Properties

		#region Methods

		/// <summary>
		/// オブジェクトプールを空にする
		/// </summary>
		void Clear();

		/// <summary>
		/// プール可能な最大数を設定する
		/// </summary>
		/// <param name="capacity">プール可能な最大数</param>
		/// <returns>変更後のキャパシティ</returns>
		int SetCapacity(int capacity);

		/// <summary>
		/// プールされているオブジェクトを取得する
		/// </summary>
		/// <param name="advancedSettlingAfterCollectingAction">追加の回収後処理</param>
		/// <returns>プールされているオブジェクト</returns>
		T Get(Action<T> advancedSettlingAfterCollectingAction);

		/// <summary>
		/// プールオブジェクトを取り除く
		/// </summary>
		/// <param name="object">取り除くプールオブジェクト</param>
		/// <returns>取り除かれたオブジェクト</returns>
		T RemoveObject(T @object);

		#endregion Methods
	}
}
