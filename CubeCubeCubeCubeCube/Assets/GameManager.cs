using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    static public GameManager instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameManager>();
            }
            return _instance;
        }
    }
    static private GameManager _instance;
    [Header("Current Session")]
    public float score = 0;
    public int difficulty = 0;
    [Header("Basic")]
    public float worldBottomBoundary = -15f;
    public float wallOfDeathSpeed = 15f;
    public float wallOfDeathMaxSpeed = 20f;
    public float preferredDistanceBetweenWallAndPlayer = 20f;
    public float maximumDistanceBetweenWallAndPlayer = 40f;
    public float distancePerDifficulty = 500f;

    [Header("Colour Settings")]
    public Material targetMaterial;
    public List<Color> floorColourPerDifficulty;
    public List<Color> ambientColourPerDifficulty;
    public List<Color> fogColourPerDifficulty;
    [Header("Enemy Spawning Rules")]
    public float firstSpawnDelay;
    public float minimumSpawnInterval;
    public float maximumSpawnInterval;
    public List<GameObject> enemies = new List<GameObject>();
    public List<Transform> enemySpawnPoints = new List<Transform>();
    [Header("Platform Spawning Rules")]
    public float minimumPlatformAltitude = 0f;
    public float maximumPlatformAltitude = 20f;
    public float spawnAheadDistance = 85f;
    public float despawnBehindDistance = 5f;
    public float calculateFuturePositionOfPlayer = 1f;
    private List<Platform> currentPlatforms = new List<Platform>();
    public Transform parentOfAllPlatforms;
    public List<GameObject> regularPrefabs;
    public List<GameObject> ascendingPrefabs;
    public List<GameObject> descendingPrefabs;
    public List<GameObject> specialPrefabs;
    [HideInInspector]
    public float gameStartTime;
    [HideInInspector]
    public float gameEndTime;


    [Header("Pre-Configurations")]
    public GameObject scoreGib;
    public GameObject gib;
    public GameObject bulletHit;
    public Texture2D cursorTexture;
    public Camera mainCamera;
    private Player player;
    public GameObject poolOfDeath;
    public GameObject wallOfDeath;

    public int totalShots = 0;
    public int missedShots = 0;

    public int killCount = 0;


    public float travelDistance;

    public float playTime
    {
        get
        {
            return gameEndTime - gameStartTime;
        }
    }

    public GameObject gameOverScreen;

    private void Start () {
        Cursor.SetCursor(cursorTexture, new Vector3(30, 30), CursorMode.Auto);
        player = Player.instance;
        gameStartTime = Time.time;
        currentPlatforms.Add(GameObject.Find("Start Platform").GetComponent<Platform>());
        StartCoroutine("EnemySpawnerRoutine");

    }
	
    public void AddScore(float amount)
    {
        score += amount * (difficulty + 1);
        if (amount >= 5)
        {
            ForegroundCanvasController.instance.SetScoreEmission(2.5f);
            General.instance.sfx.Play("Score");
            
        }
        
    }
    public void SpawnBulletHit(Vector3 position)
    {
        Destroy( Instantiate(bulletHit, position, Quaternion.identity), 2f);

    }
    public void SpawnGib(Vector3 position, int amount)
    {
        for (int i = 1; i <= amount; i++) //취향입니다 존중하세요
        {
            Instantiate(gib, position, Random.rotation);
        }
    }

    public void SpawnScoreGib(Vector3 position, int amount)
    {
        for(int i = 1; i <= amount; i++)
        {
            Instantiate(scoreGib, position, Random.rotation);
        }
    }

    public void SpawnEnemy(GameObject enemyType, int amount)
    {
        Vector3 spawnPos = enemySpawnPoints[Random.Range(0, enemySpawnPoints.Count)].position;
        for (int i = 1; i <= amount; i++)
        {
            Instantiate(enemyType, spawnPos, Quaternion.identity);
        }

    }


    private Vector3 GetLastExitPosition()
    {
        return currentPlatforms[currentPlatforms.Count - 1].exitPoint.position;
    }


    public void SpawnAppropriatePlatform()
    {
        List<GameObject> pool = new List<GameObject>();
        pool.AddRange(regularPrefabs);

        
        if (GetLastExitPosition().y > minimumPlatformAltitude)
        {
            pool.AddRange(descendingPrefabs);
        }
        
        if(GetLastExitPosition().y < maximumPlatformAltitude)
        {
            pool.AddRange(ascendingPrefabs);
        }
        SpawnRandomOutOfPool(pool);
    }


    public void SpawnRandomOutOfPool(List<GameObject> pool)
    {
        GameObject platformType;
        platformType = pool[Random.Range(0, pool.Count)];
        Platform newPlatform = Instantiate(platformType).GetComponent<Platform>();

        if(currentPlatforms.Count != 0)
        {
            newPlatform.targetPosition = currentPlatforms[currentPlatforms.Count - 1].targetPosition - currentPlatforms[currentPlatforms.Count - 1].transform.position +
                GetLastExitPosition()
                - (newPlatform.entryPoint.position - newPlatform.transform.position);
        }
        else
        {
            newPlatform.targetPosition = Player.instance.transform.position + Vector3.down * 2f - (newPlatform.entryPoint.position - newPlatform.transform.position);
        }
        newPlatform.transform.parent = parentOfAllPlatforms;
        currentPlatforms.Add(newPlatform);
        StartCoroutine(PlatformSpawnRoutine(newPlatform, newPlatform.targetPosition));
    }

    public void UpdateColour()
    {
        StopCoroutine("GradualColourChangeRoutine");
        StartCoroutine(nameof(GradualColourChangeRoutine));

        
    }

    IEnumerator GradualColourChangeRoutine()
    {
        Color startFloorColour = targetMaterial.color;
        Color startFogColour = RenderSettings.fogColor;
        Color startAmbientColour = RenderSettings.ambientLight;

        for(float t = 0; t < 1; t += Time.deltaTime*.5f)
        {
            targetMaterial.color = Color.Lerp(startFloorColour, floorColourPerDifficulty[difficulty], t);
            RenderSettings.fogColor = Color.Lerp(startFogColour, fogColourPerDifficulty[difficulty], t);
            RenderSettings.ambientLight = Color.Lerp(startAmbientColour, ambientColourPerDifficulty[difficulty], t);
            yield return null;
        }
        targetMaterial.color = floorColourPerDifficulty[difficulty];
        RenderSettings.fogColor = fogColourPerDifficulty[difficulty];
        RenderSettings.ambientLight = ambientColourPerDifficulty[difficulty];
    }

    private void UpdateDifficulty()
    {
        float timeElapsed = Time.time - gameStartTime;
        int newDifficulty = Mathf.Clamp((int)(travelDistance / distancePerDifficulty), 0, 5);
        if (difficulty == newDifficulty) return;
        difficulty = newDifficulty;
        UpdateColour();
        switch (difficulty)
        {
            case 0:
                player.playerZaWarudo.regularTimescale = 0.8f;
                player.shieldChargePerSecond = 25f;
                wallOfDeathSpeed = 8;
                break;
            case 1:
                player.playerZaWarudo.regularTimescale = 0.9f;
                player.shieldChargePerSecond = 20f;
                wallOfDeathSpeed = 8.5f;
                break;
            case 2:
                player.playerZaWarudo.regularTimescale = 1.0f;
                player.shieldChargePerSecond = 17f;
                wallOfDeathSpeed = 9.5f;
                break;
            case 3:
                player.playerZaWarudo.regularTimescale = 1.07f;
                player.shieldChargePerSecond = 15f;
                wallOfDeathSpeed = 10.5f;
                break;
            case 4:
                player.playerZaWarudo.regularTimescale = 1.12f;
                player.shieldChargePerSecond = 13f;
                wallOfDeathSpeed = 11.5f;
                break;
            case 5:
                player.playerZaWarudo.regularTimescale = 1.15f;
                player.shieldChargePerSecond = 10f;
                wallOfDeathSpeed = 12.5f;
                break;
        }
    }

    IEnumerator EnemySpawnerRoutine()
    {
        int dice;

        while (true)
        {
            yield return new WaitForSeconds(firstSpawnDelay);
            dice = Random.Range(1, 7);
            if (difficulty >= 0 && difficulty<= 1)
            {
                if (dice == 1)
                {
                    SpawnEnemy(enemies[1], 2);
                }
                else
                {
                    SpawnEnemy(enemies[0], 1);
                }

            } else if (difficulty >= 2 && difficulty <= 3)
            {
                if (dice <= 2)
                {
                    SpawnEnemy(enemies[0], 2);
                } else if (dice <= 4)
                {
                    SpawnEnemy(enemies[0], 1);
                }else if(dice == 5)
                {
                    SpawnEnemy(enemies[1], 3);
                }
                else
                {
                    SpawnEnemy(enemies[2], 1);
                }

            }
            else
            {
                switch (dice)
                {
                    case 1:
                        SpawnEnemy(enemies[0], 1);
                        break;
                    case 2:
                        SpawnEnemy(enemies[0], 2);
                        break;
                    case 3:
                        SpawnEnemy(enemies[1], 4);
                        break;
                    case 4:
                        SpawnEnemy(enemies[2], 1);
                        break;
                    case 5:
                        SpawnEnemy(enemies[0], 1);
                        SpawnEnemy(enemies[1], 2);
                        break;
                    case 6:
                        SpawnEnemy(enemies[2], 1);
                        break;
                } 
            }
            yield return new WaitForSeconds(Random.Range(minimumSpawnInterval, maximumSpawnInterval));
        }
    }


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.R))
        {
            int highscore = PlayerPrefs.GetInt("Highscore", 0);
            if (highscore < (int)score)
            {
                PlayerPrefs.SetInt("Highscore", (int)score);
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }


        if (!player.isDead)
        {
            AddScore(Time.unscaledDeltaTime * 10f);
            travelDistance = player.transform.position.z;
        }

        UpdateDifficulty();

        Vector3 newPosition = poolOfDeath.transform.position;
        newPosition.x = player.transform.position.x;
        newPosition.z = player.transform.position.z;
        poolOfDeath.transform.position = newPosition;
        if (currentPlatforms.Count != 0 && currentPlatforms[0].exitPoint.position.z < Player.instance.transform.position.z - despawnBehindDistance)
        {
            StartCoroutine(PlatformDespawnRoutine(currentPlatforms[0]));
            currentPlatforms.RemoveAt(0);
        }
        if(currentPlatforms.Count == 0 || currentPlatforms[currentPlatforms.Count-1].exitPoint.position.z < Player.instance.transform.position.z + Player.instance.rb.velocity.z * calculateFuturePositionOfPlayer + spawnAheadDistance)
        {
            SpawnAppropriatePlatform();
        }


        if(!player.isDead && player.transform.position.y < worldBottomBoundary)
        {
            player.TakeDamage(100f * Time.deltaTime);
            if (player.transform.position.y < worldBottomBoundary - 7f)
            {
                player.TakeDamage(200f * Time.deltaTime);
            }
        }

       

        if (player.transform.position.z < wallOfDeath.transform.position.z)
        {
            player.TakeDamage(50f * Time.deltaTime);
        }

        Vector3 newWallPosition = wallOfDeath.transform.position;
        

        if(player.transform.position.z - newWallPosition.z > maximumDistanceBetweenWallAndPlayer)
        {
            newWallPosition.z = player.transform.position.z - maximumDistanceBetweenWallAndPlayer;
        } else if (player.transform.position.z - newWallPosition.z > preferredDistanceBetweenWallAndPlayer)
        {
            newWallPosition.z += wallOfDeathMaxSpeed * Time.deltaTime;
        }
        else
        {
            newWallPosition.z += wallOfDeathSpeed * Time.deltaTime;
        }

        wallOfDeath.transform.position = newWallPosition;
    }


    IEnumerator PlatformSpawnRoutine(Platform platform, Vector3 targetPosition)
    {

        float timeFactor = Random.Range(.7f, 2.2f);
        for (float t = 0; t < 1f; t += Time.deltaTime * timeFactor)
        {
            platform.transform.position = targetPosition + Vector3.down * (50 - t * 50);
            yield return null;
        }
        platform.transform.position = targetPosition;
    }

    IEnumerator PlatformDespawnRoutine(Platform platform)
    {

        float timeFactor = Random.Range(.3f, .7f);
        for (float t = 0; t < 1f; t += Time.deltaTime * timeFactor)
        {
            platform.transform.position += Vector3.down * 50f * Time.deltaTime * timeFactor;
            yield return null;
        }
        Destroy(platform.gameObject);
    }

    public void OnPlayerDeath()
    {
        gameOverScreen.SetActive(true);
        StopCoroutine("EnemySpawnerRoutine");
        gameEndTime = Time.unscaledTime;

    }

}
