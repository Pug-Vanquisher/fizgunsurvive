using UnityEngine;

public class SetkaFix : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
    }


}
