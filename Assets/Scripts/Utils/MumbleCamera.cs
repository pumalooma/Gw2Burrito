using MumbleLink_CSharp;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(Mumble))]
public class MumbleCamera : MonoBehaviour
{
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        var identity = Mumble.mIdentity;
        if (identity != null) {
            cam.fieldOfView = identity.fov * Mathf.Rad2Deg;

			float x = Mumble.mem.FCameraPosition[0];
			float y = Mumble.mem.FCameraPosition[1];
			float z = Mumble.mem.FCameraPosition[2];

			transform.position = new Vector3(x, y, z);
            transform.rotation = Quaternion.LookRotation(new Vector3(Mumble.mem.FCameraFront[0], Mumble.mem.FCameraFront[1], Mumble.mem.FCameraFront[2]));
        }
    }
}
