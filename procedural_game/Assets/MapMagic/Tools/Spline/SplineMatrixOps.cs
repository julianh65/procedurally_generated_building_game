using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Den.Tools;
using Den.Tools.Splines;
using Den.Tools.Matrices;
using Den.Tools.GUI;


public static class SplineMatrixOps
{
	public static void Stroke (SplineSys spline, MatrixWorld matrix, 
		bool white=false, float intensity=1,
		bool antialiased=false, bool padOnePixel=false)
	/// Draws a line on matrix
	/// White will fill the line with 1, when disabled it will use spline height
	/// PaddedOnePixel works similarly to AA, but fills border pixels with full value (to create main tex for the mask)
	{
		foreach (Line line in spline.lines)
			for (int s=0; s<line.segments.Length; s++)
		{
			int numSteps = (int)(line.segments[s].length / matrix.PixelSize.x * 0.1f + 1);

			Vector3 startPos = line.segments[s].start.pos;
			Vector3 prevCoord = matrix.WorldToPixelInterpolated(startPos.x, startPos.z);
			float prevHeight = white ? intensity : (startPos.y / matrix.worldSize.y);

			for (int i=0; i<numSteps; i++)
			{
				float percent = numSteps!=1 ? 1f*i/(numSteps-1) : 1;
				Vector3 pos = line.segments[s].GetPoint(percent);
				float posHeight = white ? intensity : (pos.y / matrix.worldSize.y);
				pos = matrix.WorldToPixelInterpolated(pos.x, pos.z);

				matrix.Line(
					new Vector2(prevCoord.x, prevCoord.z), 
					new Vector2(pos.x, pos.z), 
					prevHeight,
					posHeight,
					antialised:antialiased,
					paddedOnePixel:padOnePixel,
					endInclusive:i==numSteps-1);

				prevCoord = pos;
				prevHeight = posHeight;
			}
		}
	}


	public static void Silhouette (SplineSys spline, MatrixWorld matrix, bool strokePrepared=false)
	/// Fills all pixels within closed spline with 1, and all outer pixels with 0
	/// Pixels directly in the spline are filled with 0.5
	/// Requires an empty matrix (can use matrix with the stroke, in this case strokePrepared should be set to true)
	{
		if (!strokePrepared) 
			Stroke (spline, matrix, white:true, intensity:0.5f, antialiased:false, padOnePixel:false);

		CoordRect rect = matrix.rect;
		Coord min = rect.Min; Coord max = rect.Max;

		for (int x=min.x; x<max.x; x++)
			for (int z=min.z; z<max.z; z++)
			{
				int pos = (z-rect.offset.z)*rect.size.x + x - rect.offset.x;
				
				if (matrix.arr[pos] < 0.01f) //free from stroke and fill
				{
					Vector3 pixelPos = matrix.PixelToWorld(x, z, center:true);
					bool handness = spline.Handness(pixelPos) >= 0;

					matrix.PaintBucket(new Coord(x,z), handness ? 0.75f : 0.25f);
				}
			}
	}

	public static void PaintBucket (this MatrixWorld matrix, Coord coord, float val, float threshold=0.0001f, int maxIterations=10)
	/// Like a paintBucket tool in photoshop
	/// Fills all zero (lower than threshold) values with val, until meets borders
	/// Doesnt guarantee filling (areas after the corner could be missed)
	/// Use threshold to change between mask -1 or 0
	{
		CoordRect rect = matrix.rect;
		Coord min = rect.Min; Coord max = rect.Max;

		MatrixOps.Stripe stripe = new MatrixOps.Stripe( Mathf.Max(rect.size.x, rect.size.z) );

		stripe.length = rect.size.x;

		matrix[coord] = -256; //starting mask

		//first vertical spread is one row-only
		MatrixOps.ReadRow(stripe, matrix, coord.x, matrix.rect.offset.z);
		PaintBucketMaskStripe(stripe, threshold);
		MatrixOps.WriteRow(stripe, matrix, coord.x, matrix.rect.offset.z);

		for (int i=0; i<maxIterations; i++) //ten tries, but hope it will end before that
		{
			bool change = false;

			//horizontally
			for (int z=min.z; z<max.z; z++)
			{
				MatrixOps.ReadLine(stripe, matrix, rect.offset.x, z);
				change = PaintBucketMaskStripe(stripe, threshold)  ||  change;
				MatrixOps.WriteLine(stripe, matrix, rect.offset.x, z);
			}

			//vertically
			for (int x=min.x; x<max.x; x++)
			{
				MatrixOps.ReadRow(stripe, matrix, x, matrix.rect.offset.z);
				change = PaintBucketMaskStripe(stripe, threshold)  ||  change;
				MatrixOps.WriteRow(stripe, matrix, x, matrix.rect.offset.z);
			}

			if (!change)
				break;

			//if (i==maxIterations-1 && !change)
			//	Debug.Log("Reached max iterations");
		}

		//filling masked values with val
		for (int i=0; i<matrix.arr.Length; i++)
			if (matrix.arr[i] < -255) matrix.arr[i] = val;
	}

	private static bool PaintBucketMaskStripe (MatrixOps.Stripe stripe, float threshold=0.0001f)
	/// Fills stripe until first unmasked value with -256
	/// Returns true if anything masked
	{
		bool changed = false;

		//to right
		bool masking = false;
		for (int i=0; i<stripe.length; i++)
		{
			if (stripe.arr[i] < -255)
			{
				masking = true;
				continue;
			}

			if (stripe.arr[i] < threshold)
			{
				if (masking) 
				{
					stripe.arr[i] = -256;
					changed = true;
				}
			}
			else
				masking = false;
		}

		//to left
		masking = false;
		for (int i=stripe.length-1; i>=0; i--)
		{
			if (stripe.arr[i] < -255)
			{
				masking = true;
				continue;
			}

			if (stripe.arr[i] < threshold)
			{
				if (masking) 
				{
					stripe.arr[i] = -256;
					changed = true;
				}
			}
			else
				masking = false;
		}

		return changed;
	}
}
