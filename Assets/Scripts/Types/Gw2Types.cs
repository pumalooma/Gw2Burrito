
using System.Collections.Generic;
using UnityEngine;

public class Gw2Identity {
	public int map_id;
    //public int world_id;
    public float fov;
}

/*
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
public struct GW2Context {
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 28)]
    public readonly byte[] ServerAddress;
    public readonly uint MapId;
    public readonly uint MapType;
    public readonly uint ShardId;
    public readonly uint Instance;
    public readonly uint BuildId;
}
*/

public class Gw2World {
    public int id;
    public string name;
}

public class Gw2Objective {
    public string id;
    public string name;
    public int sector_id;
    public string type;
    public string map_type;
    public int map_id;
    public float[] coord;
}
public class Gw2MatchObjective {
    public string id;
    public string type;
    public string owner;
    public string last_flipped;

    public Color GetColor() {
        if (owner == "Red")
            return Color.red;
        else if (owner == "Green")
            return Color.green;
        else if (owner == "Blue")
            return Color.blue;

        return Color.white;
    }
}

public class Gw2MatchMap {
    public int id;
    public string type;
    public List<Gw2MatchObjective> objectives;
}

public class Gw2Match {
    public string id;
    public List<Gw2MatchMap> maps;
}

public class Gw2Map {
    public int id;
    public string name;
    public string type;
    public float[][] map_rect;
    public float[][] continent_rect;
}