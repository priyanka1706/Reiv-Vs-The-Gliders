using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LivesCounterUI : MonoBehaviour {
    private Text livesText;

    void Awake() {
        livesText = GetComponent<Text>();
    }

    void Update() {
        livesText.text = "Lives: " + GameMaster.RemainingLives.ToString();
    }
}