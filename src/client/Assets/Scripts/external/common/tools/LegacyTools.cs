using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LegacyTools {
  public static void SetActiveRecursively(GameObject go, bool value) {
	Dictionary<int, Transform> listOfObjectsToBeSetActiveById = new Dictionary<int, Transform>();
	Dictionary<int, Transform> listOfObjectsToBeSetToActivationListById = new Dictionary<int, Transform>();
	listOfObjectsToBeSetToActivationListById.Add(go.transform.GetInstanceID(), go.transform);
	while (listOfObjectsToBeSetToActivationListById.Count > 0) {
	  List<Transform> objs = new List<Transform>();
	  foreach (int tr in listOfObjectsToBeSetToActivationListById.Keys) {
		objs.Add(listOfObjectsToBeSetToActivationListById[tr]);
	  }
	  foreach (Transform tr in objs) {
		for (int i = 0; i < tr.childCount; i++) {
		  listOfObjectsToBeSetToActivationListById.Add(tr.GetChild(i).GetInstanceID(), tr.GetChild(i));
		}
		listOfObjectsToBeSetActiveById.Add(tr.GetInstanceID(), tr);
		listOfObjectsToBeSetToActivationListById.Remove(tr.GetInstanceID());
	  }
	}
	// here set activation:
	foreach (int gameobjNr in listOfObjectsToBeSetActiveById.Keys) {
	  listOfObjectsToBeSetActiveById[gameobjNr].gameObject.SetActive(value);
	}
	listOfObjectsToBeSetActiveById.Clear();
	listOfObjectsToBeSetToActivationListById.Clear();
  }
}