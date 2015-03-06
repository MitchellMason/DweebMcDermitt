using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConstructiveSolidGeometry;

namespace LevelEditor
{
	public class SubMaterial
	{
		public Shader shader;
		public List<string> textureNames;
		public List<Texture2D> textures;
		public string shadname;
		public List<int> indices;
		public List<Vector2> uvs;
		public List<Vector3> verts;
		public float scalemaps;
		public Material mat;
		public SubMaterial(Shader shader, List<string> textureNames, List<Texture2D> textures)
		{
			this.shader = shader;
			this.textureNames = textureNames;
			this.textures = textures;
			scalemaps = 0.5f;
				shadname = shader.name;
			mat = new Material(shader);
			
			for (int i = 0; i < textureNames.Count; ++i)
			{
				if (textures[i].width > 1 || textures[i].height > 1)
				{
					mat.SetTexture(textureNames[i], textures[i]);
					mat.SetTextureScale(textureNames[i], new Vector2(scalemaps,scalemaps));
				}
			} 
		}

	}
}