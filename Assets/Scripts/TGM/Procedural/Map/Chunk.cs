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
using UnityEngine;

namespace TGM.Procedural.Map
{
	/// <summary>
	/// 1チャックを表す
	/// </summary>
	public class Chunk
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
		/// チャンクX座標
		/// </summary>
		private int x;
		/// <summary>
		/// チャンクY座標
		/// </summary>
		private int y;
		/// <summary>
		/// チャンクZ座標
		/// </summary>
		private int z;

		/// <summary>
		/// マップを構成するブロック
		/// </summary>
		private GameObject[,,] blocks = new GameObject[Chunk.ZSize, Chunk.YSize, Chunk.XSize];

		/// <summary>
		/// チャックを作り直す
		/// </summary>
		/// <param name="blocks">新しいチャンクを構成するブロック</param>
		public void Renew(GameObject[,,] blocks)
		{
			if ((blocks.GetLength(0) != Chunk.ZSize) || (blocks.GetLength(1) != Chunk.YSize) || (blocks.GetLength(2) != Chunk.XSize))
			{
				Debug.LogError($"ブロックの配列サイズは[{Chunk.ZSize}, {Chunk.YSize}, {Chunk.XSize}]でなければなりませんが、[{blocks.GetLength(0)}, {blocks.GetLength(1)}, {blocks.GetLength(2)}]でした");
				return;
			}

			this.Clear();

			this.blocks = blocks;
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
					block.SetActive(false);
				}
			}
		}
	}
}
