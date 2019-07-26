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
using Valve.VR;

#endregion
using GameStates;

namespace UI
{
    public class WidgetButtonStatisticsCategory : NestedControllerBehaviour
    {

        public GameStateStatistics.GameStatisticsCategories statisticsCategory;

        //private MonoBehaviourRef<WidgetNotificationNewItems> widgetNotificationNewItems = new MonoBehaviourRef<WidgetNotificationNewItems>("widget_new_item_counter");

        public void Awake()
        {
            //    widgetNotificationNewItems.LoadRef(this);
        }

        public void SetNewItemsCount(int count)
        {
            //    StartCoroutine(SetNewItemsCountCoroutine(count));
        }

        //private IEnumerator SetNewItemsCountCoroutine(int count)
        //{
        //    while (widgetNotificationNewItems.Reference == null)
        //    {
        //        yield return null;
        //    }
        //    widgetNotificationNewItems.Reference.ItemCount = count;
        //}

    }
}
