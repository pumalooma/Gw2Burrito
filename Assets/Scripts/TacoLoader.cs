using System;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class TacoLoader : MonoBehaviour {
    
    private int mMapId = 0;
    private List<Vector3> mPointList = new List<Vector3>();

    void Awake () {
        ArrayExpander.Expand(transform, 0);
    }

    public void Reload(int mapId) {
        mPointList.Clear();
        mMapId = mapId;

#if UNITY_EDITOR
        LoadFile("SAB_Dig_Locations.xml");
#else
        string[] files = Environment.GetCommandLineArgs();
        for (int ii = 1; ii < files.Length; ++ii)
            if (File.Exists(files[ii]))
                LoadFile(files[ii]);
#endif

        ArrayExpander.Expand(transform, mPointList.Count);

        for (int ii = 0; ii < mPointList.Count; ++ii) {
            Transform t = transform.GetChild(ii);
            t.position = mPointList[ii];
            t.gameObject.SetActive(true);
        }

        Debug.Log("Loading " + mPointList.Count + " taco POIs.");
    }

    private void LoadFile(string path) {
        Debug.Log("Loading taco file: " + path);
        XmlSerializer ser = new XmlSerializer(typeof(TacoOverlayData));
        XmlReader reader = XmlReader.Create(path);
        var tacoData = (TacoOverlayData)ser.Deserialize(reader);
        reader.Close();
        
        GetRoutePoints(tacoData.RouteData);
    }

    private void GetRoutePoints(TacoPOIs routeData) {

        if (routeData == null)
            return;

        GetPointsOfInterest(routeData.POIs);

        if (routeData.Routes != null)
            foreach (var route in routeData.Routes)
                GetPointsOfInterest(route.POIs);
    }

    private void GetPointsOfInterest (List<TacoPOI> poiList) {
        foreach (var poi in poiList)
            if (poi.MapID == 0 || poi.MapID == mMapId)
                mPointList.Add(new Vector3(poi.xpos, poi.ypos, poi.zpos));
    }
}
