using ProjectAona.Engine.Tiles;

namespace ProjectAona.Engine.Jobs
{
    public class Job
    {
        private float _jobTimer;
        private bool _jobRepeats;

        public Tile Tile { get; private set; }

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
            Tile = tile;
            JobType = jobType;
            _jobTimer = jobTimer;
            _jobRepeats = false;
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
    }
}
