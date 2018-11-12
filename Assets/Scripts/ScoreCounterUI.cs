using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ScoreCounterUI : MonoBehaviour {
    private Text scoreText;

    void Awake() {
        scoreText = GetComponent<Text>();
    }

    void Update() {
        scoreText.text = "Score: " + GameMaster.CurrentScore.ToString();
    }
}