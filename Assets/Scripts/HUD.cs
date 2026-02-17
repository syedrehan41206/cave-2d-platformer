using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Slider slider;
    public PlayerControl playerControl;
    public float maxValue;
    public float minValue;

    private void Start()
    {
        maxValue = playerControl.maxHealth;
       slider.maxValue = maxValue;
    }
    private void Update()
    {
        slider.value = playerControl.currentHealth;
   }
}
  