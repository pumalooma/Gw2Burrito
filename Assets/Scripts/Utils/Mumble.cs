//using MumbleLink_CSharp_GW2;
using MumbleLink_CSharp;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

public class Mumble : MonoBehaviour
{
	private MumbleLink link;

	public static MumbleLinkedMemory mem;
	public static Gw2Identity mIdentity;

    private char[] mIdentityCache;

	private void Awake()
    {
        link = new MumbleLink();
	}

	void Update() {
		mem = link.Read();
        
        /*int size = Marshal.SizeOf(typeof(GW2Context));
        IntPtr ptr = Marshal.AllocHGlobal(size);
        Marshal.Copy(mem.Context, 0, ptr, size);
        var context = (GW2Context)Marshal.PtrToStructure(ptr, typeof(GW2Context));
        Marshal.FreeHGlobal(ptr);*/

		if(mem.Identity != null) {

            bool identityChanged = mIdentityCache == null || mIdentityCache.Length != mem.Identity.Length;

			int length = 0;
            for (; length < mem.Identity.Length; length++) {
                
                if (mem.Identity[length] == 0)
                    break;

                if (!identityChanged && mIdentityCache[length] != mem.Identity[length])
                    identityChanged = true;
            }

            if (identityChanged) {
                var identityStr = new string(mem.Identity, 0, length);
                mIdentity = JsonConvert.DeserializeObject<Gw2Identity>(identityStr);
                mIdentityCache = mem.Identity;
            }
        }	
		else {
			mIdentity = null;
            mIdentityCache = null;
        }

	}

    private void OnDestroy()
    {
		link.Dispose();
    }
}
