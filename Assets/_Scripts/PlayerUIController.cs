using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI weaponNameText;
    [SerializeField] private TextMeshProUGUI weaponAmmoText;
    [SerializeField] private Slider healthSlider;


    public void SetHealthSliderValue(int value) {
        healthSlider.value = value;
    }

    public void SetWeaponNameText(string name) {
        weaponNameText.text = name;
    }

    public void SetWeaponAmmoText(int maxAmmo, int currentAmmo) {
        weaponAmmoText.text = currentAmmo.ToString() + "/" + maxAmmo.ToString();
    }
}
