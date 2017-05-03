using System;
using UnityEngine;

public class Burrito : MonoBehaviour
{
    public int currentMapId;
    public WvWHelper wvwHelper;
    public TacoLoader taco;

    private bool burritoEnabled = true;
    
    private void Update()
    {
        var identity = Mumble.mIdentity;
        int mapId = identity != null ? identity.map_id : 0;
        
        if(mapId != currentMapId) {
            Debug.Log("Switched to mapId: " + mapId);
            wvwHelper.Reload(mapId);
            taco.Reload(mapId);

            currentMapId = mapId;
        }


        // alt shift
        int keystate = (int)Win32.GetAsyncKeyState((int)VirtualKeyStates.VK_ADD) & 1;
        if (keystate == 1) {
            Debug.Log("Toggling UI.");
            burritoEnabled = !burritoEnabled;

            wvwHelper.Reload(burritoEnabled ? mapId : 0);
            taco.Reload(burritoEnabled ? mapId : 0);
        }
    }
}
