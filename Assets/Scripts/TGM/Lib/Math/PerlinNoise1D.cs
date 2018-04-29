// ***********************************************************************
// Assembly         : Assembly-CSharp
// Author           : ただのごみ
// Created          : 04-28-2018
//
// Last Modified By : ただのごみ
// Last Modified On : 04-29-2018
// ***********************************************************************
// <copyright file="PerlinNoiseID.cs" company="">
//     Copyright (c) ただのごみ. Please read LICENSE file. If it is nothing, all rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace TGM.Lib.Math
{
	/// <summary>
	/// １次元パーリンノイズを生成する
	/// </summary>
	public class PerlinNoise1D
	{
		/// <summary>
		/// シード値
		/// </summary>
		private readonly int seed;

		/// <summary>
		/// コンストラクタ <see cref="PerlinNoise1D" /> class.
		/// </summary>
		public PerlinNoise1D() : this(UnityEngine.Random.Range(int.MinValue, int.MaxValue))
		{
		}

		/// <summary>
		/// コンストラクタ <see cref="PerlinNoise1D" /> class.
		/// </summary>
		/// <param name="seed">シード値</param>
		/// この値が大きければ大きな波になる</param>
		public PerlinNoise1D(int seed)
		{
			this.seed = seed;
		}

		/// <summary>
		/// １次元パーリンノイズを生成する
		/// </summary>
		/// <param name="x">x座標</param>
		/// <returns>波形の高さ</returns>
		public float Noise(float x)
		{
			// 整数部分と小数部分に分ける
			float fx = x % 1;
			int ix = (int)x;

			float wave = PerlinNoise1D.Wavelet(fx, Random.GetSmallRandom(unchecked(this.seed + ix)));
			float nextWave = PerlinNoise1D.Wavelet(fx - 1f, Random.GetSmallRandom(unchecked(this.seed + ix + 1)));

			return Mathf.Lerp(wave, nextWave, fx);
		}

		/// <summary>
		/// ウェーブレット関数
		/// </summary>
		/// <param name="t">時間</param>
		/// <param name="a">原点での傾き
		/// この値が大きければ、高い波形になる</param>
		/// <returns>高さ</returns>
		/// <remarks>ウェーブレット関数の作る波形の定義
		/// ・原点の高さは0
		/// ・原点での傾きが0でない
		/// ・原点からの距離1の地点でなだらかに値が0になる
		/// ・原点からの距離が1より大きい地点では値が0
		/// ・-1～1の範囲を積分すると値は0
		/// ・原点で符号反転すると左右で線対称な波形になる</remarks>
		private static float Wavelet(float t, float a)
		{
			if (a == 0f)
			{
				Debug.LogWarning("原点での傾きが0ではウェーブレット関数の定義を満たせません");
				return 0f;
			}

			// C(t) = 1 - (6|t^5| - 15t^4 + 10|t^3|)
			float c = 1f - (6f * Mathf.Abs(Mathf.Pow(t, 5)) - 15f * Mathf.Pow(t, 4) + 10f * Mathf.Abs(Mathf.Pow(t, 3)));
			// L(t) = at
			float l = a * t;

			return c * l;
		}
	}
}
