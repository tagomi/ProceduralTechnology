// ***********************************************************************
// Assembly         : Assembly-CSharp
// Author           : ただのごみ
// Created          : 04-30-2018
//
// Last Modified By : ただのごみ
// Last Modified On : 04-30-2018
// ***********************************************************************
// <copyright file="MapBehaviour.cs" company="">
//     Copyright (c) ただのごみ. Please read LICENSE file. If it is nothing, all rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;
using TGM.Lib.Vector;
using TGM.Procedural.Entity.Block;
using UnityEngine;

namespace TGM.Procedural.Map
{
	/// <summary>
	/// マップ
	/// </summary>
	/// <seealso cref="UnityEngine.MonoBehaviour" />
	public sealed class MapBehaviour : MonoBehaviour
	{
		/// <summary>
		/// チャンク座標がキー、チャンクが値の辞書
		/// </summary>
		private readonly Dictionary<IntVector3, Chunk> chunkDic = new Dictionary<IntVector3, Chunk>();

		/// <summary>
		/// マップ生成機
		/// </summary>
		private MapGenerator mapGenerator;

		/// <summary>
		/// チャンク生成
		/// </summary>
		/// <param name="chunkPos">チャンク座標</param>
		/// <returns>生成されたチャンク</returns>
		private Chunk CreateChunk(IntVector3 chunkPos)
		{
			// 1チャンク分の属性を決める
			var attributes = this.mapGenerator.CreateChunkBlockAttributes(chunkPos);
			// チャンク座標のワールド座標
			var chunkWorldPos = Chunk.ConvertChunkPosToWorldPos(chunkPos);
			// ブロック管理
			var blockSingleton = BlockSingletonBehaviour.GetOrCreate();
			// 1チャンク分のブロックを格納する用
			var blocks = Chunk.CreateChunkBlockArray<BlockBehaviour>();

			if ((attributes.GetLength(0) != blocks.GetLength(0)) || (attributes.GetLength(1) != blocks.GetLength(1)) || (attributes.GetLength(2) != blocks.GetLength(2)))
			{
				Debug.LogError("生成されたブロック属性とブロックそのもの配列のサイズが合致しません");
				return null;
			}

			// ブロックを作って配置する
			for (int z = 0, zEnd = attributes.GetLength(0); z < zEnd; z++)
			{
				for (int y = 0, yEnd = attributes.GetLength(1); y < yEnd; y++)
				{
					for (int x = 0, xEnd = attributes.GetLength(2); x < xEnd; x++)
					{
						var block = blockSingleton.GetBlock(attributes[z, y, x]);
						block.transform.position = new Vector3(chunkWorldPos.x + x, chunkWorldPos.y + y, chunkWorldPos.z + z);

						blocks[z, y, x] = block;
					}
				}
			}

			/// @todo プール化
			// チャンク
			var chunk = new Chunk();
			// 作り直す
			chunk.Renew(blocks, chunkPos);

			return chunk;
		}
	}
}
