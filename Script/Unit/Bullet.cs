using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bullet : MonoBehaviour
{

    [SerializeField] private GameObject effetch;
    private Transform _target;
    
    private void Update()
    {
        if (_target)
        {
            transform.position = Vector3.MoveTowards(transform.position, _target.position,20f*Time.deltaTime);
            transform.LookAt(_target.position);
            if (transform.position == _target.position && gameObject.activeSelf)
            {
                GameObject eff = Instantiate(effetch, transform.position, Quaternion.identity);
                Destroy(eff, 1f);
                gameObject.SetActive(false);
                if (gameObject.CompareTag("bullet") ){
                    SoundManager.Instance.PlayBulletexplosiveBullet();
                }
                if (gameObject.CompareTag("rocket"))
                {
                    SoundManager.Instance.PlayShootRocket();
                }
                Destroy(gameObject, 2f);
            }
        } else
        {
            Destroy(gameObject);
        }
    }
    public void SetTarget(Transform target)
    {
        _target = target;
    }


}
