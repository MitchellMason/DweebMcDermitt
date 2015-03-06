using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConstructiveSolidGeometry;

namespace LevelEditor
{
	public class PolyMesh : MonoBehaviour
	{
		public Shader ShaderToUse;
		
		public List<string> textureNames = new List<string>();
		public List<Texture2D> texturesToUse = new List<Texture2D>();

		public List<Vector3> keyPoints = new List<Vector3>();
		public float height = 3;
		public float floor = 0;
		public float zIndex = 0;
		public MeshCollider meshCollider;
		public float colliderDepth = 1;
		public bool buildColliderEdges = true;
		public bool buildColliderFront;
		public Vector2 uvPosition;
		public float uvScale = 1;
		public int matindex = 0;
		public float uvRotation;
		
		static public void CreatePolyMesh(GameObject parent)
		{
			var obj = new GameObject("PolyMesh");
			obj.transform.parent = parent.transform;
			obj.transform.localPosition = new Vector3 (0,0,0);
			obj.transform.localRotation = Quaternion.identity;
			var polyMesh = obj.AddComponent<PolyMesh>();
			polyMesh.CreateSquare (0.5f);
		}

		public SubMaterial getSubMat()
		{
			if (ShaderToUse == null)
			{
				ShaderToUse = Shader.Find("Bumped Diffuse");
				Material mat = new Material(ShaderToUse);
				textureNames = MeshUtils.getTextures(mat);
				texturesToUse = new List<Texture2D>();
				for (int i = 0; i < textureNames.Count; ++i)
				{
					texturesToUse.Add(new Texture2D(1,1));
					texturesToUse[i].name = "Click to set " + textureNames[i];
					//buttontext.Add ("Select a texture and click here to set " + options.textureNames[i]);
				}
			}
			return new SubMaterial (ShaderToUse, textureNames, texturesToUse);
		}
		public void CreateSquare(float size)
		{
			keyPoints.AddRange(new Vector3[] { new Vector3(size, size), new Vector3(size, -size), new Vector3(-size, -size), new Vector3(-size, size)} );
			
			BuildFinishedMesh();
		}
		public List<Vector3> GetEdgePoints()
		{
			//Build the point list and calculate curves
			var points = new List<Vector3>();
			for (int i = 0; i < keyPoints.Count; i++)
			{
				points.Add(transform.parent.InverseTransformPoint(transform.TransformPoint(keyPoints[i])));
			}
			return points;
		}


		public SubMesh getSub()
		{
			
			var points = GetEdgePoints();
			SubMesh sub = PolyUtils.makeSub (points, height, floor);
			for (int i = 0; i < sub.verts.Count; ++i)
			{
				sub.zones.Add(matindex);
			}
			return sub;
		}

		
		public void BuildFinishedMesh()
		{

			GameObject parent = transform.parent.gameObject;
			if (parent.GetComponents<PolyTree>().Length > 0)
			{
				parent.GetComponent<PolyTree>().BuildFinishedMesh();
			}

		}
		void UpdateCollider(List<Vector3> points, List<int> tris)
		{
			if (meshCollider == null)
			{
				meshCollider = gameObject.AddComponent<MeshCollider>();
			}
			Mesh collider = meshCollider.sharedMesh;
			if (collider == null)
			{
				collider = new Mesh();
				collider.name = "PolySprite_Collider";
			}
			collider.vertices = points.ToArray ();
			collider.triangles = tris.ToArray ();
			collider.RecalculateBounds();
			collider.RecalculateNormals();
			collider.Optimize();
			meshCollider.sharedMesh = null;
			meshCollider.sharedMesh = collider;


		}

	}

}