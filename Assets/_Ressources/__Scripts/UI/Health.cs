using UnityEngine;
using UnityEngine.UI;
public class Health : MonoBehaviour
{
    public Image[] healthPoints;

    float _health, maxHealth = 100;
    float lerpSpeed;

    private void Start()
    {
        _health = maxHealth;
    }

    private void Update()
    {
        if (_health > maxHealth) _health = maxHealth;

        lerpSpeed = 3f * Time.deltaTime;

        HealthBarFiller();
        //ColorChanger();
    }

    void HealthBarFiller()
    {
        for (int i = 0; i < healthPoints.Length; i++)
        {
            healthPoints[i].enabled = !DisplayHealthPoint(_health, i);
        }
    }
    void ColorChanger()
    {
       // Color healthColor = Color.Lerp(Color.red, Color.green, (health / maxHealth));
    }

    bool DisplayHealthPoint(float _health, int pointNumber)
    {
        return ((pointNumber * 10) >= _health);
    }

    public void Damage(float damagePoints)
    {
        if (_health > 0)
            _health -= damagePoints;
    }
    public void Heal(float healingPoints)
    {
        if (_health < maxHealth)
            _health += healingPoints;
    }
}
