using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TGM.Lib.Utility;

public class PerlinNoise1DTest : MonoBehaviour
{
	public int seed;
	public float amplitude;
	public float accuracy;
	public int maxStep;
	public float width;

	// Use this for initialization
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
	private void Update()
	{
	}
}
