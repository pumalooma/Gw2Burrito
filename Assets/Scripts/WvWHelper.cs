using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WvWHelper : MonoBehaviour {

    public bool isInWvwMap;
    public Sprite[] sprites;
    public float worldScale = 1.0f;
    public Vector3 worldOffset;
    public float heightScale = 0.1f;
    public float heightOffset = 0.0f;
    public Dropdown serverListDropDown;
    
    public float distanceMultiplier = 0.005f;
    public float distanceAdd = 0.001f;
    public float minDist = 50.0f;
    public float maxDist = 1000.0f;
    public float scaleAtMinDistance = 1.0f;
    public float scaleAtMaxDistance = 0.5f;
    

    private string mWorldPath = "worlds.json";
    private string mObjectivesPath = "objectives.json";
    private string mMatchesPath = "matches.json";
    private string mMapsPath = "maps.json";
    private List<Gw2Objective> mObjectivesList;
    private Dictionary<string,Gw2Objective> mObjectiveMap;
    private Dictionary<int,Gw2Map> mMapTable = new Dictionary<int, Gw2Map>();
    private Gw2Match mMatch;
    private int mMapId = 0;
    private Coroutine mainLoop;
    private bool fullyLoaded = false;

	public static List<Gw2World> mWorlds;
	private static List<int> mValidMapIds = new List<int>() { 38, 95, 96, 1099 };

    void Start() {
        ArrayExpander.Expand(transform, 0);
    }
    
    public void Reload (int mapId) {

        if (mapId == 1237)
            mapId = 1099;

        mMapId = mapId;
        Debug.Log("Map Loaded: " + mapId);

        isInWvwMap = mValidMapIds.Contains(mapId);
        
        if (isInWvwMap) {
            if (mainLoop == null)
                mainLoop = StartCoroutine(MainLoop());
        }
        else { 
            Debug.Log("Clearing any wvw data...");
            ArrayExpander.Expand(transform, 0);
        }
    }

    private IEnumerator MainLoop () {

        Debug.Log("Initializing wvw data...");

        string url = "https://api.guildwars2.com/v2/worlds?ids=all";
        yield return StartCoroutine(RestCache.Get(mWorldPath, url));
        mWorlds = JsonConvert.DeserializeObject<List<Gw2World>>(RestCache.mJsonData);
		mWorlds.Sort((x, y) => x.name.CompareTo(y.name));

        Debug.Log("Servers: " + mWorlds.Count);

		RefreshDropDown();
		
        url = "https://api.guildwars2.com/v2/wvw/objectives?ids=all";
        yield return StartCoroutine(RestCache.Get(mObjectivesPath, url));
        mObjectivesList = JsonConvert.DeserializeObject<List<Gw2Objective>>(RestCache.mJsonData);
        

        mObjectiveMap = new Dictionary<string, Gw2Objective>();
        Debug.Log("Objectives: " + mObjectivesList.Count);
        foreach (var objective in mObjectivesList)
            mObjectiveMap[objective.id] = objective;

        
        url = "https://api.guildwars2.com/v2/maps?ids=38,95,96,1099";
        yield return StartCoroutine(RestCache.Get(mMapsPath, url));
        var maps = JsonConvert.DeserializeObject<List<Gw2Map>>(RestCache.mJsonData);
        foreach( var map in maps)
            mMapTable[map.id] = map;

        Debug.Log("Maps: " + mMapTable.Count);
        
        url = "https://api.guildwars2.com/v2/wvw/matches?world=" + Config.Instance.worldId;
        yield return StartCoroutine(RestCache.Get(mMatchesPath, url));
        mMatch = JsonConvert.DeserializeObject<Gw2Match>(RestCache.mJsonData);
        fullyLoaded = !RestCache.mCached;

        if(RestCache.mCached)
            StartCoroutine(UpdateMatch());

        int limiter = 0;

        while (isInWvwMap && isActiveAndEnabled) {

            if (++limiter > 5) {
                StartCoroutine(UpdateMatch());
                limiter = 0;
            }

            Gw2MatchMap mapItem = mMatch.maps.Find(x => x.id == mMapId);
            if (mapItem != null)
                RefreshMap(mapItem);

            yield return new WaitForSeconds(1.0f);
        }

        mainLoop = null;
    }

    private IEnumerator UpdateMatch () {
        WWW www = new WWW("https://api.guildwars2.com/v2/wvw/matches?world=" + Config.Instance.worldId);
        yield return www;

        if (www.error != null) {
            Debug.Log("WWW Error: " + www.error);
            yield break;
        }
        
        mMatch = JsonConvert.DeserializeObject<Gw2Match>(www.text);
        fullyLoaded = true;
    }

    private void RefreshMap (Gw2MatchMap mapItem) {
        int index =0;

		SetAppIconColor();

        float[][] cr = mMapTable[mapItem.id].continent_rect;

        var rect = new Rect(cr[0][0], cr[0][1], cr[1][0] - cr[0][0], cr[1][1] - cr[0][1]);
        
        foreach (var objective in mapItem.objectives) {

            var objectiveItem = mObjectiveMap[objective.id];

            if (objectiveItem == null || objectiveItem.coord == null || objectiveItem.coord.Length < 3) {
                //Debug.Log("Couldn't find objective lookup for id: " + objective.id);
                continue;
            }

            index++;
        }

        ArrayExpander.Expand(transform, index);
        index = 0;

        foreach (var objective in mapItem.objectives) {

            var objectiveItem = mObjectiveMap[objective.id];

            if (objectiveItem == null || objectiveItem.coord == null || objectiveItem.coord.Length < 3) {
                //Debug.Log("Couldn't find objective lookup for id: " + objective.id);
                continue;
            }

            var wvwItem = transform.GetChild(index).GetComponent<WvWItem>();

            float[] eventcoord = objectiveItem.coord;
            
            float x = (eventcoord[0] - rect.center.x) * worldScale;
            float z = (rect.center.y - eventcoord[1]) * worldScale;

            float y = eventcoord[2] * heightScale + heightOffset;

            //Debug.Log(objectiveItem.name + "  " + x + "   " + z);

            wvwItem.transform.position = new Vector3(x, y, z);
            wvwItem.image.sprite = GetSprite(objectiveItem.type);
            
            wvwItem.image.color = fullyLoaded ? objective.GetColor() : Color.white;
            wvwItem.title.text = objectiveItem.name;
            wvwItem.title.color = wvwItem.image.color;
            wvwItem.gameObject.name = objectiveItem.name;
            wvwItem.gameObject.SetActive(true);

            string ri = fullyLoaded ? GetRI(objective.last_flipped) : null;
            wvwItem.riLabel.gameObject.SetActive(ri != null);
            if (ri != null)
                wvwItem.riLabel.text = ri;

            index++;
        }
        
    }

    private Sprite GetSprite(string type) {

        if (type == "Camp")
            return sprites[1];
        else if (type == "Tower")
            return sprites[2];
        else if (type == "Keep")
            return sprites[3];
        else if (type == "Castle")
            return sprites[4];

        return sprites[0];
    }


    private const double riLength = 60.0 * 5.0;

    private string GetRI(string dateStr) {

        if (string.IsNullOrEmpty(dateStr))
            return null;

        DateTime date;
        if (!DateTime.TryParse(dateStr, out date))
            return null;

        TimeSpan timeSpan = DateTime.Now - date;

        if (timeSpan.TotalSeconds >= riLength)
            return null;
        
        timeSpan = new TimeSpan(0, 0, (int)(riLength - timeSpan.TotalSeconds));

        return string.Format("{0}:{1} RI", timeSpan.Minutes, timeSpan.Seconds);
    }

	public void RefreshDropDown() {

		serverListDropDown.options.Clear();

		string burritoState = Burrito.burritoEnabled ? "Disable Overlay" : "Enable Overlay";

		serverListDropDown.options.Add(new Dropdown.OptionData() { text = burritoState });
		serverListDropDown.options.Add(new Dropdown.OptionData() { text = "" });
		serverListDropDown.options.Add(new Dropdown.OptionData() { text = "Exit Burrito" });
		serverListDropDown.options.Add(new Dropdown.OptionData() { text = "" });
		
		int index = 4;
        foreach(var world in mWorlds)
		{
			serverListDropDown.options.Add(new Dropdown.OptionData() { text = world.name });

			if(world.id == Config.Instance.worldId)
				serverListDropDown.value = index;

			index++;
		}
	}

	private void SetAppIconColor() {

		Image icon = Burrito.instance.dropDown.GetComponent<Image>();
		
		for(int ii = 0; ii < mMatch.all_worlds.red.Length; ++ii ) {
			if(mMatch.all_worlds.red[ii] == Config.Instance.worldId) {
				icon.color = Color.red;
				return;
			}
        }

		for(int ii = 0; ii < mMatch.all_worlds.green.Length; ++ii)
		{
			if(mMatch.all_worlds.green[ii] == Config.Instance.worldId)
			{
				icon.color = Color.green;
				return;
			}
		}

		for(int ii = 0; ii < mMatch.all_worlds.blue.Length; ++ii)
		{
			if(mMatch.all_worlds.blue[ii] == Config.Instance.worldId)
			{
				icon.color = Color.blue;
				return;
			}
		}

		icon.color = Color.white;
	}
}
