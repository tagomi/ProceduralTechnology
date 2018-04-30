// ***********************************************************************
// Assembly         : Assembly-CSharp
// Author           : ただのごみ
// Created          : 04-30-2018
//
// Last Modified By : ただのごみ
// Last Modified On : 04-30-2018
// ***********************************************************************
// <copyright file="MapGenerator.cs" company="">
//     Copyright (c) ただのごみ. Please read LICENSE file. If it is nothing, all rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using TGM.Lib.Math;
using TGM.Lib.Vector;
using TGM.Procedural.Entity.Block;
using UnityEngine;

namespace TGM.Procedural.Map
{
	/// <summary>
	/// マップ生成機
	/// </summary>
	public class MapGenerator
	{
		/// <summary>
		/// 波の大きさ
		/// </summary>
		private readonly float amplitude;
		/// <summary>
		/// 波の周期
		/// </summary>
		private readonly float wavePeriod;

		/// <summary>
		/// ノイズ生成機
		/// </summary>
		private readonly PerlinNoise2D noiseGenerator;

		/// <summary>
		/// コンストラクタ <see cref="MapGenerator" /> class.
		/// </summary>
		/// <param name="amplitude">波の大きさ</param>
		/// <param name="wavePeriod">波の周期</param>
		/// <param name="seed">シード値</param>
		public MapGenerator(float amplitude, float wavePeriod, int seed)
		{
			this.amplitude = amplitude;
			this.wavePeriod = wavePeriod;
			this.noiseGenerator = new PerlinNoise2D(seed);
		}

		/// <summary>
		/// 1チャック分のブロック属性を決める
		/// </summary>
		/// <param name="chunkPos">チャンク</param>
		/// <returns>1チャンク分のブロック属性</returns>
		public BlockAttribute[,,] CreateChunkBlockAttributes(IntVector3 chunkPos)
		{
			var attributes = Chunk.CreateChunkBlockArray<BlockAttribute>(() => new BlockAttribute(BlockTypes.Empty));

			// チャンク座標をワールド座標に変換
			IntVector3 chunkWorldPos = Chunk.ConvertChunkPosToWorldPos(chunkPos);
			// 地表の頂点座標を求める
			var peeks = this.CreateChunkPeeks(chunkWorldPos);

			if ((attributes.GetLength(0) != peeks.GetLength(0)) || (attributes.GetLength(2) != peeks.GetLength(1)))
			{
				Debug.LogError($"生成した地表の頂点座標配列のサイズが不正です サイズ：[{peeks.GetLength(0)}, {peeks.GetLength(1)}]");
				return attributes;
			}

			// 地表のブロック属性を決める
			for (int z = 0, zEnd = attributes.GetLength(0); z < zEnd; z++)
			{
				for (int x = 0, xEnd = attributes.GetLength(2); x < xEnd; x++)
				{
					int peek = peeks[z, x];
					// 頂点がこのチャンクより下にあるなら飛ばす
					if (peek < chunkWorldPos.y)
					{
						continue;
					}

					for (int y = 0; y < peek; y++)
					{
						attributes[z, y, x] = new BlockAttribute(BlockTypes.Dirt);
					}
					// 地表の1ブロックは草にする
					attributes[z, peek, x] = new BlockAttribute(BlockTypes.Grass);
				}
			}

			return attributes;
		}

		/// <summary>
		/// 1チャンク分の地表の頂点座標を求める
		/// </summary>
		/// <param name="chunkWorldPos">チャンク座標のワールド座標</param>
		/// <returns>1チャンク分の頂点座標</returns>
		private int[,] CreateChunkPeeks(IntVector3 chunkWorldPos)
		{
			// 地形の頂点
			var peeks = new int[Chunk.ZSize, Chunk.XSize];

			for (int z = 0, iEnd = peeks.GetLength(0); z < iEnd; z++)
			{
				for (int x = 0, jEnd = peeks.GetLength(1); x < jEnd; x++)
				{
					// パーリンノイズの結果を四捨五入してから頂点の座標とする
					peeks[z, x] = (int)Mathf.Round(this.noiseGenerator.Noise((chunkWorldPos.x + x) / this.wavePeriod, (chunkWorldPos.z + z) / this.wavePeriod));
				}
			}

			return peeks;
		}
	}
}
