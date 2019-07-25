//-------------------------------------------------------
//	
//-------------------------------------------------------

#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;


#endregion

namespace shared_data_layer
{
  /// <summary>
  /// This is the enemy interface
  /// </summary>
  public interface IEnemy
  {
	string GetId();
	string GetName();
	int GetNrTrophies();

	//Equippment

  }
}
