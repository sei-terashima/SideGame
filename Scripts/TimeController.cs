using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public bool isCountDown = true; //カウントダウンにするかのフラグ※フラグなければカウントアップ
    public float gameTime = 0; //ゲームの最大時間
    public bool isTimeOver = false; //trueになると計測を停止
    public float displayTime = 0; //ユーザーへのアナウンス用

    float times = 0; //ゲーム開始してから現在までの時間

    // Start is called before the first frame update
    void Start()
    {
        //カウントダウンのフラグがついていたら
        if(isCountDown)
        {
            //ユーザーへのアナウンスはまずはgameTimeと同じ
            displayTime = gameTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //計測停止フラグがfalseなら
        if(isTimeOver == false)
        {
            times += Time.deltaTime; //時間の蓄積＝ゲーム開始してからの経過時間

            //もしカウントダウンのフラグがあれば
            if (isCountDown)
            {
                //カウントダウンの仕組み

                //アナウンス用の時間は
                //最大時間 - ゲームの経過時間
                displayTime = gameTime - times;

                //0以下になるなら
                if(displayTime <= 0.0f)
                {
                    displayTime = 0.0f; //表示は0で止める
                    isTimeOver = true; //計測停止フラグをON
                }
            }
            //カウントダウンのフラグがない
            else
            {
                //カウントアップの仕組み

                //アナウンス用の時間は
                //ゲームの経過時間そのもの
                displayTime = times; 

                //最大時間以上になるなら
                if(displayTime >= gameTime)
                {
                    displayTime = gameTime;//表示は最大時間で止める
                    isTimeOver = true;//計測停止フラグをON
                }
            }
            //Debug.Log("TIMES:" + displayTime);
        }
    }
}
