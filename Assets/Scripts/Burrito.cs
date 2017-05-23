using UnityEngine;
using UnityEngine.UI;

public class Burrito : MonoBehaviour
{
    public int currentMapId;
    public WvWHelper wvwHelper;
    public TacoLoader taco;
	public Dropdown dropDown;

    public static bool burritoEnabled = true;
    
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
		
        int keystate = (int)Win32.GetAsyncKeyState((int)VirtualKeyStates.VK_ADD) & 1;
        if (keystate == 1) {
			ToggleOverlay();
        }
    }

	private void ToggleOverlay() {
		Debug.Log("Toggling UI.");
		burritoEnabled = !burritoEnabled;

		wvwHelper.Reload(burritoEnabled ? currentMapId : 0);
		taco.Reload(burritoEnabled ? currentMapId : 0);
	}

	public void PointerEnter(bool enter) {
		Debug.Log(enter ? "Pointer Enter" : "Pointer Exit");

#if !UNITY_EDITOR
		Win32.AllowClicking(WindowTracker.hwndApp, enter);
#endif
	}


	public void DropDownClicked(Dropdown dd) {
		if(dd.value == 0)
		{
			ToggleOverlay();
			wvwHelper.RefreshDropDown();
			return;
		}
		else if(dd.value == 1 || dd.value == 3)
			return;
		else if(dd.value == 2) {
			Application.Quit();
		}
		else if(dd.value >= 4) {

			var world = WvWHelper.mWorlds[dd.value - 4];

			Debug.Log(string.Format("Selected {0} ({1})", world.name, world.id));

			Config.Instance.worldId = world.id;
			Config.Instance.SaveConfig();
		}
	}
}
