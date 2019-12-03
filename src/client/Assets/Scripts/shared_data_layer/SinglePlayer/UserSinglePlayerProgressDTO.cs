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

namespace shared_data_layer.SinglePlayer
{
    public class UserSinglePlayerProgressDTO
    {

        public const int maxStarsPerLevel = 5;

        public UserSinglePlayerProgressDTO()
        {
            SPLevelProgress = new Dictionary<string, int>();
            SPLevelEnemyTemplateIds = new Dictionary<string, List<string>>();
            SPLevelStars = new Dictionary<string, int>();
            CurrentLevelId = "";
        }

        public Dictionary<string, int> SPLevelProgress { get; set; }
        public Dictionary<string, List<string>> SPLevelEnemyTemplateIds { get; set; } //To be pregenerated for a level when it is started (from new or after loss).
        public Dictionary<string, int> SPLevelStars { get; set; }

        /// <summary>
        /// List of levels the user has already played at least once.
        /// </summary>
        public List<string> PlayedLevels { get; set; }

        /// <summary>
        /// Denotes the latest level the user has entered.
        /// </summary>
        public string CurrentLevelId { get; set; }
        
        /// <summary>
        /// Returns the total number of stars the user has collected
        /// </summary>
        public int TotalNumberStars
        {
            get
            {
                int totalNumberStars = 0;
                foreach (string levelId in SPLevelStars.Keys)
                {
                    totalNumberStars += SPLevelStars[levelId];
                }
                return totalNumberStars;
            }
        }

        public string GetCurrentEnemyTemplate()
        {
            int levelProgress = GetCurrentProgress();
            List<string> levelEnemyTemplates = SPLevelEnemyTemplateIds[CurrentLevelId];
            string enemyTemplate = "";
            if (levelProgress >= 0 && levelProgress < levelEnemyTemplates.Count)
            {
                enemyTemplate = levelEnemyTemplates[levelProgress];
            }
            return (enemyTemplate);
        }

        public int GetProgress(string levelId)
        {
            if (SPLevelProgress.ContainsKey(levelId))
            {
                return (SPLevelProgress[levelId]);
            }
            else
            {
                return (0);
            }
        }

        /// <summary>
        /// Method to return the current progress of the user. E.g. the amount of enemies beaten in the current level.
        /// </summary>
        /// <returns></returns>
        public int GetCurrentProgress()
        {
            return (GetProgress(CurrentLevelId));
        }

        public int GetCurrentLevelStars()
        {
            return (GetLevelStars(CurrentLevelId));
        }

        public int GetLevelStars(string levelId)
        {
            if (SPLevelStars.ContainsKey(levelId))
            {
                return (SPLevelStars[levelId]);
            }
            else
            {
                return (0);
            }
        }

        public bool HasAllLevelStars(string levelId)
        {
            return (GetLevelStars(levelId) >= maxStarsPerLevel);
        }

        /// <summary>
        /// Increases the current level progress by 1.
        /// </summary>
        public void IncrementCurrentProgress()
        {
            SPLevelProgress[CurrentLevelId] = GetCurrentProgress() + 1;
        }

        /// <summary>
        /// Increases the current level stars by the given amount.
        /// The level stars are capped to not be bigger than maxStarsPerPlanet.
        /// </summary>
        public void IncrementCurrentStars(int amount)
        {
            if (SPLevelStars.ContainsKey(CurrentLevelId) == false)
            {
                SPLevelStars[CurrentLevelId] = 0;
            }
            SPLevelStars[CurrentLevelId] += amount;
            if (SPLevelStars[CurrentLevelId] > maxStarsPerLevel)
            {
                SPLevelStars[CurrentLevelId] = maxStarsPerLevel;
            }
        }

        /// <summary>
        /// Method to reset the progress of the currently entered level.
        /// To be invoked for example when a duel is lost.
        /// </summary>
        public void ResetCurrentProgress()
        {
            ResetProgress(CurrentLevelId);
        }

        public void ResetProgress(string levelId)
        {
            SPLevelProgress[levelId] = 0;
        }

        public void ResetAllProgress()
        {
            SPLevelProgress = new Dictionary<string, int>();
            SPLevelStars = new Dictionary<string, int>();
            
        }

    }
}
