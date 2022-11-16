using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum Teams
    {
        BLUE,
        GREEN,
        BOTH
    }

    [SerializeField]
    private GameObject blueWizard;

    [SerializeField]
    private GameObject greenWizard;

    [SerializeField]
    private int maxEnemiesEachTeam = 40;

    [SerializeField]
    private float spawnRhythm = 2f;

    [SerializeField]
    private Text nbBlueEnemies;

    [SerializeField]
    private Text nbGreenEnemies;

    [SerializeField]
    private Text nbBlueSpawner;

    [SerializeField]
    private Text nbGreenSpawner;

    [SerializeField]
    private Text winningText;

    private int blueEnemyCount;

    private int greenEnemyCount;

    private float spawnTimer;

    private GameObject[] blueEnemies;

    private GameObject[] greenEnemies;

    private List<GameObject> blueSpawners;

    private List<GameObject> greenSpawners;

    private GameObject[] blueBushes;

    private GameObject[] greenBushes;

    private List<GameObject> blueCovers;

    private List<GameObject> greenCovers;

    private void Awake()
    {
        blueSpawners = GameObject.FindGameObjectsWithTag("BlueSpawner").Cast<GameObject>().ToList();
        greenSpawners = GameObject.FindGameObjectsWithTag("GreenSpawner").Cast<GameObject>().ToList();
        blueBushes = GameObject.FindGameObjectsWithTag("BlueBush");
        greenBushes = GameObject.FindGameObjectsWithTag("GreenBush");
        blueEnemies = InstantiateEnemies(blueWizard);
        greenEnemies = InstantiateEnemies(greenWizard);
        blueCovers = blueSpawners.Concat(blueBushes).Cast<GameObject>().ToList();
        greenCovers = greenSpawners.Concat(greenBushes).Cast<GameObject>().ToList();
        WriteEnemyCount("WizardBlue");
        WriteEnemyCount("WizardGreen");
        WriteSpawnerCount();
    }

    private GameObject[] InstantiateEnemies(GameObject wizard)
    {
        GameObject[] enemies = new GameObject[maxEnemiesEachTeam];

        for (int j = 0; j < maxEnemiesEachTeam; j++)
        {
            enemies[j] = Instantiate(wizard);
            enemies[j].SetActive(false);
        }
        return enemies;
    }

    private void Update()
    {
        ManageEnemies();
        if(greenEnemyCount == 0 && greenSpawners.Count == 0)
        {
            winningText.color = Color.cyan;
            winningText.text = "Partie terminée ! Les bleus gagnent !";
        } else if(blueEnemyCount == 0 && blueSpawners.Count == 0)
        {
            winningText.color = Color.green;
            winningText.text = "Partie terminée ! Les verts gagnent !";
        }
        if (Input.GetButtonDown("Cancel"))
        {
            Application.Quit();
        }
    }

    private void ManageEnemies()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnRhythm)
        {
            SpawnEnemy(blueEnemies, blueSpawners,"WizardBlue");
            SpawnEnemy(greenEnemies, greenSpawners, "WizardGreen");
            WriteEnemyCount("WizardBlue");
            WriteEnemyCount("WizardGreen");
            spawnTimer = 0;
            spawnRhythm += 0.05f;
        }
    }

    private void SpawnEnemy(GameObject[] enemies, List<GameObject> spawners, string tag)
    {
        if (spawners.Count != 0)
        {
            GameObject enemy = GetFirstInactiveEnemy(enemies);
            if (enemy != null && enemy.activeSelf)
            {
                enemy.transform.position = spawners[Random.Range(0, spawners.Count - 1)].transform.position + (transform.up * 2);
                ChangeEnemyCount(tag, 1);
            }
        }
    }

    private GameObject GetFirstInactiveEnemy(GameObject[] enemies)
    {
        for (int i = 0; i < maxEnemiesEachTeam; i++)
        {
            if (!enemies[i].activeInHierarchy)
            {
                enemies[i].SetActive(true);
                return enemies[i];
            }
        }
        return null;
    }

    public GameObject GetOneOfLastEnemiesAlive(string tag)
    {
        GameObject[] enemies;
        if(tag == "WizardBlue")
        {
            enemies = greenEnemies;
        } else
        {
            enemies = blueEnemies;
        }
        for (int i = 0; i < maxEnemiesEachTeam; i++)
        {
            if (enemies[i].activeInHierarchy)
            {
                return enemies[i];
            }
        }
        return null;
    }

    public void ChangeEnemyCount(string tag, int value)
    {
        if (tag == "WizardBlue")
        {
            blueEnemyCount += value;
        }
        else
        {
            greenEnemyCount += value;
        }
        WriteEnemyCount(tag);
    }

    public void WriteEnemyCount(string tag)
    {
        if (tag == "WizardBlue")
        {
            nbBlueEnemies.text = "Enemies bleus restants: " + blueEnemyCount.ToString();
        }
        else
        {
            nbGreenEnemies.text = "Enemies verts restants: " + greenEnemyCount.ToString();
        }
    }

    public void WriteSpawnerCount()
    {
        nbBlueSpawner.text = "Spawners bleus restants: " + blueSpawners.Count.ToString();
        nbGreenSpawner.text = "Spawners verts restants: " + greenSpawners.Count.ToString();
    }

    public void SpawnPointDeactivated(GameObject spawnPoint)
    {
        if (spawnPoint.CompareTag("BlueSpawner"))
        {
            blueSpawners.Remove(spawnPoint);
        }
        else
        {
            greenSpawners.Remove(spawnPoint);
        }
        WriteSpawnerCount();
    }

    public List<GameObject> GetAllTargetsByType(string tag, bool cover)
    {
        if (tag == "WizardBlue")
        {
            if (cover)
            {
                return blueCovers.Cast<GameObject>().ToList();
            }
            return greenSpawners;
        }
        else
        {
            if (cover)
            {
                return greenCovers.Cast<GameObject>().ToList();
            }
            return blueSpawners;

        }
    }
}
