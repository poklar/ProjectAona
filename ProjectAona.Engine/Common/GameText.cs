namespace ProjectAona.Engine.Common
{
    public class GameText
    {
        public class BuildMenu
        {
            #region Main in-game menu
            public const string BUILD = "Build";
            public const string AGRICULTURE = "Agriculture";
            public const string JOBS = "Jobs";
            public const string RESEARCH = "Research";
            public const string RELATIONS = "Relations";
            #endregion

            #region Build menu
            public const string WORKSHOP = "Workshop";
            public const string STRUCTURE = "Structure";
            public const string FURNITURE = "Furniture";
            public const string STORAGE = "Storage";
            public const string FLOOR = "Floor";
            public const string ROOM = "Room";

            #region Structure sub menu
            public const string WALL = "";
            public const string WOODWALL = "Wood Wall";
            public const string BRICKWALL = "Brick Wall";
            public const string STONEWALL = "Stone Wall";

            public const string DOOR = "Door";
            public const string WOODDOOR = "Wood";
            #endregion

            #region Storage sub menu
            public const string STORAGEAREA = "Area";
            public const string STORAGECRATE = "Crate";
            public const string STORAGEBARREL = "Barrel";
            #endregion

            #endregion

            public const string DECONSTRUCT = "Deconstruct";
            public const string CANCEL = "Cancel";
        }

        public class Material
        {
            #region Raw materials
            public const string WOOD = "Wood";
            public const string DIRT = "Dirt";
            public const string COALORE = "CoalOre";
            public const string STONEORE = "StoneOre";
            public const string IRONORE = "IronOre";
            #endregion
        }
    }
}
