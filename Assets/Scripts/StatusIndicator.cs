using UnityEngine.UI;
using UnityEngine;

public class StatusIndicator : MonoBehaviour {

    [SerializeField]
    private RectTransform healthBarRect;
    [SerializeField]
    private Text healthText;

    void Start ()
    {
        if(healthBarRect == null)
        {
            Debug.LogError("STAT IND: No health bar object referenced");
        }
        if(healthText == null)
        {
            Debug.LogError("STAT IND: No health text object referenced");
        }
    }

    public void SetHealth(int _cur,int _max) // underscore - pvt var, only in the scope of this fn
    {
        float _value = (float)_cur / _max;

        healthBarRect.localScale = new Vector3(_value, healthBarRect.localScale.y, healthBarRect.localScale.z);
        healthText.text = _cur + "/" + _max + " HP";

    }

}
