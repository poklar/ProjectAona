// Credit goes to: https://blogs.msdn.microsoft.com/ericlippert/tag/astar/

using System.Collections;
using System.Collections.Generic;

namespace ProjectAona.Engine.Pathfinding
{
    public class Path<Node> : IEnumerable<Path<Node>>
    {
        public Node LastStep { get; private set; }

        public Path<Node> PreviousSteps { get; private set; }

        public float TotalCost { get; private set; }

        private Path(Node lastStep, Path<Node> previousSteps, float totalCost)
        {
            LastStep = lastStep;
            PreviousSteps = previousSteps;
            TotalCost = totalCost;
        }

        public Path(Node start)
            : this(start, null, 0)
        {

        }

        public Path<Node> AddStep(Node step, float stepCost)
        {
            return new Path<Node>(step, this, TotalCost + stepCost);
        }

        public IEnumerator<Path<Node>> GetEnumerator()
        {
            for (Path<Node> p = this; p != null; p = p.PreviousSteps)
                yield return p;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
