using System;
using System.ComponentModel;
using System.Threading;
using Craft.Net.Data.Events;
namespace Craft.Net.Data.Entities
{
    public class PlayerEntity : LivingEntity
    {
        public PlayerEntity()
        {
            Inventory = new Slot[45];
            for (int i = 0; i < Inventory.Length; i++)
                Inventory[i] = new Slot(0xFFFF, 0);
            SelectedSlot = InventoryHotbar;
            bedUseTimer = new Timer(state =>
            {
                if (BedTimerExpired != null)
                    BedTimerExpired(this, null);
            });
            BedPosition = -Vector3.One;
            Health = 20;
            Food = 20;
        }

        #region Constants

        public const int InventoryHotbar = 36;
        public const int InventoryCraftingGrid = 1;
        public const int InventoryCraftingOutput = 0;
        public const int InventoryArmor = 5;
        public const int InventoryMain = 9;

        #endregion

        #region Properties

        public override Size Size
        {
            get { return new Size(0.6, 1.8, 0.6); }
        }

        public override short MaxHealth
        {
            get { return 20; }
        }

        public static double Width
        {
            get { return 0.6; }
        }

        public static double Height
        {
            get { return 1.8; }
        }

        public static double Depth
        {
            get { return 0.6; }
        }

        public short Food { get; set; }
        public float FoodSaturation { get; set; }
        public float FoodExhaustion { get; set; }
        public int XpLevel { get; set; }
        public int XpTotal { get; set; }
        public float XpProgress { get; set; }

        public string Username { get; set; }
        /// <summary>
        /// The client's current inventory.
        /// </summary>
        public Slot[] Inventory { get; set; }
        public short SelectedSlot { get; set; }
        /// <summary>
        /// Set to -Vector3.One if the player is not in a bed.
        /// </summary>
        public Vector3 BedPosition { get; set; }
        public GameMode GameMode { get; set; }
        public Vector3 SpawnPoint { get; set; }

        private Timer bedUseTimer;

        public event EventHandler BedStateChanged, BedTimerExpired;

        #endregion

        public void SetSlot(short index, Slot slot)
        {
            if (InventoryChanged != null)
                InventoryChanged(this, new InventoryChangedEventArgs()
                {
                    Index = index,
                    OldValue = Inventory[index],
                    NewValue = slot
                });
            Inventory[index] = slot;
        }

        public void EnterBed(Vector3 position)
        {
            BedPosition = position;
            bedUseTimer.Change(5000, Timeout.Infinite);
            if (BedStateChanged != null)
                BedStateChanged(this, null);
        }

        public event EventHandler<InventoryChangedEventArgs> InventoryChanged;

        public void LeaveBed()
        {
            BedPosition = -Vector3.One;
            if (BedStateChanged != null)
                BedStateChanged(this, null);
        }
    }
}