using ProjectAona.Engine.Tiles;
using ProjectAona.Engine.World;
using System.Linq;

namespace ProjectAona.Engine.Jobs
{
    public class Job
    {
        private float _jobTimer;

        public Tile Tile { get; private set; }

        public JobType JobType { get; private set; }

        public IQueueable JobObjectPrototype { get; set; } // TODO: Do I need this?

        public IQueueable JobObject { get; set; } // TODO: Do I need this? // Finished product

        public float JobTimer { get { return _jobTimer; } set { _jobTimer = value; if (_jobTimer <= 0) JobComplete(JobObjectPrototype, this); } } // In seconds

        public delegate void JobHandler(IQueueable item, Job job);
        public event JobHandler JobComplete;
        public event JobHandler JobCancel;
        
        public Job(Tile tile, JobType jobType, float jobTimer = 5f)
        {
            Tile = tile;
            JobType = jobType;
            _jobTimer = jobTimer;
        }
    }
}
