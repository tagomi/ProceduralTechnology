// ***********************************************************************
// Assembly         : Assembly-CSharp
// Author           : ただのごみ
// Created          : 04-30-2018
//
// Last Modified By : ただのごみ
// Last Modified On : 04-30-2018
// ***********************************************************************
// <copyright file="MapTest.cs" company="">
//     Copyright (c) ただのごみ. Please read LICENSE file. If it is nothing, all rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using TGM.Procedural.Map;
using UnityEngine;

/// <summary>
/// Class MapTest.
/// </summary>
public class MapTest : MonoBehaviour
{
	/// <summary>
	/// The map
	/// </summary>
	private MapBehaviour map;

	/// <summary>
	/// The amplitude
	/// </summary>
	public float amplitude;
	/// <summary>
	/// The wave period
	/// </summary>
	public float wavePeriod;

	/// <summary>
	/// 波の大きさの1オクターブ毎の減少率
	/// </summary>
	public float amplitudeDecreasingRate;
	/// <summary>
	/// 波の周期の1オクターブ毎の減少率
	/// </summary>
	public float wavePeriodDecreasingRate;

	/// <summary>
	/// ノイズを何回重ねるか
	/// </summary>
	public int octaves;

	/// <summary>
	/// The seed
	/// </summary>
	public int seed;

	// Use this for initialization
	/// <summary>
	/// Starts this instance.
	/// </summary>
	private void Start()
	{
		this.map = this.gameObject.AddComponent<MapBehaviour>();
		this.map.Initialize(new MapGenerator(this.amplitude, this.wavePeriod, this.amplitudeDecreasingRate, this.wavePeriodDecreasingRate, this.octaves, this.seed));

		for (int z = 0; z < 4; z++)
		{
			for (int x = 0; x < 4; x++)
			{
				this.map.CreateChunk(new TGM.Lib.Vector.IntVector3(x, 0, z));
			}
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
