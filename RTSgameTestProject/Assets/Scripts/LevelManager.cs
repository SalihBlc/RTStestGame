using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class LevelManager : MonoBehaviour
{
    public TextMeshProUGUI UIText;
    public Level[] levels;
    public GameObject Ground;
    public GameObject SpiderPrefab;
    public GameObject TowerPrefab;
    public GameObject TankPrefab;
    public GameObject StrikerPrefab;
    private int CurrentLevel = 0;
    private int AliveEnemies;
    private int AliveAllies;
    void Start()
    {
        StartLevel(levels[CurrentLevel]);
        UIText.SetText($"Level {CurrentLevel + 1}");
    }

    void Update()
    {
        AliveEnemies = 0;
        foreach (var enemy in GameObject.FindGameObjectsWithTag("EnemyUnit"))
        {
            if (enemy.gameObject.activeSelf)
            {
                AliveEnemies += 1;
            }
        }
        AliveAllies = 0;
        foreach (var ally in GameObject.FindGameObjectsWithTag("PlayerUnit"))
        {
            if (ally.gameObject.activeSelf)
            {
                AliveAllies += 1;
            }
        }
        UIText.SetText($"Level {CurrentLevel + 1} / Remaining Enemies {AliveEnemies}");
        if (AliveEnemies == 0)
        {
            CurrentLevel += 1;
            if (CurrentLevel <= levels.Length)
                StartLevel(levels[CurrentLevel]);
            else
            {
                UIText.SetText("You won!");
                UIText.alignment = TextAlignmentOptions.Center;
                RemoveAll();
            }
        }
        else if (AliveAllies == 0)
        {
            StartLevel(levels[CurrentLevel]);
        }
    }

    private void StartLevel(Level level)
    {
        RemoveAll();
        for (int i = 1; i <= level.SpiderCount; i++)
        {
            Spawn(SpiderPrefab, RandomPoint(Ground, 3));
        }
        for (int i = 1; i <= level.TowerCount; i++)
        {
            Spawn(TowerPrefab, RandomPoint(Ground, 3));
        }

        for (int i = 1; i <= level.TankCount; i++)
        {
            Spawn(TankPrefab, RandomPoint(Ground, 1));
        }
        for (int i = 1; i <= level.StrikerCount; i++)
        {
            Spawn(StrikerPrefab, RandomPoint(Ground, 1));
        }

    }

    private Vector3 RandomPoint(GameObject ground, int times)
    {
        return new Vector3(Random.Range(-Ground.transform.localScale.x * times, Ground.transform.localScale.x * times), 0, Random.Range(-Ground.transform.localScale.z * times, Ground.transform.localScale.z * times));
    }

    private void RemoveAll()
    {
        foreach (var selectableObject in FindObjectsOfType<UnitController>())
        {
            this.GetComponent<PlayerManager>().DeselectUnits();
            GameObject.Destroy(selectableObject.gameObject);
        }
    }

    private void Spawn(GameObject Prefab, Vector3 pos)
    {
        Instantiate(Prefab, pos, Ground.transform.rotation);
    }
}
