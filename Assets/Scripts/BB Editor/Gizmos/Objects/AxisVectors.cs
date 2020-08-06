﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace RuntimeGizmos
{
	public class AxisVectors
	{
		public List<Vector3> x = new List<Vector3>();
		public List<Vector3> y = new List<Vector3>();
		public List<Vector3> z = new List<Vector3>();
        public List<Vector3> nx = new List<Vector3>();
		public List<Vector3> ny = new List<Vector3>();
		public List<Vector3> nz = new List<Vector3>();
		public List<Vector3> all = new List<Vector3>();

		public void Add(AxisVectors axisVectors)
		{
			x.AddRange(axisVectors.x);
			y.AddRange(axisVectors.y);
			z.AddRange(axisVectors.z);
            nx.AddRange(axisVectors.nx);
			ny.AddRange(axisVectors.ny);
			nz.AddRange(axisVectors.nz);
			all.AddRange(axisVectors.all);
		}

		public void Clear()
		{
			x.Clear();
			y.Clear();
			z.Clear();
            nx.Clear();
			ny.Clear();
			nz.Clear();
			all.Clear();
		}
	}
}