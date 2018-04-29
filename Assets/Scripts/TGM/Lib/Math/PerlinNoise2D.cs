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
		/// 0以上配列サイズ未満の乱数が収められている
		/// </summary>
		private readonly int[] randomValues;

		/// <summary>
		/// コンストラクタ <see cref="PerlinNoise2D" /> class.
		/// </summary>
		public PerlinNoise2D() : this(UnityEngine.Random.Range(int.MinValue, int.MaxValue))
		{
		}

		/// <summary>
		/// コンストラクタ <see cref="PerlinNoise2D" /> class.
		/// </summary>
		/// <param name="seed">シード値
		/// この値が大きければ大きな波になる</param>
		public PerlinNoise2D(int seed)
		{
			this.seed = seed;

			UnityEngine.Random.InitState(this.seed);
			this.xOffset = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
			this.yOffset = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
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
			int ix = (int)x;
			int iy = (int)y;

			// 擬似乱数勾配ベクトルの傾き
			UnityEngine.Random.InitState(unchecked(ix * this.xOffset + iy * this.yOffset));
			float ax0y0x = UnityEngine.Random.Range(-1f, 1f);
			float ax0y0y = UnityEngine.Random.Range(-1f, 1f);
			UnityEngine.Random.InitState(unchecked((ix + 1) * this.xOffset + iy * this.yOffset));
			float ax1y0x = UnityEngine.Random.Range(-1f, 1f);
			float ax1y0y = UnityEngine.Random.Range(-1f, 1f);
			UnityEngine.Random.InitState(unchecked(ix * this.xOffset + (iy + 1) * this.yOffset));
			float ax0y1x = UnityEngine.Random.Range(-1f, 1f);
			float ax0y1y = UnityEngine.Random.Range(-1f, 1f);
			UnityEngine.Random.InitState(unchecked((ix + 1) * this.xOffset + (iy + 1) * this.yOffset));
			float ax1y1x = UnityEngine.Random.Range(-1f, 1f);
			float ax1y1y = UnityEngine.Random.Range(-1f, 1f);

			// ウェーブレット関数を計算する
			float x0y0Wave = PerlinNoise2D.Wavelet(fx, fy, ax0y0x, ax0y0y);
			float x1y0Wave = PerlinNoise2D.Wavelet(fx - 1f, fy, ax1y0x, ax0y1y);
			float x0y1Wave = PerlinNoise2D.Wavelet(fx, fy - 1f, ax0y1x, ax0y1y);
			float x1y1Wave = PerlinNoise2D.Wavelet(fx - 1f, fy - 1f, ax1y1x, ax1y1y);

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
			// C(t) = 1 - (6|t^5| - 15t^4 + 10|t^3|)
			float cx = 1f - (6f * Mathf.Abs(Mathf.Pow(x, 5)) - 15f * Mathf.Pow(x, 4) + 10f * Mathf.Abs(Mathf.Pow(x, 3)));
			float cy = 1f - (6f * Mathf.Abs(Mathf.Pow(y, 5)) - 15f * Mathf.Pow(y, 4) + 10f * Mathf.Abs(Mathf.Pow(y, 3)));
			float c = cx * cy;
			// L(x, y) = a_x*x + a_y*y
			// L(t) = at
			float l = ax * x + ay * y;

			return c * l;
		}
	}
}
