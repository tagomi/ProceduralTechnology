// ***********************************************************************
// Assembly         : Assembly-CSharp
// Author           : ただのごみ
// Created          : 04-30-2018
//
// Last Modified By : ただのごみ
// Last Modified On : 04-30-2018
// ***********************************************************************
// <copyright file="BlockTypes.cs" company="">
//     Copyright (c) ただのごみ. Please read LICENSE file. If it is nothing, all rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace TGM.Procedural.Entity.Block
{
	/// <summary>
	/// ブロックの種類
	/// </summary>
	public enum BlockTypes : int
	{
		/// <summary>
		/// 空の空間
		/// </summary>
		Empty,
		/// <summary>
		/// 土
		/// </summary>
		Dirt,
		/// <summary>
		/// 石
		/// </summary>
		Stone,
		/// <summary>
		/// 草
		/// </summary>
		Grass,
	}
}
