// ***********************************************************************
// Assembly         : Assembly-CSharp
// Author           : ただのごみ
// Created          : 04-29-2018
//
// Last Modified By : ただのごみ
// Last Modified On : 04-29-2018
// ***********************************************************************
// <copyright file="PerlinNoise1DTest.cs" company="">
//     Copyright (c) ただのごみ. Please read LICENSE file. If it is nothing, all rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TGM.Lib.Utility;

/// <summary>
/// Class PerlinNoise1DTest.
/// </summary>
public class PerlinNoise1DTest : MonoBehaviour
{
	/// <summary>
	/// The seed
	/// </summary>
	public int seed;
	/// <summary>
	/// The amplitude
	/// </summary>
	public float amplitude;
	/// <summary>
	/// The accuracy
	/// </summary>
	public float accuracy;
	/// <summary>
	/// The maximum step
	/// </summary>
	public int maxStep;
	/// <summary>
	/// The width
	/// </summary>
	public float width;

	// Use this for initialization
	/// <summary>
	/// Starts this instance.
	/// </summary>
	private void Start()
	{
		var perlin = new TGM.Lib.Math.PerlinNoise1D(this.seed, this.amplitude);
		var line = this.GetOrAddComponent<LineRenderer>();
		line.positionCount = this.maxStep;

		for (int i = 0; i < this.maxStep; i++)
		{
			float x = this.accuracy * (float)i;
			float value = perlin.Noise(x);

			line.SetPosition(i, new Vector3(x * this.width, 0f, value));
		}
	}

	// Update is called once per frame
	/// <summary>
	/// Updates this instance.
	/// </summary>
	private void Update()
	{
	}
}
