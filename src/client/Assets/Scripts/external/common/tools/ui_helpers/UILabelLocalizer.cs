//-------------------------------------------------------
//	
//-------------------------------------------------------

#region using

using UnityEngine;

#endregion

/// <summary>
/// Simple helper script, to be applied to UILabels, to allow for setting loca codes in the scene inspector without having to move to code.
/// </summary>
public class UILabelLocalizer : MonoBehaviour
{

    private static readonly _Logger log = new _Logger(typeof(UILabelLocalizer));

    public string locaCode = "";

    public delegate void LabelLocalizerDelegate(UILabel label, string locaCode);
    public static event LabelLocalizerDelegate OnAddLabelToAutoLocaList;

    public void Awake()
    {

        if (locaCode.StartsWith("\"") || locaCode.EndsWith("\""))
        {
            log.Warn(_Logger.User.Msaw, "The locaCode starts or ends with \". Make sure that this is not a copy&paste error.");
        }

        UILabel label = this.GetComponent<UILabel>();
        if (label != null)
        {
            if (OnAddLabelToAutoLocaList != null)
            {
                OnAddLabelToAutoLocaList(label, locaCode);
            }
            else
            {
                log.Warn(_Logger.User.Msaw, "OnAddLabelToAutoLocaList == null");
            }
        }
        else
        {
            log.Warn(_Logger.User.Msaw, "Cannot localize code '" + locaCode + "' on gameobject '" + name + "' because it doesn't contain a UILabel component.");
        }

        Destroy(this); //Self destruct to save performance.
    }
}