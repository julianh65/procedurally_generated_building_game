//////////////////////////////////////////////////////
// MegaSplat
// Copyright (c) Jason Booth
//////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JBooth.MicroSplat
{
#if __MICROSPLAT__ && (__MICROSPLAT_STREAMS__ || __MICROSPLAT_GLOBALTEXTURE__ || __MICROSPLAT_SNOW__)
   public class TerrainPaintJob : ScriptableObject
   {
      public Terrain terrain;
      public Texture2D streamTex;
      public Texture2D tintTex;
      public Texture2D snowTex;
      public Collider collider;

      public byte[] streamBuffer;
      public byte [] tintBuffer;
      public byte [] snowBuffer;

      public void RegisterUndo()
      {
         if (streamTex != null)
         {
            streamBuffer = streamTex.GetRawTextureData();
            UnityEditor.Undo.RegisterCompleteObjectUndo(this, "Terrain Edit");
         }
         if (tintTex != null)
         {
            tintBuffer = tintTex.GetRawTextureData ();
            UnityEditor.Undo.RegisterCompleteObjectUndo (this, "Terrain Edit");
         }
         if (snowTex != null)
         {
            snowBuffer = snowTex.GetRawTextureData ();
            UnityEditor.Undo.RegisterCompleteObjectUndo (this, "Terrain Edit");
         }
      }

      public void RestoreUndo()
      {
         if (streamBuffer != null && streamBuffer.Length > 0)
         {
            streamTex.LoadRawTextureData(streamBuffer);
            streamTex.Apply();
         }
         if (tintTex != null && tintBuffer.Length > 0)
         {
            tintTex.LoadRawTextureData (tintBuffer);
            tintTex.Apply ();
         }
         if (snowBuffer != null && snowBuffer.Length > 0)
         {
            snowTex.LoadRawTextureData (streamBuffer);
            snowTex.Apply ();
         }
      }
   }
   #endif
}
