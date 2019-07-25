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
  /// Abstract base class for both UserDTO and EnemyDTO
  /// </summary>
  public class AbstractPlayerDTO
  {
	public string PlayerId { get; set; }
	public string Name { get; set; }
	public int NrTrophies { get; set; }
	//public UserEquipmentDTO UserEquipment { get; set; }



	public AbstractPlayerDTO()
	{
	  //UserEquipment = new UserEquipmentDTO();
	}


	public string GetId()
	{
	  return PlayerId;
	}

	public string GetName()
	{
	  return Name;
	}

	public int GetNrTrophies()
	{
	  return NrTrophies;
	}

  }
}