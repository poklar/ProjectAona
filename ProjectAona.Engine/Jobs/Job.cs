using ProjectAona.Engine.Tiles;
using ProjectAona.Engine.World.Items;
using ProjectAona.Engine.World.Items.Resources;
using System.Collections.Generic;
using System.Linq;

namespace ProjectAona.Engine.Jobs
{
    public class Job
    {
        protected float _jobTimer;
        protected bool _jobRepeats;

        public Tile Destination { get; private set; }

        // Item + itemnumber
        // TODO: What if the tile condition changes
        // TODO: Reserve the IStackable?
        public List<List<IStackable>> RequiredItems { get; set; }

        public List<List<IStackable>> RequiredItemsClone { get; set; }

        public JobType JobType { get; private set; }

        public IQueueable JobObjectPrototype { get; set; } // TODO: Do I need this?

        public IQueueable JobObject { get; set; } // TODO: Do I need this? // Finished product

        public float JobTimer { get { return _jobTimer; } set { _jobTimer = value;  } } // In seconds

        public delegate void JobHandler(Job job);
        public event JobHandler JobComplete;
        public event JobHandler JobCancel;
        public event JobHandler JobStopped;
        
        public Job(Tile tile, JobType jobType, float jobTimer = 5f)
        {
            Destination = tile;
            JobType = jobType;
            _jobTimer = jobTimer;
            _jobRepeats = false;
            RequiredItems = new List<List<IStackable>>();
            RequiredItemsClone = new List<List<IStackable>>();
        }

        public void DoJob(float workTime)
        {
            JobTimer -= workTime;

            if (JobTimer <= 0)
            {
                if (JobComplete != null)
                    JobComplete(this);

                if (!_jobRepeats)
                    if (JobStopped != null)
                        JobStopped(this);
            }
        }

        public void CancelJob()
        {
            JobCancel(this);
        }

        public Tile GetNextRequiredItem(List<List<IStackable>> items)
        {
            if (RequiredItems.Count == 0)
                return null;

            foreach (var item in RequiredItems)
            {
                if (items.Count != 0 && item.Count != 0)
                {
                    foreach (var minionItem in items)
                    {
                        if (minionItem.Count != 0)
                        {
                            if (item.FirstOrDefault().GetType() == typeof(Material) &&
                                minionItem.FirstOrDefault().GetType() == typeof(Material) &&
                                item.Count > minionItem.Count)
                            {
                                Material material = minionItem.FirstOrDefault() as Material;
                                return material.Tile;
                            }
                            else if (item.FirstOrDefault().GetType() == typeof(Food) &&
                                        minionItem.FirstOrDefault().GetType() == typeof(Food) &&
                                        item.Count > minionItem.Count)
                            {
                                Food food = minionItem.FirstOrDefault() as Food;
                                return food.Tile;
                            }
                            // TODO: Add furniture     
                        }               
                    }
                }
                else if (item.Count != 0)
                {
                    if (item.FirstOrDefault().GetType() == typeof(Material))
                    {
                        Material material = item.FirstOrDefault() as Material;
                        return material.Tile;
                    }
                    else if (item.FirstOrDefault().GetType() == typeof(Food))
                    {
                        Food food = item.FirstOrDefault() as Food;
                        return food.Tile;
                    }
                }
            }
            

            return null;
        }
    }
}
