// ***********************************************************************
// Assembly         : Assembly-CSharp
// Author           : ただのごみ
// Created          : 04-18-2018
//
// Last Modified By : ただのごみ
// Last Modified On : 04-18-2018
// ***********************************************************************
// <copyright file="IReference.cs" company="">
//     Copyright (c) ただのごみ. Please read LICENSE file. If it is nothing, all rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace TGM.Lib.Object
{
	/// <summary>
	/// 参照を表すインターフェイス
	/// </summary>
	public interface IReference
	{
		/// <summary>
		/// 引数の<see cref="System.Object" />がこのインスタンスと同値かを返す
		/// </summary>
		/// <param name="obj">このインスタンスと比較をする<see cref="System.Object" />のインスタンス</param>
		/// <returns><c>true</c>引数の<see cref="System.Object" />と同値。そうでなければ、<c>false</c></returns>
		bool Equals(object obj);

		/// <summary>
		/// このインスタンスのハッシュコードを返す
		/// </summary>
		/// <returns>ハッシュコード</returns>
		int GetHashCode();

		/// <summary>
		/// このインスタンスの説明である<see cref="System.String" />を返す
		/// </summary>
		/// <returns>このインスタンスの説明である<see cref="System.String" /></returns>
		string ToString();
	}
}
