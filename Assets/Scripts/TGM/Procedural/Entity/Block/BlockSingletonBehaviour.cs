// ***********************************************************************
// Assembly         : Assembly-CSharp
// Author           : ただのごみ
// Created          : 04-30-2018
//
// Last Modified By : ただのごみ
// Last Modified On : 04-30-2018
// ***********************************************************************
// <copyright file="BlockSingletonBehaviour.cs" company="">
//     Copyright (c) ただのごみ. Please read LICENSE file. If it is nothing, all rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using TGM.Lib.DesignerPattern;
using UnityEngine;

namespace TGM.Procedural.Entity.Block
{
	/// <summary>
	/// ブロック管理のシングルトン
	/// </summary>
	/// <seealso cref="TGM.Lib.DesignerPattern.SingletonBehaviour{TGM.Procedural.Entity.Block.BlockSingletonBehaviour}" />
	public sealed class BlockSingletonBehaviour : SingletonBehaviour<BlockSingletonBehaviour>
	{
		/// <summary>
		/// ブロックの種類毎のマテリアル
		/// </summary>
		[SerializeField]
		private List<Material> materialList = new List<Material>(Enum.GetNames(typeof(BlockTypes)).Length);

		/// <summary>
		/// ブロックを取得する
		/// </summary>
		/// <param name="attribute">ブロックの属性</param>
		/// <returns>ブロック</returns>
		public BlockBehaviour GetBlock(BlockAttribute attribute)
		{
			/// @todo プール化
			var blockObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
			var block = blockObject.AddComponent<BlockBehaviour>();

			// 作り直す
			block.Renew(attribute);

			return block;
		}

		/// <summary>
		/// ブロックのマテリアルを取得する
		/// </summary>
		/// <param name="type">ブロックの種類</param>
		/// <returns>マテリアル</returns>
		public Material GetBlockMaterial(BlockTypes type) => this.materialList[(int)type];
	}
}
