using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("OverlayData")]
public class TacoOverlayData {
    public TacoMarkerCategory MarkerCategory;
    [XmlElement("POIs")]
    public TacoPOIs RouteData;
}

public class TacoMarkerCategory {
    [XmlAttribute]
    public string name;
    [XmlAttribute]
    public string DisplayName;
    [XmlAttribute]
    public string iconFile;
    [XmlAttribute]
    public float iconSize;
    [XmlAttribute]
    public float heightOffset;
    [XmlAttribute]
    public int behavior;
    [XmlAttribute]
    public float fadeNear;
    [XmlAttribute]
    public float fadeFar;

    [XmlElement("MarkerCategory")]
    public List<TacoMarkerCategory> ChildrenCategories;
}


public class TacoPOIs {
    [XmlElement("Route")]
    public List<TacoRoute> Routes;

    [XmlElement("POI")]
    public List<TacoPOI> POIs;
}

public class TacoRoute {
    [XmlAttribute]
    public string Name;
    [XmlAttribute]
    public int BackwardDirection;

    [XmlElement("POI")]
    public List<TacoPOI> POIs;
}

public class TacoPOI {
    [XmlAttribute]
    public int MapID;
    [XmlAttribute]
    public float xpos;
    [XmlAttribute]
    public float ypos;
    [XmlAttribute]
    public float zpos;
    [XmlAttribute]
    public string type;
    [XmlAttribute]
    public string GUID;
}