// ***********************************************************************
// Assembly         : Assembly-CSharp
// Author           : ただのごみ
// Created          : 04-29-2018
//
// Last Modified By : ただのごみ
// Last Modified On : 04-29-2018
// ***********************************************************************
// <copyright file="PerlinNoise2DTest.cs" company="">
//     Copyright (c) ただのごみ. Please read LICENSE file. If it is nothing, all rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Test
{
	/// <summary>
	/// Class PerlinNoise2DTest.
	/// </summary>
	/// <seealso cref="UnityEngine.MonoBehaviour" />
	public class PerlinNoise2DTest : MonoBehaviour
	{
		/// <summary>
		/// The x width
		/// </summary>
		public int xWidth;
		/// <summary>
		/// The y width
		/// </summary>
		public int yWidth;
		/// <summary>
		/// The seed
		/// </summary>
		public int seed;
		/// <summary>
		/// The amplitude
		/// </summary>
		public float amplitude;
		/// <summary>
		/// The wave period
		/// </summary>
		public float wavePeriod;

		/// <summary>
		/// Starts this instance.
		/// </summary>
		private void Start()
		{
			var noiseMaker = new TGM.Lib.Math.PerlinNoise2D(this.seed);

			for (int i = 0; i < this.yWidth; i++)
			{
				for (int j = 0; j < this.xWidth; j++)
				{
					var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
					cube.transform.position = new Vector3(j, noiseMaker.Noise((float)j / this.wavePeriod, (float)i / this.wavePeriod) * this.amplitude, i);
				}
			}
		}
	}
}
