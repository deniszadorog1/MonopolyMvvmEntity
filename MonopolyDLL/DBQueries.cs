using MonopolyDLL.DBService;
using MonopolyDLL.Monopoly.InventoryObjs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MonopolyDLL.Monopoly;
using MonopolyDLL.Monopoly.Enums;

using Item = MonopolyDLL.Monopoly.InventoryObjs.Item;
using BoxItem = MonopolyDLL.Monopoly.InventoryObjs.BoxItem;
using System.Data;
using System.Dynamic;
using System.Security.Policy;
using System.Diagnostics;

namespace MonopolyDLL
{
    public static class DBQueries
    {
        public static List<BoxItem> GetItemsToUseInGame(int playerId)
        {
            List<BoxItem> res = new List<BoxItem>();

            using (MonopolyModel model = new MonopolyModel())
            {
                foreach (var item in model.InventoryStaffs)
                {
                    if (item.PlayerId == playerId)
                    {
                        if (!(item.IfEnabled is null) && (bool)item.IfEnabled)
                        {
                            int? id = GetItemIdByIdFromItems((int)item.StaffId);

                            if (id is null) return res;

                            BoxItem boxItem = GetBoxItemById((int)id);
                            boxItem.SetInventoryId(item.Id);
                            if (!(item.StationId is null)) boxItem.StationId = (int)item.StationId;
                            res.Add(boxItem);
                        }
                    }
                }
            }
            return res;
        }

        public static void ClearInventoryItemById(int id)
        {
            using (MonopolyModel model = new MonopolyModel())
            {
                var item = model.InventoryStaffs.FirstOrDefault(x => x.Id == id);
                if (item is null) return;
                item.StationId = null;
                item.IfEnabled = false;

                model.SaveChanges();
            }
        }

        private static int? GetItemIdByIdFromItems(int id)
        {
            using (MonopolyModel model = new MonopolyModel())
            {
                return model.Items.FirstOrDefault(x => x.Id == id).ItemId;
            }
        }


        public static void SetBoxItemWhichUserStartsToUse(BoxItem item)
        {
            int inventoryId = item.GetInventoryIdInDB();
            using (MonopolyModel model = new MonopolyModel())
            {
                var staff = model.InventoryStaffs.FirstOrDefault(s => s.Id == inventoryId);

                if (staff != null)
                {
                    staff.IfEnabled = true;
                    staff.StationId = item.StationId;
                    model.SaveChanges();
                }
            }
        }

        public static void SetBoxItemWhichUserNotUse(BoxItem item)
        {
            using (MonopolyModel model = new MonopolyModel())
            {
                foreach (var staff in model.InventoryStaffs)
                {
                    if (staff.Id == item.GetInventoryIdInDB())
                    {
                        staff.IfEnabled = false;
                        staff.StationId = null;
                        model.SaveChanges();
                        return;
                    }
                }
            }
        }


        public static BoxItem GetBoxItemByName(string name)
        {
            using (MonopolyModel model = new MonopolyModel())
            {
                foreach (var item in model.BoxItems)
                {
                    if (item.Name == name)
                    {
                        (int r, int g, int b) colorParams = GetColorByRearityId((int)item.RearityId);
                        return new BoxItem(item.Name, GetImagePathById((int)item.PicId),
                            GetRearityById((int)item.RearityId), (BusinessType)item.StationTypeId,
                            (int)item.StationId, GetMultiplierById((int)item.MultiplierId),
                            colorParams.r, colorParams.g, colorParams.b);
                    }
                }
            }
            throw new Exception("no item with such name");
        }

        public static void AddBoxItemInUserInventory(string userLogin, string itemName)
        {
            using (MonopolyModel model = new MonopolyModel())
            {
                model.InventoryStaffs.Add(new InventoryStaff()
                {
                    PlayerId = GetPlayerIdByLogin(userLogin),
                    StaffId = GetItemIdByBoxItemName(itemName),
                    StationId = null,
                    IfEnabled = false,
                });
                model.SaveChanges();
            }
        }

        private static int GetItemIdByBoxItemName(string name)
        {
            int boxItemId = GetBoxItemIdByName(name);
            using (MonopolyModel model = new MonopolyModel())
            {
                foreach (var item in model.Items)
                {
                    if (item.ItemId == boxItemId)
                    {
                        return item.Id;
                    }
                }
            }

            throw new Exception("no item with such item id");
        }

        private static int GetBoxItemIdByName(string name)
        {
            using (MonopolyModel model = new MonopolyModel())
            {
                foreach (var item in model.BoxItems)
                {
                    if (item.Name == name)
                    {
                        return item.Id;
                    }
                }
            }
            throw new Exception("no boxItem with such name");
        }

