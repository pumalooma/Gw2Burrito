using UnityEngine;

public class CustomScale : MonoBehaviour
{
    private WvWHelper wvw;
    void Start() {
        wvw = transform.parent.GetComponent<WvWHelper>();
    }

    void Update()
    {
        Vector3 vec_to_target = transform.position - Camera.main.transform.position;

        float distance = vec_to_target.magnitude;

        // We want the transform to scale with distance, so it doesn't shrink when the player gets far away.
        float customScale = distance * wvw.distanceMultiplier + wvw.distanceAdd; // these magic numbers seem to work well. 

        // now our custom scale stuff
        float clampedDistance = Mathf.Clamp(distance, wvw.minDist, wvw.maxDist);
        float clampedRange = (clampedDistance - wvw.minDist) / (wvw.maxDist - wvw.minDist);
        float clampedScale = Mathf.Lerp(wvw.scaleAtMinDistance, wvw.scaleAtMaxDistance, clampedRange);

        customScale *= clampedScale;

        transform.localScale = new Vector3(customScale, customScale, customScale);

       
        GetComponent<WvWItem>().image.gameObject.SetActive(distance > 75.0f);
        GetComponent<WvWItem>().distanceLabel.text = string.Format("{0}m", (int)distance);
    }
}