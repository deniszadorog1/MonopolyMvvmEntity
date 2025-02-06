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

namespace MonopolyDLL
{
    public static class DBQueries
    {
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

        public static (int, int, int) GetColorInSystemColorsById(int id)
        {
            using (MonopolyModel model = new MonopolyModel())
            {
                foreach (var color in model.SystemColors)
                {
                    if (color.Id == id)
                    {
                        return ((int)color.R, (int)color.G, (int)color.B);
                    }
                }
            }

            throw new Exception("no color with such id");
        }

        public static List<(byte, byte, byte)> GetAllRearityColors()
        {
            List<(byte, byte, byte)> res = new List<(byte, byte, byte)>();

            using(MonopolyModel model = new MonopolyModel())
            {
                foreach(var color in model.SystemColors)
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
                        items.Add(GetItemFromItemsTableById((int)item.StaffId));
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
    }
}
