using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public static Main S;
    protected static Dictionary<WeaponType, WeaponDefinition> WEAPON_DICT;

    

    [Header("Set in Inspector")]
    public GameObject[] prefabEnemies;
    public float enemySpawnPerSecond = 0.5f;
    public float enemyDefaultPadding = 1.5f;
    public WeaponDefinition[] weaponDefinitions;
    public GameObject prefabPowerUp;
    public WeaponType[] powerUpFrequency = new WeaponType[]
    {
        WeaponType.blaster,
        WeaponType.blaster,
        WeaponType.spread,
        WeaponType.shield
    };

    private BoundsCheck bndCheck;

    public void ShipDestroyed(Enemy e)
    {
        if(Random.value <= e.powerUpDropChance)
        {
            int index = Random.Range(0, powerUpFrequency.Length);
            WeaponType puType = powerUpFrequency[index];
            GameObject go = Instantiate(prefabPowerUp)as GameObject;
            PowerUp pu = go.GetComponent<PowerUp>();
            pu.SetType(puType);
            pu.transform.position = e.transform.position;
        }
    }

    private void Awake()
    {
        S= this;
        bndCheck = GetComponent<BoundsCheck>();
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
        WEAPON_DICT = new Dictionary<WeaponType, WeaponDefinition>();
        foreach(WeaponDefinition weapon in weaponDefinitions)
        {
            WEAPON_DICT[weapon.type] = weapon;
        }
    }

    public void SpawnEnemy()
    {
        int index = Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate(prefabEnemies[index]);

        float enemyPadding = enemyDefaultPadding;
        if(go.GetComponent<BoundsCheck>() != null)
        {
            enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }

        Vector3 pos = Vector3.zero;
        float xMin = -bndCheck.camWidth + enemyPadding;
        float xMax = bndCheck.camWidth - enemyPadding;
        pos.x = Random.Range(xMin,xMax);
        pos.y = bndCheck.camHeight + enemyPadding;
        go.transform.position = pos;
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
    }

    public void DelayedRestart(float delay)
    {
        Invoke("Restart", delay);
    }

    public void Restart()
    {
        SceneManager.LoadScene("_Scene_0");        
    }

    /// <summary>
    /// Static function returning WeaponDefinition from static protected из статического
    /// WEAP_DICT class Main.
    /// </summary>
    /// <returns> WeaponDefinition object or if it does not have for certain WeaponType,
    /// returns new WeaponDefinition object with type 'none'
    /// </returns>
    /// <param name="wt">Weapon type of  WeaponType,which is need to get WeaponDefinition</param>
    
    public static WeaponDefinition GetWeaponDefinition(WeaponType wt)
    {
        if (WEAPON_DICT.ContainsKey(wt))
        {
            return WEAPON_DICT[wt];
        }
        return new WeaponDefinition();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
