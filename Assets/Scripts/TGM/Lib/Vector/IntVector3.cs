// ***********************************************************************
// Assembly         : Assembly-CSharp
// Author           : ただのごみ
// Created          : 04-30-2018
//
// Last Modified By : ただのごみ
// Last Modified On : 04-30-2018
// ***********************************************************************
// <copyright file="IntVector3.cs" company="">
//     Copyright (c) ただのごみ. Please read LICENSE file. If it is nothing, all rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using UnityEngine;

namespace TGM.Lib.Vector
{
	/// <summary>
	/// Int型のVector3
	/// </summary>
	public struct IntVector3
	{
		/// <summary>
		/// x座標
		/// </summary>
		public int x;
		/// <summary>
		/// y座標
		/// </summary>
		public int y;
		/// <summary>
		/// z座標
		/// </summary>
		public int z;

		/// <summary>
		/// コンストラクタ <see cref="IntVector3"/> struct.
		/// </summary>
		/// <param name="x">x座標</param>
		/// <param name="y">y座標</param>
		/// <param name="z">z座標</param>
		public IntVector3(int x, int y, int z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		/// <summary>
		/// <see cref="IntVector3"/>から<see cref="Vector3"/>への暗黙的変換
		/// </summary>
		/// <param name="intVec">Int型のVector3</param>
		/// <returns>変換結果</returns>
		public static implicit operator Vector3(IntVector3 intVec)
		{
			return new Vector3(intVec.x, intVec.y, intVec.z);
		}

		/// <summary>
		/// <see cref="Vector3"/>から<see cref="IntVector3"/>への暗黙的変換
		/// </summary>
		/// <param name="vec">Vector3</param>
		/// <returns>変換結果</returns>
		public static implicit operator IntVector3(Vector3 vec)
		{
			return new IntVector3((int)vec.x, (int)vec.y, (int)vec.z);
		}
	}
}
