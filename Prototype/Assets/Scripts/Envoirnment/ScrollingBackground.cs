using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour {

    [SerializeField]
    private float speed = 1.0f;
    public float Speed { get { return speed; } set { speed = value; } }

    Vector2 savedOffset;
    Renderer meshRenderer;

    public bool infiniteScroll = true;
    /// <summary>
    /// This is the 
    /// </summary>
    public Transform Follow;
    [Tooltip("The left most x and right x of the entire world.")]
    public Vector2 XBounds;
    [Tooltip("The left most y and right y of the entire world.")]
    public Vector2 YBounds;

    /// <summary>
    /// Refrence to the Orthographic Size of Camera.
    /// </summary>
    const int CAMERA_SIZE = 12;

    // Use this for initialization
    void Start ()
    {
        meshRenderer = GetComponent<Renderer>();
        savedOffset = meshRenderer.sharedMaterial.GetTextureOffset("_MainTex");
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(!Follow) {
            return;
        }

        if (Follow.position.x > XBounds.x && Follow.position.x < XBounds.y)
        {
            float inf = 1;
            //Constantlly move the object in the left direction.
            if (infiniteScroll)
            {
                inf = Mathf.Repeat(Time.time * Speed / 30, 1);
            }

            // Repeat X Direction.
            var totalScreenWidth = XBounds.y - XBounds.x;
            var widthOfLoop = totalScreenWidth / speed;
            float x = Mathf.Repeat((Follow.position.x % widthOfLoop) / widthOfLoop + inf, 1);
             
            // Move Y.
            var totalScreenHeight = YBounds.y - YBounds.x;
            var maxLoops = 1.3f;
            var maxSpeed = 4;
            var heightOfLoop = totalScreenHeight / (Speed / maxSpeed * maxLoops);
            var yOffset = (Follow.position.y + 3 % heightOfLoop) / heightOfLoop;

            // Touching Floor.
            if(Follow.position.y - (CAMERA_SIZE + 0.5f) <= YBounds.x) {
                yOffset = 0;
            }

            Vector2 offset = new Vector2(x, yOffset);
            meshRenderer.sharedMaterial.SetTextureOffset("_MainTex", offset);
        }
    }

    void OnDisable()
    {
        meshRenderer.sharedMaterial.SetTextureOffset("_MainTex", savedOffset);
    }
}
