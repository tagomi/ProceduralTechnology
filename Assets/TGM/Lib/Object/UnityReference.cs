// ***********************************************************************
// Assembly         : Assembly-CSharp
// Author           : ただのごみ
// Created          : 04-01-2018
//
// Last Modified By : ただのごみ
// Last Modified On : 04-01-2018
// ***********************************************************************
// <copyright file="UnityReference.cs" company="">
//     Copyright (c) ただのごみ. Please read LICENSE file. If it is nothing, all rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace TGM.Lib.Object
{
	/// <summary>
	/// インスタンスを参照するだけ
	/// </summary>
	/// <typeparam name="T">参照先の型</typeparam>
	/// <seealso cref="TGM.Lib.Object.Reference{T}" />
	public class UnityReference<T> : Reference<T> where T : UnityEngine.Object
	{
		/// <summary>
		/// コンストラクタ <see cref="UnityReference{T}" /> class.
		/// </summary>
		/// <param name="target">参照先</param>
		public UnityReference(T target) : base(target)
		{
		}

		/// <summary>
		/// 破棄されていないObjectへの参照を持っているか
		/// </summary>
		public bool HasAliveRef => this.target != null;

		/// <summary>
		/// このインスタンスのハッシュコードを返す
		/// </summary>
		/// <returns>ハッシュコード。参照先が破棄済みであれば、-1を返す</returns>
		public override int GetHashCode()
		{
			return this.HasAliveRef ? this.target.GetInstanceID() : -1;
		}

		/// <summary>
		/// このインスタンスの説明である<see cref="System.String" />を返す
		/// </summary>
		/// <returns>このインスタンスの説明である<see cref="System.String" /></returns>
		public override string ToString()
		{
			return this.HasAliveRef ? base.ToString() : "null";
		}
	}
}
