using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SphericalFog : MonoBehaviour
{
	protected MeshRenderer sphericalFogObject;
	public Material sphericalFogMaterial;
    public float maxScaleFactor = 1.2f, minScaleFactor = 0.8f, speed = 10, lifeTime = 5;
    float scaleFactor, scaleSpeed = 0.005f;
    int scaleDirection = 1;

	void OnEnable ()
	{
        scaleFactor = minScaleFactor;

		sphericalFogObject = gameObject.GetComponent<MeshRenderer>();
		if (sphericalFogObject == null)
			Debug.LogError("Volume Fog Object must have a MeshRenderer Component!");
		
		if (Camera.main.depthTextureMode == DepthTextureMode.None)
			Camera.main.depthTextureMode = DepthTextureMode.Depth;
		
		sphericalFogObject.material = sphericalFogMaterial;
	}

	void Update ()
	{
		float radius = (transform.lossyScale.x + transform.lossyScale.y + transform.lossyScale.z) / 6;
		Material mat = Application.isPlaying ? sphericalFogObject.material : sphericalFogObject.sharedMaterial;
		if (mat) mat.SetVector ("FogParam", new Vector4(transform.position.x, transform.position.y, transform.position.z, radius * scaleFactor));

        scaleFactor += scaleSpeed * scaleDirection;

        if (scaleFactor > maxScaleFactor || scaleFactor < minScaleFactor) scaleDirection *= -1;

        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0) Destroy(gameObject);

        transform.Translate(Vector3.up * speed * Time.deltaTime);
	}
}
