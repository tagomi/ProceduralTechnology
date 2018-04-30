// ***********************************************************************
// Assembly         : Assembly-CSharp
// Author           : ただのごみ
// Created          : 04-30-2018
//
// Last Modified By : ただのごみ
// Last Modified On : 04-30-2018
// ***********************************************************************
// <copyright file="BlockAttribute.cs" company="">
//     Copyright (c) ただのごみ. Please read LICENSE file. If it is nothing, all rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace TGM.Procedural.Map
{
	/// <summary>
	/// ブロックの属性
	/// </summary>
	[Serializable]
	public class BlockAttribute
	{
		/// <summary>
		/// ブロックの種類
		/// </summary>
		public readonly BlockTypes type;

		/// <summary>
		/// コンストラクタ <see cref="BlockAttribute" /> class.
		/// </summary>
		/// <param name="type">ブロックの種類</param>
		public BlockAttribute(BlockTypes type)
		{
			this.type = type;
		}
	}
}
