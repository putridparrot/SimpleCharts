﻿using System;
using System.Collections.Generic;
using System.Linq;
using SkiaSharp;

namespace SimpleCharts
{
	/// <summary>
	/// ![chart](../images/Donut.png)
	/// 
	/// A donut chart.
	/// </summary>
	public class DonutChart : Chart
	{
		public DonutChart()
		{
			AllowsZoom = false;
		}
		#region Properties

		/// <summary>
		/// Gets or sets the radius of the hole in the center of the chart.
		/// </summary>
		/// <value>The hole radius.</value>
		public float HoleRadius { get; set; } = 0;

		#endregion

		#region Methods

		public override void DrawContent(SKCanvas canvas, int width, int height)
		{
			this.DrawCaption(canvas, width, height);
			using (new SKAutoCanvasRestore(canvas))
			{
				canvas.Translate(width / 2, height / 2);
				var sumValue = this.Entries?.Sum(x => Math.Abs(GetValue(x))) ?? 0;
				var radius = (Math.Min(width, height) - (2 * Margin)) / 2;

				var start = 0.0f;
				for (int i = 0; i < this.Entries.Count(); i++)
				{
					var entry = this.Entries.ElementAt(i);
					var end = start + (Math.Abs(GetValue(entry)) / sumValue);

					// Sector
					var path = RadialHelpers.CreateSectorPath(start, end, radius, radius * this.HoleRadius);
					using (var paint = new SKPaint
					{
						Style = SKPaintStyle.Fill,
						Color = GetColor(entry),
						IsAntialias = true,
					})
					{
						canvas.DrawPath(path, paint);
					}

					start = end;
				}
			}
		}

		private void DrawCaption(SKCanvas canvas, int width, int height)
		{
			var sumValue = this.Entries.Sum(x => Math.Abs(GetValue(x)));
			var rightValues = new List<object>();
			var leftValues = new List<object>();

			int i = 0;
			var current = 0.0f;
			while (i < this.Entries?.Count() && (current < sumValue / 2))
			{
				var entry = this.Entries.ElementAt(i);
				rightValues.Add(entry);
				current += Math.Abs(GetValue(entry));
				i++;
			}

			while (i < this.Entries.Count())
			{
				var entry = this.Entries.ElementAt(i);
				leftValues.Add(entry);
				i++;
			}

			leftValues.Reverse();

			this.DrawCaptionElements(canvas, width, height, rightValues, false);
			this.DrawCaptionElements(canvas, width, height, leftValues, true);
		}

		#endregion
	}
}