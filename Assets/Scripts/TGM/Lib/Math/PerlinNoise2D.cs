using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace TGM.Lib.Math
{
	/// <summary>
	/// 2次元パーリンノイズを生成する
	/// </summary>
	public class PerlinNoise2D
	{
		/// <summary>
		/// シード値
		/// </summary>
		private readonly int seed;

		/// <summary>
		/// ウェーブレット関数の原点での傾きの最大値
		/// この値が大きければ大きな波になる
		/// </summary>
		private readonly float amplitude;

		/// <summary>
		/// x座標のこのクラス内でのズレ
		/// </summary>
		/// <remarks>x座標とy座標が同じ値だとウェーブレット関数の戻り値が同じになってしまうので、対策</remarks>
		private readonly int xOffset;

		/// <summary>
		/// y座標のこのクラス内でのズレ
		/// </summary>
		/// <remarks>x座標とy座標が同じ値だとウェーブレット関数の戻り値が同じになってしまうので、対策</remarks>
		private readonly int yOffset;

		/// <summary>
		/// コンストラクタ <see cref="PerlinNoise2D" /> class.
		/// </summary>
		/// <param name="amplitude">ウェーブレット関数の原点での傾きの最大値
		/// この値が大きければ大きな波になる</param>
		public PerlinNoise2D(float amplitude) : this(UnityEngine.Random.Range(int.MinValue, int.MaxValue), amplitude)
		{
		}

		/// <summary>
		/// コンストラクタ <see cref="PerlinNoise2D" /> class.
		/// </summary>
		/// <param name="seed">シード値</param>
		/// <param name="amplitude">ウェーブレット関数の原点での傾きの最大値
		/// この値が大きければ大きな波になる</param>
		public PerlinNoise2D(int seed, float amplitude)
		{
			this.seed = seed;
			this.amplitude = amplitude;
			var randomGenerator = new System.Random(this.seed);
			this.yOffset = randomGenerator.Next();
			this.xOffset = randomGenerator.Next();

			// 完全に0はまずい
			Assert.IsFalse(Mathf.Approximately(this.amplitude, 0f), "傾きの最大値が0の場合、ウェーブレット関数の定義を満たせません");
		}

		/// <summary>
		/// 2次元パーリンノイズを生成する
		/// </summary>
		/// <param name="x">x座標</param>
		/// <param name="y">y座標</param>
		/// <returns>波形の高さ</returns>
		public float Noise(float x, float y)
		{
			// 整数部分と小数部分に分ける
			float fx = x % 1;
			float fy = y % 1;
			int ix = unchecked((int)x + this.xOffset);
			int iy = unchecked((int)y + this.yOffset);

			// 擬似乱数勾配ベクトルの傾き
			float ax0 = this.amplitude * Random.GetSmallRandom(unchecked(this.seed + ix));
			float ax1 = this.amplitude * Random.GetSmallRandom(unchecked(this.seed + ix + 1));
			float ay0 = this.amplitude * Random.GetSmallRandom(unchecked(this.seed + iy));
			float ay1 = this.amplitude * Random.GetSmallRandom(unchecked(this.seed + iy + 1));

			// ウェーブレット関数を計算する
			float x0y0Wave = PerlinNoise2D.Wavelet(fx, fy, ax0, ay0);
			float x1y0Wave = PerlinNoise2D.Wavelet(fx - 1f, fy, ax1, ay0);
			float x0y1Wave = PerlinNoise2D.Wavelet(fx, fy - 1f, ax0, ay1);
			float x1y1Wave = PerlinNoise2D.Wavelet(fx - 1f, fy - 1f, ax1, ay1);

			// まずx軸方向で線形補間する
			float y0Wave = Mathf.Lerp(x0y0Wave, x1y0Wave, fx);
			float y1Wave = Mathf.Lerp(x0y1Wave, x1y1Wave, fx);

			// 次にy軸方向で線形補間する
			return Mathf.Lerp(y0Wave, y1Wave, fy);
		}

		/// <summary>
		/// ウェーブレット関数
		/// </summary>
		/// <param name="x">単位正方形内のx座標</param>
		/// <param name="y">単位正方形内のy座標</param>
		/// <param name="ax">x軸の原点での傾き
		/// この値が大きければ、高い波形になる</param>
		/// <param name="ay">y軸の原点での傾き
		/// この値が大きければ、高い波形になる</</param>
		/// <returns>高さ</returns>
		/// <remarks>ウェーブレット関数の作る波形の定義
		/// ・原点の高さは0
		/// ・原点での勾配a = (ax, ay)がx軸にもy軸にも平行でない
		/// ・自身の単位正方形の境界でなだらかに値が(0, 0)になる
		/// ・自身の単位正方形の境界を超えた地点では値が(0, 0)
		/// ・(0, 0)から(1, 1)の範囲を重積分すると値が0
		/// ・原点で符号反転すると点対称な波形になる</remarks>
		private static float Wavelet(float x, float y, float ax, float ay)
		{
			if ((ax == 0f) || (ay == 0f))
			{
				Debug.LogWarning("原点での傾きが0ではウェーブレット関数の定義を満たせません");
				return 0f;
			}

			// C(x, y) = C_x(x)C_y(y)
			// C(t) = 1-3t^2+2|t|^3
			float cx = 1f - 3f * Mathf.Pow(x, 2) + 2f * Mathf.Pow(Mathf.Abs(x), 3);
			float cy = 1f - 3f * Mathf.Pow(y, 2) + 2f * Mathf.Pow(Mathf.Abs(y), 3);
			float c = cx * cy;
			// L(x, y) = a_x*x + a_y*y
			// L(t) = at
			float l = ax * x + ay * y;

			return c * l;
		}
	}
}
