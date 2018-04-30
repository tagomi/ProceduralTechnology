// ***********************************************************************
// Assembly         : Assembly-CSharp
// Author           : ただのごみ
// Created          : 04-30-2018
//
// Last Modified By : ただのごみ
// Last Modified On : 04-30-2018
// ***********************************************************************
// <copyright file="BlockBehaviour.cs" company="">
//     Copyright (c) ただのごみ. Please read LICENSE file. If it is nothing, all rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using UnityEngine;

namespace TGM.Procedural.Map
{
	/// <summary>
	/// ブロック
	/// </summary>
	/// <seealso cref="UnityEngine.MonoBehaviour" />
	public class BlockBehaviour : MonoBehaviour
	{
		/// <summary>
		/// ブロックの属性
		/// </summary>
		protected BlockAttribute attribute;

		/// <summary>
		/// ブロックの状態
		/// </summary>
		protected BlockStatus status = new BlockStatus();

		/// <summary>
		/// 作り直す
		/// </summary>
		/// <param name="attribute">属性</param>
		public void Renew(BlockAttribute attribute)
		{
			this.attribute = attribute;
		}
	}
}