        private static int GetPlayerIdByLogin(string login)
        {
            using (MonopolyModel model = new MonopolyModel())
            {
                foreach (var player in model.Players)
                {
                    if (player.Login == login)
                    {
                        return player.Id;
                    }
                }
            }

            throw new Exception("No user with such login");
        }

        public static User GetPlayerById(int id)
        {
            using (MonopolyModel model = new MonopolyModel())
            {
                foreach (var player in model.Players)
                {
                    if (player.Id == id)
                    {
                        return new User(player.Login, player.Id);
                    }
                }
            }
            throw new Exception("No user with such Id");
        }

        public static UserInventory GetInventoryItems(string userLogin)
        {
            List<Item> inventoryItems = new List<Item>();

            //Getting lotbox items
            inventoryItems.AddRange(GetLotBoxesForInventory());

            //Getting station items
            inventoryItems.AddRange(GetItemsFromItems());

            return new UserInventory(inventoryItems);
        }

        public static List<Item> GetItemsFromItems()
        {
            List<Item> res = new List<Item>();

            using (MonopolyModel model = new MonopolyModel())
            {
                foreach (var item in model.Items)
                {
                    if (!(bool)item.IsBox)
                    {
                        res.Add(GetBoxItemById((int)item.ItemId));
                    }
                }
            }
            return res;
        }

        public static BoxItem GetBoxItemById(int id)
        {
            using (MonopolyModel model = new MonopolyModel())
            {
                foreach (var item in model.BoxItems)
                {
                    if (item.Id == id)
                    {
                        (int r, int g, int b) colorParams = GetColorByRearityId((int)item.RearityId);
                        return new BoxItem(item.Name, GetImagePathById((int)item.PicId),
                            GetRearityById((int)item.RearityId), (BusinessType)item.StationTypeId,
                            (int)item.StationId, GetMultiplierById((int)item.MultiplierId),
                            colorParams.r, colorParams.g, colorParams.b);

                    }
                }
            }
            throw new Exception("No such boxItem");
        }

        public static (int, int, int) GetColorByRearityId(int rearityId)
        {
            using (MonopolyModel model = new MonopolyModel())
            {
                foreach (var rearity in model.Rearities)
                {
                    if (rearity.Id == rearityId)
                    {
                        return GetColorInSystemColorsById((int)rearity.ColorId);
                    }
                }
            }

            throw new Exception("no Rearity with such Id");
        }

        public static (byte, byte, byte) GetColorByRearityName(string name)
        {
            using (MonopolyModel model = new MonopolyModel())
            {
                foreach (var rear in model.Rearities)
                {
                    if (rear.Rearity1 == name)
                    {
                        return GetColorInSystemColorsById((int)rear.ColorId);
                    }
                }
            }

            return (76, 180, 219);
        }

        public static (byte, byte, byte) GetColorParamsByName(string name)
        {
            using (MonopolyModel model = new MonopolyModel())
            {
                foreach (var color in model.SystemColors)
                {
                    if (color.Name == name)
                    {
                        return ((byte)color.R, (byte)color.G, (byte)color.B);
                    }
                }
            }
            return (76, 180, 219);
        }

        public static (byte, byte, byte) GetColorInSystemColorsById(int id)
        {
            using (MonopolyModel model = new MonopolyModel())
            {
                foreach (var color in model.SystemColors)
                {
                    if (color.Id == id)
                    {
                        return ((byte)color.R, (byte)color.G, (byte)color.B);
                    }
                }
            }

            throw new Exception("no color with such id");
        }

        public static List<(byte, byte, byte)> GetAllRearityColors()
        {
            List<(byte, byte, byte)> res = new List<(byte, byte, byte)>();

            using (MonopolyModel model = new MonopolyModel())
            {
                foreach (var color in model.SystemColors)
                {
                    res.Add(((byte)color.R, (byte)color.G, (byte)color.B));
                }
            }
            return res;
        }

        public static double GetMultiplierById(int id)
        {
            using (MonopolyModel model = new MonopolyModel())
            {
                foreach (var mult in model.PriceMultipliers)
                {
                    if (mult.Id == id)
                    {
                        return (double)mult.Multiplier;
                    }
                }
            }
            throw new Exception("no multiplier with such id");
        }

        public static List<Item> GetUserItemsFromInventory(int playerId)
        {
            List<Item> items = new List<Item>();
            using (MonopolyModel model = new MonopolyModel())
            {
                foreach (var item in model.InventoryStaffs)
                {
                    if (item.PlayerId == playerId)
                    {
                        Item toAdd = GetItemFromItemsTableById((int)item.StaffId);
                        if (toAdd is BoxItem boxItem)
                        {
                            boxItem.SetInventoryId(item.Id);
                            boxItem.SetTick(item.IfEnabled);
                        }
                        items.Add(toAdd);
                    }
                }
            }
            return items;
        }

