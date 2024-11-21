using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchAction : MonoBehaviour
{
    //連動するMovingBlockを決める変数
    public GameObject targetMoveBlock;

    public Sprite imageOn; //スイッチが入っているイラスト
    public Sprite imageOff; //スイッチが切れているイラスト

    public bool on = false; //スイッチの状態

    // Start is called before the first frame update
    void Start()
    {
        //最初からスイッチがONの場合
        if(on)
        {
            //イラストもONにする
            GetComponent<SpriteRenderer>().sprite = imageOn;
        }
        else //最初からスイッチがOFFの場合
        {
            //イラストはOFFにする
            GetComponent<SpriteRenderer>().sprite = imageOff;

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //何かと接触した時
    void OnTriggerEnter2D(Collider2D col)
    {
        //相手がプレイヤーなら
        if(col.gameObject.tag == "Player")
        {
            //接触時、もともとスイッチがONだった場合
            if (on)
            {
                //スイッチの状態をOFFにする
                on = false;
                //イラストもオフにする
                GetComponent<SpriteRenderer>().sprite = imageOff;
                //対象のMovingBlockを止める
                MovingBlock movBlock = targetMoveBlock.GetComponent<MovingBlock>();
                movBlock.Stop();
            }
            else　//接触時、もともとスイッチがOFFだった場合
            {
                //スイッチの状態をONにする
                on = true;
                //イラストもオンにする
                GetComponent<SpriteRenderer>().sprite = imageOn;
                //対象のMovingBlockを動かす
                MovingBlock movBlock = targetMoveBlock.GetComponent<MovingBlock>();
                movBlock.Move();
            }
        }    
    }
}
