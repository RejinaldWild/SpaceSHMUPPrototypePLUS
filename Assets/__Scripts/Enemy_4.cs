using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Part - serializable class like WeaponDefinition to store data
/// </summary>

[System.Serializable]
public class Part
{
    public string name;
    public float health;
    public string[] protectedBy;

    [HideInInspector]
    public GameObject go;

    [HideInInspector]
    public Material mat;
}

/// <summary>
/// Enemy_4 is going to spawn behind top point, choose new point and going to it.
/// Approached to the point it choose new random point and continue going until
/// player destroy the enemy.
/// </summary>

public class Enemy_4 : Enemy
{
    [Header("Set in Inspector: Enemy_4")]
    public Part[] parts;

    private Vector3 p0,p1;
    private float _timeStart;
    private float _duration = 4f;

    // Start is called before the first frame update
    void Start()
    {
        p0 = p1 = pos;
        InitMovement();

        Transform t;
        foreach(Part part in parts)
        {
            t = transform.Find(part.name);
            if(t != null)
            {
                part.go = t.gameObject;
                part.mat = part.go.GetComponent<Renderer>().material;
            }
        }
    }

    private void InitMovement()
    {
        p0 = p1;
        float widthMinRad = bndCheck.camWidth - bndCheck.radius;
        float heightMinRad = bndCheck.camHeight - bndCheck.radius;
        p1.x = Random.Range( - widthMinRad, widthMinRad );
        p1.y = Random.Range( - heightMinRad, heightMinRad );
        _timeStart = Time.time;
    }

    public override void Move()
    {
        float u = (Time.time - _timeStart)/_duration;
        if (u >= 1)
        {
            InitMovement();
            u = 0;
        }
        u = 1 - Mathf.Pow(1 - u, 2);
        pos = (1 - u) * p0 + u * p1;
    }

    private Part FindPart(string n)
    {
        foreach( Part part in parts)
        {
            if(part.name == n)
            {
                return part;
            };
        }
        return null;
    }

    private Part FindPart(GameObject go)
    {
        foreach (Part part in parts)
        {
            if (part.go == go)
            {
                return part;
            };
        }
        return null;
    }

    private bool isPartDestroyed(string n)
    {
        return isPartDestroyed(FindPart(n));
    }

    private bool isPartDestroyed(GameObject go)
    {
        return isPartDestroyed(FindPart(go));
    }

    private bool isPartDestroyed(Part part)
    {
        if(part == null)
        {
            return true;
        }
        return (part.health <= 0);
    }

    private void ShowLocaleDamage(Material m)
    {
        m.color = Color.red;
        damageDoneTime = Time.time + showDamageDuration;
        showingDamage = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        switch (other.tag)
        {
            case "ProjectileHero":
                 Projectile p = other.GetComponent<Projectile>();
                if (!bndCheck.isOnScreen)
                {
                    Destroy(other);
                    break;
                }

                GameObject goHit = collision.contacts[0].thisCollider.gameObject;
                Part partHit = FindPart(goHit);
                if(partHit == null)
                {
                    goHit = collision.contacts[0].otherCollider.gameObject;
                    partHit = FindPart(goHit);
                }

                if(partHit.protectedBy != null)
                {
                    foreach( string s in partHit.protectedBy)
                    {
                        if (!isPartDestroyed(s))
                        {
                            Destroy(other);
                            return;
                        }
                    }
                }

                partHit.health -= Main.GetWeaponDefinition(p.type).damageOnHit;
                ShowLocaleDamage(partHit.mat);

                if (partHit.health <= 0)
                {
                    partHit.go.SetActive(false);
                }

                bool allDestroyed = true;
                foreach(Part part in parts)
                {
                    if(!isPartDestroyed(part))
                    {
                        allDestroyed = false;
                        break;
                    }
                }

                if (allDestroyed)
                {
                    Main.S.ShipDestroyed(this);
                    Destroy(this.gameObject);
                }

                Destroy(other);
                break;
        }
    }
}
