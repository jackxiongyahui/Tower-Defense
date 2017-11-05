using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 子弹类，实现子弹的所有效果
/// </summary>
public class Bullet : MonoBehaviour {

    public int damage = 50;  //子弹伤害
    public float speed = 20;  //子弹速度

    private Transform target;  //设置子弹攻击的目标

    public GameObject explosionEffectPrefab; //爆炸特效

    private float distanceArrivetarget = 1.2f;  //子弹到敌人的距离

	public void SetTarget(Transform _target)
    {
        this.target = _target;

    }

    private void Update()
    {
        //当敌人消失是，子弹也会直接爆炸销毁
        if (target == null)
        {
            Die();
            return;
        }
        transform.LookAt(target.position);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);


        Vector3 dir = target.position - transform.position;
        if(dir.magnitude<distanceArrivetarget)
        {
            target.GetComponent<Enemy>().TakeDamage(damage); //让敌人掉血
            Die();
        }

       
    }

    /// <summary>
    /// 这种检测方式对missile是没有没有什么用处的，因为missile是不规则的物体
    /// </summary>
    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("OnTriggerEnter");
    //    if(other.tag=="Enemy")
    //    {
    //        other.GetComponent<Enemy>().TakeDamage(damage); //让敌人掉血
    //        Die();
    //    }
    //}

    void Die()
    {
        GameObject effect = GameObject.Instantiate(explosionEffectPrefab, transform.position, transform.rotation); //添加爆炸特效
        Destroy(effect, 1); //1秒之后销毁特效
        Destroy(this.gameObject); //销毁子弹
    }
}
