using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //UIを扱うために必要な名前空間
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject mainImage; //イラスト文字を持つGameObject
    public Sprite gameOverSpr; //GAMEOVER画像
    public Sprite gameClearSpr; //GAMECLEAR画像
    public GameObject panel; //パネル
    public GameObject restartButton; //RESTARTボタン
    public GameObject nextButton; //NEXTボタン

    Image titleImage; //イラスト文字を表示しているImageコンポーネント

    //+++時間制限追加+++
    public GameObject timeBar; //時間表示イメージ
    public GameObject timeText; //時間テキスト
    TimeController timeCnt; //TimeControllerスクリプト



    //+++スコア追加+++//
    public GameObject scoreText; //スコアテキストオブジェクト
    public static int totalScore; //合計スコア
    public int stageScore = 0; //そのステージ中に入手したスコア

    //+++サウンド再生追加+++
    public AudioClip meGameOver; //ゲームオーバー
    public AudioClip meGameClear; //ゲームクリア

    //+++プレイヤー操作+++
    public GameObject inputUI;

    // Start is called before the first frame update
    void Start()
    {
        //画像を非表示にする
        Invoke("InactiveImage", 1.0f);
        //ボタン（パネル）を非表示にする
        panel.SetActive(false);

        //+++時間制限追加+++
        //TimeControllerを取得
        timeCnt = GetComponent<TimeController>();
        if (timeCnt != null)
        {
            if(timeCnt.gameTime == 0.0f)
            {
                timeBar.SetActive(false); //制限時間なしなら隠す
            }
        }

        //+++スコア追加+++//
        UpdateScore();

    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerController.gameState == "gameclear")
        {
            //ゲームクリア
            mainImage.SetActive(true); //イラスト文字を表示
            panel.SetActive(true); //パネル（ボタン）を表示
            //RESTARTボタンの無効化
            Button bt = restartButton.GetComponent<Button>();
            bt.interactable = false; //Buttonコンポーネントのボタン有効化の変数をfalse
            mainImage.GetComponent<Image>().sprite = gameClearSpr; //GAMECLEARのイラスト文字に変更

            PlayerController.gameState = "gameend"; //何回もこの一連の処理を繰り返さないようにするため


            //+++時間制限追加+++
            if(timeCnt != null)
            {
                timeCnt.isTimeOver = true; //時間カウント停止

                //+++スコア追加+++
                //タイムボーナス：整数に代入することで小数を切り捨てる
                int time = (int)timeCnt.displayTime;
                totalScore += time + 10;//残り時間をスコアに加える
            }

            //+++スコア追加+++
            //ゲームクリアしたので、stageScoreはtotalScoreとして確定
            //※次に備えてstageScoreは0にしておく
            totalScore += stageScore;
            stageScore = 0;
            //UIに数字を反映
            UpdateScore();

            //+++サウンド再生追加+++
            AudioSource soundPlayer = GetComponent<AudioSource>();
            if (soundPlayer != null)
            {
                //BGM停止
                soundPlayer.Stop();
                soundPlayer.PlayOneShot(meGameClear);
            }

            //+++プレイヤー操作+++
            inputUI.SetActive(false); //タッチ操作UIを隠す

        }
        else if(PlayerController.gameState == "gameover")
        {
            //ゲームオーバー
            mainImage.SetActive(true); //イラスト文字を表示する
            panel.SetActive(true); //パネル（ボタン）を表示する
            //NEXTボタンを無効化する
            Button bt = nextButton.GetComponent<Button>();
            bt.interactable = false;//Buttonコンポーネントのボタン有効化の変数をfalse
            mainImage.GetComponent<Image>().sprite = gameOverSpr; //GAMECLEARのイラスト文字に変更

            PlayerController.gameState = "gameend"; //何回もこの一連の処理を繰り返さないようにするため

            //++時間制限追加+++
            if(timeCnt != null)
            {
                timeCnt.isTimeOver = true; //時間カウント停止
            }

            //+++サウンド再生追加+++
            AudioSource soundPlayer = GetComponent<AudioSource>();
            if (soundPlayer != null)
            {
                //BGM停止
                soundPlayer.Stop();
                soundPlayer.PlayOneShot(meGameOver);
            }

            //+++プレイヤー操作+++
            inputUI.SetActive(false); //タッチ操作UIを隠す

        }
        else if(PlayerController.gameState == "playing")
        {
            //ゲーム中
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            //PlayerControllerを取得する
            PlayerController playerCnt = player.GetComponent<PlayerController>();
            //+++時間制限追加+++
            //タイムを更新する
            if(timeCnt != null)
            {
                if(timeCnt.gameTime > 0.0f)
                {
                    //整数に代入することで小数を切り捨てる
                    int time = (int)timeCnt.displayTime;
                        //タイム更新
                        timeText.GetComponent<TextMeshProUGUI>().text = time.ToString();
                    //タイムオーバー
                    if(time == 0)
                    {
                        playerCnt.GameOver(); //ゲームオーバーにする
                    }
                }
            }

            //+++スコア追加+++
            //プレイヤーがアイテムと接触をして、プレイヤーのもつ変数scoreに変化がないか常に監視
            if(playerCnt.score != 0)
            {
                //変化があったらstageScore変数にすぐに加算
                stageScore += playerCnt.score;
                //加算が済んだらプレイヤーのもつ変数scoreは0に戻し、次の変化を伺う準備とする
                playerCnt.score = 0;
                //UIに反映させる
                UpdateScore();
            }
        }
    }

    void InactiveImage()
    {
        mainImage.SetActive(false);
    }

    //+++スコア追加+++

    //UIへの表示担当
    void UpdateScore()
    {
        //ステージ開始直後は 0点 + トータル点
        int score = stageScore + totalScore;
        scoreText.GetComponent<TextMeshProUGUI>().text = score.ToString();
    }

    //+++プレイヤー操作+++
    //GameManager経由のジャンプメソッドの制作
    public void Jump()
    {
        //プレイヤーオブジェクトを変数playerに取得
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        //プレイヤーについているPlayerController.csを変数playerCntに取得
        PlayerController playerCnt = player.GetComponent<PlayerController>();

        //PlayerControllerのJumpメソッドを発動
        playerCnt.Jump();
    }

}
