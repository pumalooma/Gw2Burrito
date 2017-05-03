using System.Collections;
using System.IO;
using UnityEngine;

public static class RestCache {

    public static string mJsonData;
    public static bool mCached;
    public static IEnumerator Get(string path, string url) {

        if (File.Exists(path)) {
            mCached = true;
            mJsonData = File.ReadAllText(path);
            Debug.Log("Loading cached " + path);
        }
        else {
            mCached = false;
            Debug.Log("Fetching " + url);
            WWW www = new WWW(url);
            yield return www;

            if (www.error != null) {
                Debug.Log("WWW Error: " + www.error);
                yield break;
            }

            mJsonData = www.text;
            File.WriteAllText(path, mJsonData);
        }
    }
}
