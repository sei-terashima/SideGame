using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //追加
using TMPro; //追加

public class ResultManager : MonoBehaviour
{
    //トータルスコアのオブジェクトを参照する変数
    public GameObject scoreText;

    // Start is called before the first frame update
    void Start()
    {
        //static変数であるtotalScore(GameManagerの所有物)を文字列に変換して、scoreTextのTMPコンポーネントのtextパラメータに代入
        scoreText.GetComponent<TextMeshProUGUI>().text = GameManager.totalScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
