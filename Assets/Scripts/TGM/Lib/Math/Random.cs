// ***********************************************************************
// Assembly         : Assembly-CSharp
// Author           : ただのごみ
// Created          : 04-29-2018
//
// Last Modified By : ただのごみ
// Last Modified On : 04-29-2018
// ***********************************************************************
// <copyright file="Random.cs" company="">
//     Copyright (c) ただのごみ. Please read LICENSE file. If it is nothing, all rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace TGM.Lib.Math
{
	/// <summary>
	/// 乱数生成
	/// </summary>
	public class Random
	{
		/// <summary>
		/// [-1, 1)の疑似乱数値を返す
		/// </summary>
		/// <param name="seed">シード値</param>
		/// <returns>[-1, 1)の疑似乱数値</returns>
		/// <remarks>シード値が同じ値であれば、必ず同じ値を返す</remarks>
		public static float GetSmallRandom(int seed)
		{
			// 同じ値であれば、同じ結果になるように毎回作り直す
			var randomGenerator = new System.Random(seed);

			return ((float)randomGenerator.NextDouble() - 0.5f) * 2f;
		}
	}
}
