// ***********************************************************************
// Assembly         : Assembly-CSharp
// Author           : ただのごみ
// Created          : 04-30-2018
//
// Last Modified By : ただのごみ
// Last Modified On : 04-30-2018
// ***********************************************************************
// <copyright file="IMapGenerator.cs" company="">
//     Copyright (c) ただのごみ. Please read LICENSE file. If it is nothing, all rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using TGM.Lib.Vector;
using TGM.Procedural.Entity.Block;

namespace TGM.Procedural.Map
{
	/// <summary>
	/// マップ生成機のインターフェイス
	/// </summary>
	public interface IMapGenerator
	{
		/// <summary>
		/// 1チャック分のブロック属性を決める
		/// </summary>
		/// <param name="chunkPos">チャンク</param>
		/// <returns>1チャンク分のブロック属性</returns>
		BlockAttribute[,,] CreateChunkBlockAttributes(IntVector3 chunkPos);
	}
}
