// ***********************************************************************
// Assembly         : Assembly-CSharp
// Author           : ただのごみ
// Created          : 04-01-2018
//
// Last Modified By : ただのごみ
// Last Modified On : 04-01-2018
// ***********************************************************************
// <copyright file="Reference.cs" company="">
//     Copyright (c) ただのごみ. Released under the MIT license.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace TGM.Lib.Object
{
	/// <summary>
	/// インスタンスを参照するだけ
	/// </summary>
	/// <typeparam name="T">参照先の型</typeparam>
	public class Reference<T> where T : class
	{
		/// <summary>
		/// 参照先
		/// </summary>
		public readonly T target;

		/// <summary>
		/// コンストラクタ <see cref="Reference{T}" /> class.
		/// </summary>
		/// <param name="target">参照先</param>
		public Reference(T target)
		{
			this.target = target;
		}

		/// <summary>
		/// 引数の<see cref="System.Object" />がこのインスタンスと同値かを返す
		/// </summary>
		/// <param name="obj">このインスタンスと比較をする<see cref="System.Object" />のインスタンス</param>
		/// <returns><c>true</c>引数の<see cref="System.Object" />と同値。そうでなければ、<c>false</c></returns>
		public override bool Equals(object obj)
		{
			return target.Equals(obj);
		}

		/// <summary>
		/// このインスタンスのハッシュコードを返す
		/// </summary>
		/// <returns>ハッシュコード</returns>
		public override int GetHashCode()
		{
			return this.target.GetHashCode();
		}

		/// <summary>
		/// このインスタンスの説明である<see cref="System.String" />を返す
		/// </summary>
		/// <returns>このインスタンスの説明である<see cref="System.String" /></returns>
		public override string ToString()
		{
			return this.target.ToString();
		}
	}
}
