//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace PosterGen
{
	[System.Serializable]
	public class FrameGen : MonoBehaviour 
	{
		public Vector2 scaler;
		public Vector3 rotate;
		public bool isPoster = true;
		public List<Vector2> divides;
		public bool bevel = false;
		public float bevelamt = 0.01f;
		
		public float border = 0.1f;
		public float tile = 0.25f;

		public Shader ShaderToUseback;
		
		public List<string> textureNamesback = new List<string>();
		public List<Texture2D> texturesToUseback = new List<Texture2D>();
		
		public Shader ShaderToUse;

		public List<string> textureNames = new List<string>();
		public List<Texture2D> texturesToUse = new List<Texture2D>();
		public GameObject obj, obj2;

		public void setValues(Object fg)
		{	
			FrameGen temp = (FrameGen)fg;
			rotate = temp.rotate;
			divides = temp.divides;
			bevel = temp.bevel;
			bevelamt = temp.bevelamt;
			
			border = temp.border;
			tile = temp.tile;
			
			ShaderToUseback = temp.ShaderToUseback;
			
			textureNamesback = temp.textureNamesback;
			texturesToUseback = temp.texturesToUseback;
			
			ShaderToUse = temp.ShaderToUse;
			
			textureNames = temp.textureNames;
			texturesToUse = temp.texturesToUse;
			return;
		}
	}
}