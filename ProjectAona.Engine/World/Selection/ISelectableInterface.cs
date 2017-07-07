using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAona.Engine.World.Selection
{
    public interface ISelectableInterface
    {
        /// <summary>
        /// Gets the name of the object.
        /// </summary>
        /// <returns></returns>
        string GetName();
    }
}
