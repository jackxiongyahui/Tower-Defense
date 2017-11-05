using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 敌人类
/// </summary>
public class Enemy : MonoBehaviour {

    public float speed = 10f;
    public float hp = 150;  //敌人默认的血量
    public float totalHp;   //总的血量
    public GameObject explosionEffect;  //爆炸效果
    public Slider hpSlider; //血量条的UI
    private Transform[] positions;
    private int index = 0;

	// Use this for initialization
	void Start () {
        positions = Waypoints.positions;
        totalHp = hp;
	}
	
	// Update is called once per frame
	void Update () {
        Move();
	}

    void Move()
    {
        if(index>positions.Length-1)
        {
            return;
        }
        transform.Translate((positions[index].position - transform.position).normalized * Time.deltaTime * speed);
        if(Vector3.Distance(positions[index].position,transform.position)<0.2)
        {
            index++;
        }
        if(index>positions.Length-1)
        {
            ReachDestination();
        }
    }

    //到达终点
    void ReachDestination()
    {
        GameManager.Instance.Failed();
        GameObject.Destroy(this.gameObject);
        
    }

    void OnDestroy()
    {
        EnemySpawner.CountEnemyAlive--;
    }

    public void TakeDamage(float damage)
    {
        if(hp<0)
        {
            return;
        }
        hp -= damage;
        hpSlider.value = (float)hp / totalHp;
        //如果敌人的血量小于等于0，则销毁自身
        if(hp<=0)  
        {
            Die();
        }
    }

    //销毁自身
    public void Die()
    {
        GameObject  effect = GameObject.Instantiate(explosionEffect, transform.position, transform.rotation); //得到敌人爆炸的特效
        Destroy(effect, 1.5f); //延时1.5f后销毁
        Destroy(this.gameObject); //销毁自身
    }
}
