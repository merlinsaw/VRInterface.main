using UnityEngine;

public class DisableOnAwake : MonoBehaviour
{

    public bool disable = true;

    private void Awake()
    {
        this.gameObject.SetActive(!disable);
    }

}
