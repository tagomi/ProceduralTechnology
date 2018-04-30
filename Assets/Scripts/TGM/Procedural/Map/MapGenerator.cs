// ***********************************************************************
// Assembly         : Assembly-CSharp
// Author           : ただのごみ
// Created          : 04-30-2018
//
// Last Modified By : ただのごみ
// Last Modified On : 05-01-2018
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
using UnityEngine.Assertions;

namespace TGM.Procedural.Map
{
	/// <summary>
	/// マップ生成機
	/// </summary>
	/// <seealso cref="TGM.Procedural.Map.IMapGenerator" />
	public class MapGenerator : IMapGenerator
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
		/// 波の大きさの1オクターブ毎の変化率
		/// </summary>
		private readonly float amplitudeDecreasingRate;
		/// <summary>
		/// 波の周期の1オクターブ毎の変化率
		/// </summary>
		private readonly float wavePeriodDecreasingRate;

		/// <summary>
		/// ノイズを何回重ねるか
		/// </summary>
		private readonly int octaves;

		/// <summary>
		/// ノイズ生成機
		/// </summary>
		private readonly PerlinNoise2D noiseGenerator;

		/// <summary>
		/// コンストラクタ <see cref="MapGenerator" /> class.
		/// </summary>
		/// <param name="amplitude">波の大きさ</param>
		/// <param name="wavePeriod">波の周期</param>
		/// <param name="amplitudeDecreasingRate">波の大きさの1オクターブ毎の変化率</param>
		/// <param name="wavePeriodDecreasingRate">波の周期の1オクターブ毎の変化率</param>
		/// <param name="octaves">ノイズを何回重ねるか</param>
		/// <param name="seed">シード値</param>
		public MapGenerator(float amplitude, float wavePeriod, float amplitudeDecreasingRate, float wavePeriodDecreasingRate, int octaves, int seed)
		{
			this.amplitude = amplitude;
			this.wavePeriod = wavePeriod;
			this.amplitudeDecreasingRate = amplitudeDecreasingRate;
			this.wavePeriodDecreasingRate = wavePeriodDecreasingRate;
			this.octaves = octaves;
			this.noiseGenerator = new PerlinNoise2D(seed);

			Assert.IsTrue(this.amplitude > 0, $"波の大きさは0以下にできません");
			Assert.IsTrue(this.wavePeriod > 0, $"波の周期は0以下にできません");
			Assert.IsTrue((0 < this.amplitudeDecreasingRate) && (this.amplitudeDecreasingRate <= 1f), $"波の大きさの変化率は(0, 1]でなければなりません");
			Assert.IsTrue((0 < this.wavePeriodDecreasingRate) && (this.wavePeriodDecreasingRate <= 1f), $"波の周期の変化率は(0, 1]でなければなりません");
			Assert.IsTrue(this.octaves > 0, $"ノイズを重ねる回数は0以下にできません");
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
			// 地形の頂点(初期値は0)
			var peeks = new int[Chunk.ZSize, Chunk.XSize];

			// 初期値
			float amplitude = this.amplitude;
			float wavePeriod = this.wavePeriod;

			// 波の大きさの合計値
			float amplitudeSum = 0;
			// 非整数ブラウン運動
			for (int i = 0; i < this.octaves; i++)
			{
				amplitudeSum += amplitude;

				this.MovePeeks(peeks, chunkWorldPos, amplitude, wavePeriod);

				amplitude *= this.amplitudeDecreasingRate;
				wavePeriod *= this.wavePeriodDecreasingRate;
			}
			// ノイズを重ねた事で増加した分だけ高さを下げる
			float amplitudeRate = amplitudeSum / this.amplitude;
			for (int i = 0, iEnd = peeks.GetLength(0); i < iEnd; i++)
			{
				for (int j = 0, jEnd = peeks.GetLength(1); j < jEnd; j++)
				{
					peeks[i, j] = Mathf.RoundToInt((float)peeks[i, j] / amplitudeRate);
				}
			}

			return peeks;
		}

		private void MovePeeks(int[,] peeks, IntVector3 chunkWorldPos, float amplitude, float wavePeriod)
		{
			for (int z = 0, iEnd = peeks.GetLength(0); z < iEnd; z++)
			{
				for (int x = 0, jEnd = peeks.GetLength(1); x < jEnd; x++)
				{
					// パーリンノイズの結果を四捨五入してから頂点の座標とする
					peeks[z, x] += Mathf.RoundToInt(noiseGenerator.Noise((chunkWorldPos.x + x) / wavePeriod, (chunkWorldPos.z + z) / wavePeriod) * amplitude);
				}
			}
		}
	}
}
