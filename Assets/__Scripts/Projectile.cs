using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private BoundsCheck bndCheck;
    private Renderer rend;

    [Header("Set Dynamically")]
    public Rigidbody rigid;

    [SerializeField]
    private WeaponType _type;

    public WeaponType type
    {
        get { return _type; }
        set { SetType(value); }
    }
    
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
        bndCheck = GetComponent<BoundsCheck>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bndCheck.offUp)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Changing private _type field and setting color for projectile,
    /// as it has definition in WeaponDefinition.
    /// </summary>
    /// <param name="eType"> WeaponType type of weapon which is used.</param>
    public void SetType(WeaponType eType)
    {
        _type = eType;
        WeaponDefinition def = Main.GetWeaponDefinition(_type);
        rend.material.color = def.projectileColor;
    }
}
