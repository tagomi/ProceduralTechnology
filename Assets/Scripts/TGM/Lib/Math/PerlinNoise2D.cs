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
		/// <param name="quality">乱数の品質</param>
		public PerlinNoise2D(int quality = 256) : this(UnityEngine.Random.Range(int.MinValue, int.MaxValue), quality)
		{
		}

		/// <summary>
		/// コンストラクタ <see cref="PerlinNoise2D" /> class.
		/// </summary>
		/// <param name="seed">シード値
		/// この値が大きければ大きな波になる</param>
		/// <param name="quality">乱数の品質</param>
		public PerlinNoise2D(int seed, int quality = 256)
		{
			this.seed = seed;

			UnityEngine.Random.InitState(this.seed);
			this.yOffset = UnityEngine.Random.Range(0, quality);
			this.xOffset = UnityEngine.Random.Range(0, quality);
			this.randomValues = new int[quality];
			for (int i = 0; i < quality; i++)
			{
				this.randomValues[i] = UnityEngine.Random.Range(0, quality);
			}

			Assert.IsTrue(quality > 0, "品質は0以下にできません");
		}

		/// <summary>
		/// 2次元パーリンノイズを生成する
		/// </summary>
		/// <param name="x">x座標</param>
		/// <param name="y">y座標</param>
		/// <returns>波形の高さ</returns>
		public float Noise(float x, float y)
		{
			if (this.randomValues.Length <= 0)
			{
				Debug.LogWarning("品質が0以下のため、乱数を取得できません");
				return 0;
			}

			// 整数部分と小数部分に分ける
			float fx = x % 1;
			float fy = y % 1;
			int ix = (int)x;
			int iy = (int)y;

			// 擬似乱数勾配ベクトルの傾き
			int length = this.randomValues.Length;
			float ax0y0x = Random.GetSmallRandom(this.randomValues[Mathf.Abs(this.randomValues[Mathf.Abs(this.randomValues[Mathf.Abs(ix) % length] + this.xOffset) % length] + iy) % length]);
			float ax1y0x = Random.GetSmallRandom(this.randomValues[Mathf.Abs(this.randomValues[Mathf.Abs(this.randomValues[Mathf.Abs(ix + 1) % length] + this.xOffset) % length] + iy) % length]);
			float ax0y1x = Random.GetSmallRandom(this.randomValues[Mathf.Abs(this.randomValues[Mathf.Abs(this.randomValues[Mathf.Abs(ix) % length] + this.xOffset) % length] + iy + 1) % length]);
			float ax1y1x = Random.GetSmallRandom(this.randomValues[Mathf.Abs(this.randomValues[Mathf.Abs(this.randomValues[Mathf.Abs(ix + 1) % length] + this.xOffset) % length] + iy + 1) % length]);
			float ax0y0y = Random.GetSmallRandom(this.randomValues[Mathf.Abs(this.randomValues[Mathf.Abs(this.randomValues[Mathf.Abs(ix) % length] + this.yOffset) % length] + iy) % length]);
			float ax1y0y = Random.GetSmallRandom(this.randomValues[Mathf.Abs(this.randomValues[Mathf.Abs(this.randomValues[Mathf.Abs(ix + 1) % length] + this.yOffset) % length] + iy) % length]);
			float ax0y1y = Random.GetSmallRandom(this.randomValues[Mathf.Abs(this.randomValues[Mathf.Abs(this.randomValues[Mathf.Abs(ix) % length] + this.yOffset) % length] + iy + 1) % length]);
			float ax1y1y = Random.GetSmallRandom(this.randomValues[Mathf.Abs(this.randomValues[Mathf.Abs(this.randomValues[Mathf.Abs(ix + 1) % length] + this.yOffset) % length] + iy + 1) % length]);

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
