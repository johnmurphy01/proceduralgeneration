using UnityEngine;
using System.Collections;
using FPSControl;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour {

	public float damageAmount = 1;	
	public Transform explotionPrefab;

    private DamageSource damageSource;	

    void OnCollisionEnter(Collision collisionInfo)
    {
		Damageable damageable = collisionInfo.transform.GetComponent<Damageable>();
        if (damageable != null)
        {
            DamageSource damageSource = new DamageSource();
            damageSource.appliedToPosition = collisionInfo.contacts[0].point;
            damageSource.damageAmount = damageAmount;
            damageSource.fromPosition = transform.position;
            damageSource.hitCollider = collisionInfo.collider;
            damageSource.sourceObjectType = DamageSource.DamageSourceObjectType.Player;
            damageSource.sourceType = DamageSource.DamageSourceType.GunFire;
            damageable.ApplyDamage(damageSource);
        }
		Destroy(gameObject);
		if (explotionPrefab != null) GameObject.Instantiate(explotionPrefab, transform.position, Quaternion.identity);
    }
	
}
