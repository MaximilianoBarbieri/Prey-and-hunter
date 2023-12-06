using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI whiteEnemyState, redEnemyState, blueEnemyState, yellowEnemyState;
    [SerializeField] private Enemy _enemyWhite, _enemyRed, _enemyBlue, _enemyYellow;
    void Start()
    {
        whiteEnemyState.text = "State";
        redEnemyState.text = "State";
        blueEnemyState.text = "State";
        yellowEnemyState.text = "State";
    }

    // Update is called once per frame
    void Update()
    {
        whiteEnemyState.text = _enemyWhite.State;
        redEnemyState.text = _enemyRed.State;
        blueEnemyState.text = _enemyBlue.State;
        yellowEnemyState.text = _enemyYellow.State;
    }
}
