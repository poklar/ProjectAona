using ProjectAona.Engine.Tiles;
using ProjectAona.Engine.World.Selection;
using Newtonsoft.Json;
using System.IO;
using System.Data;
using MonoGame.Extended.Serialization;
using System.Collections.Generic;
using ProjectAona.Engine.Jobs;

namespace ProjectAona.Engine.World.Items.Resources
{
    public class Material : IStackable, ISelectableInterface , IQueueable 
    {
        public Tile Tile { get; set; }

        public string ItemName { get; set; }

        public int MovementCost { get; set; }

        public int MaxStackSize { get; set; }

        public Material() { }

        public Material(Tile tile)
        {
            Tile = tile;
        }

        protected Material(IStackable other)
        {
            Tile = other.Tile;
            ItemName = other.ItemName;
            MovementCost = other.MovementCost;
            MaxStackSize = other.MaxStackSize;
        }

        // Copy constructor
        public IStackable Clone()
        {
            return new Material(this);
        } 

        public void LoadMaterial(string itemName)
        {            
            var materials = JsonConvert.DeserializeObject<MaterialRoot>(File.ReadAllText(@"..\..\..\..\data\items\materials.json"));

            foreach (var material in materials.Materials)
            {
                if (material.ItemName == itemName)
                {
                    ItemName = material.ItemName;
                    MaxStackSize = material.MaxStackSize;
                    MovementCost = material.MovementCost;
                    return;
                }
            }
        }

        public string GetName()
        {
            return ItemName + " x" + Tile.Item.Count;
        }

        public class MaterialRoot
        {
            public Material[] Materials { get; set; }
        }
    }


}
