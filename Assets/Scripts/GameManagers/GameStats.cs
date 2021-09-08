using UnityEngine;
using UnityEngine.UI;

public class GameStats : MonoBehaviour
{
    public int turretPoints = 100;
    public int projectilePoints = 50;
    public int generatorPoints = 50;
    public int allGeneratorPoints = 250;

    public static int TurretPoints;
    public static int ProjectilePoints;
    public static int GeneratorPoints;
    public static int AllGeneratorPoints;

    public Text score;

    private void Awake()
    {
        TurretPoints = turretPoints;
        ProjectilePoints = projectilePoints;
        GeneratorPoints = generatorPoints;
        AllGeneratorPoints = allGeneratorPoints;
    }

    private void Update()
    {
        score.text = "SCORE: " + PlayerStats.Score;
    }
}
