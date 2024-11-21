using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GimmickBlock : MonoBehaviour
{
    public float length = 0.0f; //自動落下検知距離
    public bool isDelete = false; //落下後に削除するかどうか
    public GameObject deadObj; //死亡当たり

    bool isFell = false; //落下フラグ
    float fadeTime = 0.5f; //フェードアウト時間

    // Start is called before the first frame update
    void Start()
    {
        //最初から落ちないようにRigidbody2Dの挙動を停止
        Rigidbody2D rbody = GetComponent<Rigidbody2D>();
        rbody.bodyType = RigidbodyType2D.Static;
        deadObj.SetActive(false); //死亡当たり判定を無効にする

    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤー情報を探して変数playerに代入
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        //プレイヤーがいれば
        if ((player != null))
        {
            //プレイヤーとの距離計測
            float d = Vector2.Distance(transform.position, player.transform.position);

            //設定した距離より、プレイヤーとブロックの差が小さければ
            if (length >= d)
            {
                Rigidbody2D rbody = GetComponent<Rigidbody2D>();

                //Rigidbody2Dの動きが止まっていれば
                if (rbody.bodyType == RigidbodyType2D.Static)
                {
                    //Rigibody2Dの挙動を再開
                    rbody.bodyType = RigidbodyType2D.Dynamic;
                    deadObj.SetActive(true); //死亡当たり判定を復活
                }
            }
        }

        //落下した
        if (isFell)
        {
            //透明値を変更してフェードアウト
            fadeTime -= Time.deltaTime; //フレーム間処理時間にあわせてマイナスしていく
            Color col = GetComponent<SpriteRenderer>().color; //現在のカラー値を一度変数colにコピー
            col.a = fadeTime; //フレームごとに減衰していく値を変数colのα値に与える
            GetComponent<SpriteRenderer>().color = col; //加工した変数colのカラー値をオブジェクトに反映
            if (fadeTime <= 0.0f)
            {
                //α値が0以下になる用ならオブジェクトごと消す
                Destroy(gameObject);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDelete)
        {
            isFell = true; //落下フラグをON
        }  
    }

    //編集画面上のガイドとして、設定距離を視覚化
    private void OnDrawGizmosSelected()
    {
        //ブロックの中心から、lengthと同じ半径の円を描画
        Gizmos.DrawWireSphere(transform.position,length);
    }
}