        public static Item GetItemFromItemsTableById(int itemId)
        {
            using (MonopolyModel model = new MonopolyModel())
            {
                foreach (var item in model.Items)
                {
                    if (item.Id == itemId)
                    {
                        if ((bool)item.IsBox) return GetBoxById((int)item.BoxId);
                        else return GetBoxItemById((int)item.ItemId);
                    }
                }
            }
            throw new Exception("No item with such Id");
        }

        public static BusinessType GetBusTypeByid(int id)
        {
            return (BusinessType)id;
        }

        public static BusRearity GetRearityById(int id)
        {
            using (MonopolyModel model = new MonopolyModel())
            {
                foreach (var rearity in model.Rearities)
                {
                    if (rearity.Id == id)
                    {
                        return (BusRearity)id;
                    }
                }
            }
            throw new Exception("no such Rearity");
        }

        public static List<CaseBox> GetLotBoxesForInventory()
        {
            List<CaseBox> res = new List<CaseBox>();
            using (MonopolyModel model = new MonopolyModel())
            {
                List<string> names = new List<string>();
                foreach (var item in model.Items)
                {
                    if ((bool)item.IsBox)
                    {
                        res.Add(GetBoxById((int)item.Id));
                    }
                    names.Add(item.BoxId.ToString());
                }
            }
            return res;
        }

        public static CaseBox GetBoxById(int boxId)
        {
            using (MonopolyModel model = new MonopolyModel())
            {
                foreach (var dbBox in model.LotBoxes)
                {
                    if (dbBox.Id == boxId)
                    {
                        return new CaseBox(dbBox.Name, GetImagePathById((int)dbBox.PicId), GetItemsForBoxByBoxId(boxId));
                    }
                }
            }
            throw new Exception("No box with such id");
        }

        public static List<BoxItem> GetItemsForBoxByBoxId(int boxId)
        {
            List<BoxItem> res = new List<BoxItem>();

            using (MonopolyModel model = new MonopolyModel())
            {
                foreach (var item in model.BoxItems)
                {
                    if (item.BoxId == boxId)
                    {
                        (int r, int g, int b) colorParams = GetColorByRearityId((int)item.RearityId);
                        res.Add(new BoxItem(item.Name, GetImagePathById((int)item.PicId),
                            GetRearityById((int)item.RearityId), (BusinessType)item.StationTypeId,
                            (int)item.StationId, GetMultiplierById((int)item.MultiplierId),
                            colorParams.r, colorParams.g, colorParams.b));
                    }
                }
            }
            return res;
        }

        public static string GetImagePathById(int picId)
        {
            using (MonopolyModel model = new MonopolyModel())
            {
                foreach (var pic in model.PictureFiles)
                {
                    if (pic.Id == picId)
                    {
                        return pic.Path;
                    }
                }
            }
            throw new Exception("No image with such Id");
        }

        public static int GetUserIdByLogin(string login)
        {
            using (MonopolyModel model = new MonopolyModel())
            {
                foreach (var user in model.Players)
                {
                    if (user.Login == login)
                    {
                        return user.Id;
                    }
                }
            }
            throw new Exception("No player with such user");
        }

        public static string GetBoxNameByItsDropItemName(string boxItemNmae)
        {
            using (MonopolyModel model = new MonopolyModel())
            {
                foreach (var item in model.BoxItems)
                {
                    if (item.Name == boxItemNmae)
                    {
                        return GetBoxNameByBoxId((int)item.BoxId);
                    }
                }
            }
            throw new Exception("No item with such item name");
        }

        private static string GetBoxNameByBoxId(int boxId)
        {
            using (MonopolyModel model = new MonopolyModel())
            {
                foreach (var box in model.LotBoxes)
                {
                    if (box.Id == boxId)
                    {
                        return box.Name;
                    }
                }
            }

            throw new Exception("no box with such id");
        }

        public static bool IfInventoryStaffIsEnabled(int id)
        {
            using (MonopolyModel model = new MonopolyModel())
            {
                return model.InventoryStaffs.Where(x => x.Id == id && x.IfEnabled != null && (bool)x.IfEnabled).Any();
            }
        }

        public static bool IfUserExistByLogin(string login)
        {
            using (MonopolyModel model = new MonopolyModel())
            {
                return model.Players.Any(x => x.Login == login);
            }
        }

        public static void AddNewUserInDB(User user)
        {
            using (MonopolyModel model = new MonopolyModel())
            {
                model.Players.Add(new Player()
                {
                    Login = user.Login,
                    Password = user.Password,
                });

                model.SaveChanges();
            }
        }

        public static List<User> GetAllPlayers()
        {
            List<User> res = new List<User>();

            using(MonopolyModel model = new MonopolyModel())
            {
                foreach(var user in model.Players)
                {
                    User addUser = new User(user.Login, user.Id);
                    res.Add(addUser);
                }
            }
            return res;
        }
    }
}
