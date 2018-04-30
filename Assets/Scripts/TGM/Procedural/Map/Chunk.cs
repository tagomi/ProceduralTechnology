// ***********************************************************************
// Assembly         : Assembly-CSharp
// Author           : ただのごみ
// Created          : 04-30-2018
//
// Last Modified By : ただのごみ
// Last Modified On : 04-30-2018
// ***********************************************************************
// <copyright file="Chunk.cs" company="">
//     Copyright (c) ただのごみ. Please read LICENSE file. If it is nothing, all rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using TGM.Lib.Vector;
using TGM.Procedural.Entity.Block;
using UnityEngine;

namespace TGM.Procedural.Map
{
	/// <summary>
	/// 1チャックを表す
	/// </summary>
	public sealed class Chunk
	{
		/// <summary>
		/// X軸方向のブロック数
		/// </summary>
		public const int XSize = 16;
		/// <summary>
		/// Y軸方向のブロック数
		/// </summary>
		public const int YSize = 16;
		/// <summary>
		/// Z軸方向のブロック数
		/// </summary>
		public const int ZSize = 256;

		/// <summary>
		/// マップを構成するブロック
		/// </summary>
		private BlockBehaviour[,,] blocks;

		/// <summary>
		/// チャンク座標
		/// </summary>
		public IntVector3 ChunkPos
		{
			get;
			private set;
		}

		/// <summary>
		/// 1チャンク分のブロックを収められるサイズの配列を作って返す
		/// </summary>
		/// <typeparam name="T">配列の型</typeparam>
		/// <param name="defaultValue">配列の初期値</param>
		/// <returns>1チャンク分のブロックを収められるサイズの配列</returns>
		public static T[,,] CreateChunkBlockArray<T>(T defaultValue = default(T))
		{
			var array = new T[Chunk.ZSize, Chunk.YSize, Chunk.XSize];

			for (int i = 0; i < Chunk.ZSize; i++)
			{
				for (int j = 0; j < Chunk.YSize; j++)
				{
					for (int k = 0; k < Chunk.XSize; k++)
					{
						array[i, j, k] = defaultValue;
					}
				}
			}

			return array;
		}

		/// <summary>
		/// 1チャンク分のブロックを収められるサイズの配列を作って返す
		/// </summary>
		/// <typeparam name="T">配列の型</typeparam>
		/// <param name="generator">初期値を生成するデリゲート</param>
		/// <returns>1チャンク分のブロックを収められるサイズの配列</returns>
		public static T[,,] CreateChunkBlockArray<T>(Func<T> generator)
		{
			var array = new T[Chunk.ZSize, Chunk.YSize, Chunk.XSize];

			if (generator == null)
			{
				Debug.LogError("初期値を生成するためのデリゲートがnullでした");
				return array;
			}

			for (int i = 0; i < Chunk.ZSize; i++)
			{
				for (int j = 0; j < Chunk.YSize; j++)
				{
					for (int k = 0; k < Chunk.XSize; k++)
					{
						array[i, j, k] = generator();
					}
				}
			}

			return array;
		}

		/// <summary>
		/// チャンク座標をワールド座標に変換する
		/// </summary>
		/// <param name="chunkPos">チャンク座標</param>
		/// <returns>ワールド座標</returns>
		public static IntVector3 ConvertChunkPosToWorldPos(IntVector3 chunkPos) => new IntVector3(chunkPos.x * Chunk.XSize, chunkPos.y * Chunk.YSize, chunkPos.z * Chunk.ZSize);

		/// <summary>
		/// チャックを作り直す
		/// </summary>
		/// <param name="blocks">新しいチャンクを構成するブロック</param>
		/// <param name="chunkPos">チャンク座標</param>
		public void Renew(BlockBehaviour[,,] blocks, IntVector3 chunkPos)
		{
			if ((blocks.GetLength(0) != Chunk.ZSize) || (blocks.GetLength(1) != Chunk.YSize) || (blocks.GetLength(2) != Chunk.XSize))
			{
				Debug.LogError($"ブロックの配列サイズは[{Chunk.ZSize}, {Chunk.YSize}, {Chunk.XSize}]でなければなりませんが、[{blocks.GetLength(0)}, {blocks.GetLength(1)}, {blocks.GetLength(2)}]でした");
				return;
			}

			this.Clear();

			this.blocks = blocks;
			this.ChunkPos = chunkPos;
		}

		/// <summary>
		/// 破棄処理
		/// </summary>
		public void Destroy()
		{
			this.Clear();
		}

		/// <summary>
		/// クリア
		/// </summary>
		private void Clear()
		{
			foreach (var block in this.blocks)
			{
				if (block != null)
				{
					// ブールされている事が前提の処理
					block.gameObject.SetActive(false);
				}
			}
		}
	}
}
