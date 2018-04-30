// ***********************************************************************
// Assembly         : Assembly-CSharp
// Author           : ただのごみ
// Created          : 04-30-2018
//
// Last Modified By : ただのごみ
// Last Modified On : 04-30-2018
// ***********************************************************************
// <copyright file="BlockStatus.cs" company="">
//     Copyright (c) ただのごみ. Please read LICENSE file. If it is nothing, all rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;

namespace TGM.Procedural.Entity.Block
{
	/// <summary>
	/// ブロックの状態
	/// </summary>
	public class BlockStatus
	{
		/// <summary>
		/// このブロックを囲んでいるブロックの数
		/// </summary>
		public int SurroundedCount
		{
			get;
			protected set;
		}

		/// <summary>
		/// 何個のブロックに囲まれているか
		/// </summary>
		/// <param name="surroundedCount">このブロックを囲んでいるブロックの数</param>
		public void Initialize(int surroundedCount)
		{
			this.SurroundedCount = surroundedCount;
		}

		/// <summary>
		/// 周囲のブロックが増加した時に呼ばれる
		/// </summary>
		/// <param name="num">増加した数</param>
		public void OnIncreaseSurroundedBlock(int num = 1)
		{
			this.SurroundedCount += num;
		}

		/// <summary>
		/// 周囲のブロックが現状した時に呼ばれる
		/// </summary>
		/// <param name="num">減少した数</param>
		public void OnDecreaseSurroundedBlock(int num = 1)
		{
			this.SurroundedCount -= num;
		}
	}
}
