using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 炮台类，实现炮台的所有效果
/// </summary>
public class Turret : MonoBehaviour {

    //得到敌人的数列
    private List<GameObject> enemyes = new List<GameObject>();

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            enemyes.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            enemyes.Remove(other.gameObject);
        }
    }

    public float AttackRateTime = 1f; //多少秒攻击一次
    private float timer = 0f;   //计时器

    public GameObject bulletPrefab; //子弹的类型
    public Transform firePosition;  //子弹初始产生的位置
    public Transform head; //炮台的头部

    public bool userLaser = false;   //是否使用激光
    public float damageRate = 70f;  //激光的伤害

    public LineRenderer laserRenderer;
    public GameObject laserEffect; //对激光特效的引用

    void Start()
    {
        timer = AttackRateTime;
    }

    void Update()
    {
        //调整攻击的方向
        if (enemyes.Count > 0 && enemyes[0] != null)
        {
            Vector3 targetPosition = enemyes[0].transform.position;
            targetPosition.y = head.position.y;
            head.LookAt(targetPosition);
        }
        if(userLaser==false) 
        {
            timer += Time.deltaTime;
            //进行攻击
            if (enemyes.Count > 0 && timer >= AttackRateTime)
            {
                timer = 0; //计时器变为0
                Attack();  //攻击
            }
        }
        else if(enemyes.Count>0)  //当使用激光时
        {
            if(laserRenderer.enabled == false)
            {
                laserRenderer.enabled = true;
                laserEffect.SetActive(true);   //启用激光特效
            }
            //判断敌人是否为空
            if (enemyes[0] == null)
            {
                UpdateEnemy();
            }
            if (enemyes.Count > 0)
            {
                laserRenderer.SetPositions(new Vector3[] { firePosition.position, enemyes[0].transform.position });
                enemyes[0].GetComponent<Enemy>().TakeDamage(damageRate*Time.deltaTime);
                laserEffect.transform.position = enemyes[0].transform.position;  //播放激光特效
                Vector3 pos = transform.position;
                pos.y = enemyes[0].transform.position.y;
                laserEffect.transform.LookAt(pos);
            }
        }
        else
        {
            laserEffect.SetActive(false);   //停用激光特效的使用
            laserRenderer.enabled = false;
        }
       

     
    }

    void Attack()
    {
        //判断敌人是否为空
        if(enemyes[0] == null)
        {
            UpdateEnemy();
        }
        if(enemyes.Count>0)
        {
            //得到子弹
            GameObject bullet = GameObject.Instantiate(bulletPrefab, firePosition.position, firePosition.rotation);
            //设置子弹的攻击目标
            bullet.GetComponent<Bullet>().SetTarget(enemyes[0].transform);
        }
        else
        {
            timer = AttackRateTime;
        }
        
    }

    void UpdateEnemy()
    {
        //保存所有空元素的索引
        List<int> emptyIndex = new List<int>();
        //移除所有为空的元素
       
        for(int index = 0; index <enemyes.Count; index++)
        {
            if(enemyes[index]==null)
            {
                emptyIndex.Add(index);
              
            }
        }

        for(int i =0; i<emptyIndex.Count; i++)
        {
            enemyes.RemoveAt(emptyIndex[i]-i);
        }
    }
}
