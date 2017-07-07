﻿using Microsoft.Xna.Framework;
using ProjectAona.Engine.World.Selection;
using System;
namespace ProjectAona.Engine.World.TerrainObjects
{
    public class Flora : ISelectableInterface
    {
        public Vector2 Position { get; set; }

        public FloraType FloraType { get; set; }

        public bool IsCut { get; set; }

        // TODO: Add a regrowth timer? Especially for berries?

        public Flora(Vector2 position, FloraType type)
        {
            Position = position;
            FloraType = type;
            IsCut = false;
        }

        /// <summary>
        /// Gets the name of the object.
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return FloraType.ToString();
        }
    }
}
