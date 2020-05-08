using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enviornment
{

    /// <summary>
    /// Parallax background allows you to create parallaxing by adding multiple images and determining the speed of each background.
    /// </summary>
    public class ParallaxBackground : MonoBehaviour
    {
        [HideInInspector]
        public readonly List<Texture> Backgrounds = new List<Texture>();
        [HideInInspector]
        public readonly List<float> Speeds = new List<float>();
        [HideInInspector]
        public readonly List<bool> ScrollInfinetly = new List<bool>();

        [Tooltip("This is the Material for the backgrounds that will be created.")]
        public Material BaseMaterial;

        [Tooltip("This is a GameObject that has a Collider2D. The Collider2D will determine the bounds for the backgrounds to loop. If you have a speed of 2 the background will loop twice inside the world bounds.")]
        public Collider2D BoundingShape2D;

        [Tooltip("Transform the scrolling backgrounds will use as center. Should be Default VM Camera if using Cinemachine.")]
        public Transform Follow;

        public void CreateBackgrounds()
        {
            if (Backgrounds.Count == 0 || Backgrounds.Exists(texture => texture == null))
            {
                Debug.LogError("You need to set the backgrounds to create.");
                return;
            }
            if(!BoundingShape2D)
            {
                Debug.LogError("There needs to be a bounding box for the world. Similar to Cinemachine.");
                return;
            }
            if (transform.childCount > 0)
            {
                Debug.Log("Destroying all children of Parallax Background.");
                var children = new Transform[transform.childCount];
                for (int i = 0; i < transform.childCount; ++i)
                {
                    children[i] = transform.GetChild(i);
                }
                foreach(Transform child in children) {
                    DestroyImmediate(child.gameObject);
                }
            }
            //Making all bgs a child of this.
            var parentTransform = transform;
            int startingZ = 10;
            for (int i = 0; i < Backgrounds.Count; ++i) {
                CreateBackground(i, startingZ);
                startingZ--;
            }
        }

        void CreateBackground(int curBG, int zPosition, Transform parentTransform = null)
        {
            GameObject obj = new GameObject(Backgrounds[curBG].name);
            obj.transform.SetParent(parentTransform ? parentTransform : transform);
            var tempTransform = obj.transform.position;
            tempTransform.z = zPosition;
            obj.transform.localPosition = tempTransform;
            obj.transform.localScale = new Vector3(1, 1, 1);

            MeshFilter meshFilter = obj.AddComponent<MeshFilter>();
            CreateQuad(meshFilter);

            MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();
            var material = new Material(BaseMaterial)
            {
                mainTexture = Backgrounds[curBG]
            };
            meshRenderer.material = material;

            ScrollingBackground scrollingBackground = obj.AddComponent<ScrollingBackground>();
            scrollingBackground.Speed = Speeds[curBG];
            scrollingBackground.infiniteScroll = ScrollInfinetly[curBG];
            scrollingBackground.Follow = Follow;

            scrollingBackground.XBounds.x = -BoundingShape2D.bounds.size.x / 2 + BoundingShape2D.transform.position.x;
            scrollingBackground.XBounds.y = BoundingShape2D.bounds.size.x / 2 + BoundingShape2D.transform.position.x;

            scrollingBackground.YBounds.x = -BoundingShape2D.bounds.size.y / 2 + BoundingShape2D.transform.position.y;
            scrollingBackground.YBounds.y = BoundingShape2D.bounds.size.y / 2 + BoundingShape2D.transform.position.y;


        }

        void CreateQuad(MeshFilter meshFilter)
        {
            var width = 1920 / 30;
            var height = 1080 / 30;

            var mesh = new Mesh();
            meshFilter.mesh = mesh;

            Vector3[] vertices = new Vector3[4];

            vertices[0] = new Vector3(-width / 2, -height / 2, 0);
            vertices[1] = new Vector3(width / 2, -height / 2, 0);
            vertices[2] = new Vector3(-width / 2, height / 2, 0);
            vertices[3] = new Vector3(width / 2, height/2, 0);

            mesh.vertices = vertices;

            int[] tri = new int[6];

            tri[0] = 0;
            tri[1] = 2;
            tri[2] = 1;

            tri[3] = 2;
            tri[4] = 3;
            tri[5] = 1;

            mesh.triangles = tri;

            Vector3[] normals = new Vector3[4];

            normals[0] = -Vector3.forward;
            normals[1] = -Vector3.forward;
            normals[2] = -Vector3.forward;
            normals[3] = -Vector3.forward;

            mesh.normals = normals;

            Vector2[] uv = new Vector2[4];

            uv[0] = new Vector2(0, 0);
            uv[1] = new Vector2(1, 0);
            uv[2] = new Vector2(0, 1);
            uv[3] = new Vector2(1, 1);

            mesh.uv = uv;
        }

    }
}
