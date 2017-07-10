using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAona.Engine.Jobs
{
    public static class JobQueue
    {
        private static Queue<Job> _jobQueue = new Queue<Job>();

        public delegate void JobHandler(Job job);
        public static event JobHandler JobCreated;

        public static void Enqueue(Job job)
        {
            if (job.JobTimer < 0)
            {
                job.DoJob(0);
                return;
            }

            _jobQueue.Enqueue(job);

            if (JobCreated != null)
                JobCreated(job);
        }

        public static Job Dequeue()
        {
            if (_jobQueue.Count == 0)
                return null;

            return _jobQueue.Dequeue();
        }

        public static void Remove(Job job)
        {
            List<Job> jobs = new List<Job>(_jobQueue);

            if (!jobs.Contains(job))
                return;

            jobs.Remove(job);

            _jobQueue = new Queue<Job>(jobs);
        }

        public static Job Peek()
        {
            if (_jobQueue.Count > 0)
                return _jobQueue.Peek();

            return null;
        }

        public static IEnumerator<Job> GetEnumerator()
        {
            return _jobQueue.GetEnumerator();
        }
    }
}
