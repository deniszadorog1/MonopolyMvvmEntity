using MonopolyDLL.DBService;
using MonopolyDLL.Monopoly;
using MonopolyDLL.Monopoly.Cell;
using MonopolyDLL.Monopoly.Enums;
using MonopolyDLL.Monopoly.InventoryObjs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.AccessControl;
using MonopolyDLL.Monopoly.Cell.AngleCells;
using MonopolyDLL.Monopoly.Cell.Bus;
using MonopolyDLL.Monopoly.Cell;


using BoxItem = MonopolyDLL.Monopoly.InventoryObjs.BoxItem;
using Item = MonopolyDLL.Monopoly.InventoryObjs.Item;

namespace MonopolyDLL
{
    public static class DBQueries
    {
        public static List<BoxItem> GetItemsToUseInGame(int playerId)
        {
            List<BoxItem> res = new List<BoxItem>();

            using (MonopolyModel model = new MonopolyModel())
            {
                foreach (var item in model.InventoryStaff)
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
                var item = model.InventoryStaff.FirstOrDefault(x => x.Id == id);
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
                var staff = model.InventoryStaff.FirstOrDefault(s => s.Id == inventoryId);

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
                int id = item.GetInventoryIdInDB();
                var staff = model.InventoryStaff.FirstOrDefault(s => s.Id == id);

                if (staff != null)
                {
                    staff.IfEnabled = false;
                    staff.StationId = null;
                    model.SaveChanges();
                }

                /*     foreach (var staff in model.InventoryStaffs)
                     {
                         if (staff.Id == item.GetInventoryIdInDB())
                         {
                             staff.IfEnabled = false;
                             staff.StationId = null;
                             model.SaveChanges();
                             return;
                         }
                     }*/
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
                        (int r, int g, int b) colorParams = GetColorByRarityId((int)item.RearityId);
                        return new BoxItem(item.Name, GetImagePathById((int)item.PicId),
                            GetRarityById((int)item.RearityId), (BusinessType)item.StationTypeId,
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
                model.InventoryStaff.Add(new InventoryStaff()
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
                foreach (var player in model.Player)
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
                foreach (var player in model.Player)
                {
                    if (player.Id == id)
                    {
                        int? picId = null;
                        if (!(player.PictureFile is null)) picId = player.PictureFile.Id;

                        return new User(player.Login, player.Id, picId, player.Password);
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
                        (int r, int g, int b) colorParams = GetColorByRarityId((int)item.RearityId);
                        return new BoxItem(item.Name, GetImagePathById((int)item.PicId),
                            GetRarityById((int)item.RearityId), (BusinessType)item.StationTypeId,
                            (int)item.StationId, GetMultiplierById((int)item.MultiplierId),
                            colorParams.r, colorParams.g, colorParams.b);

                    }
                }
            }
            throw new Exception("No such boxItem");
        }

        public static (int, int, int) GetColorByRarityId(int rearityId)
        {
            using (MonopolyModel model = new MonopolyModel())
            {
                foreach (var rarity in model.Rarity)
                {
                    if (rarity.Id == rearityId)
                    {
                        return GetColorInSystemColorsById((int)rarity.ColorId);
                    }
                }
            }

            throw new Exception("no Rarity with such Id");
        }

        private static (byte, byte, byte) _usualRarityColorParams = (76, 180, 219);
        public static (byte, byte, byte) GetColorByRarityName(string name)
        {
            using (MonopolyModel model = new MonopolyModel())
            {
                foreach (var rear in model.Rarity)
                {
                    if (rear.Rearity1 == name)
                    {
                        return GetColorInSystemColorsById((int)rear.ColorId);
                    }
                }
            }

            return _usualRarityColorParams;
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
            return _usualRarityColorParams;
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

        public static List<(byte, byte, byte)> GetAllRarityColors()
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
                foreach (var multiplier in model.PriceMultiplier)
                {
                    if (multiplier.Id == id)
                    {
                        return (double)multiplier.Multiplier;
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
                foreach (var item in model.InventoryStaff)
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

        public static BusinessType GetBusTypeById(int id)
        {
            return (BusinessType)id;
        }

        public static BusRarity GetRarityById(int id)
        {
            using (MonopolyModel model = new MonopolyModel())
            {
                foreach (var rarity in model.Rarity)
                {
                    if (rarity.Id == id)
                    {
                        return (BusRarity)id;
                    }
                }
            }
            throw new Exception("no such Rarity");
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
                foreach (var dbBox in model.LotBox)
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
                        (int r, int g, int b) colorParams = GetColorByRarityId((int)item.RearityId);
                        res.Add(new BoxItem(item.Name, GetImagePathById((int)item.PicId),
                            GetRarityById((int)item.RearityId), (BusinessType)item.StationTypeId,
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
                foreach (var pic in model.PictureFile)
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
                foreach (var user in model.Player)
                {
                    if (user.Login == login)
                    {
                        return user.Id;
                    }
                }
            }
            throw new Exception("No player with such user");
        }

        public static string GetBoxNameByItsDropItemName(string boxItemName)
        {
            using (MonopolyModel model = new MonopolyModel())
            {
                foreach (var item in model.BoxItems)
                {
                    if (item.Name == boxItemName)
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
                foreach (var box in model.LotBox)
                {
                    if (box.Id == boxId)
                    {
                        return box.Name;
                    }
                }
            }

            throw new Exception("no box with such id");
        }

        public static bool IsInventoryStaffIsEnabled(int id)
        {
            using (MonopolyModel model = new MonopolyModel())
            {
                return model.InventoryStaff.Where(x => x.Id == id && x.IfEnabled != null && (bool)x.IfEnabled).Any();
            }
        }

        public static bool IsUserExistByLogin(string login)
        {
            using (MonopolyModel model = new MonopolyModel())
            {
                return model.Player.Any(x => x.Login == login);
            }
        }

        public static void AddNewUserInDB(User user)
        {
            using (MonopolyModel model = new MonopolyModel())
            {
                model.Player.Add(new Player()
                {
                    Login = user.Login,
                    Password = user.Password,
                    PictureId = user.GetPictureId()
                });

                model.SaveChanges();
            }
        }

        public static List<User> GetAllPlayers()
        {
            List<User> res = new List<User>();

            using (MonopolyModel model = new MonopolyModel())
            {
                foreach (var user in model.Player)
                {
                    int? picId = null;
                    if (!(user.PictureFile is null)) picId = user.PictureFile.Id;

                    User addUser = new User(user.Login, user.Id, picId, user.Password);

                    addUser.GameBusses = GetItemsToUseInGame(user.Id);

                    res.Add(addUser);
                }
            }
            return res;
        }

        public static void AddPicture(string picPath)
        {
            using (MonopolyModel model = new MonopolyModel())
            {
                model.PictureFile.Add(new PictureFile()
                {
                    Name = picPath,
                    Path = picPath
                });
                model.SaveChanges();
            }
        }

        public static string GetPictureNameById(int? picId)
        {
            if (picId is null) return null;
            using (MonopolyModel model = new MonopolyModel())
            {
                return model.PictureFile.Where(x => x.Id == picId).FirstOrDefault().Path;
            };
        }

        public static int GetLastPicId()
        {
            using (MonopolyModel model = new MonopolyModel())
            {
                return model.PictureFile.OrderByDescending(p => p.Id).FirstOrDefault().Id;
            }
        }

        public static void UpdateUserLogin(string oldOne, string newOne)
        {
            using (MonopolyModel model = new MonopolyModel())
            {
                Player user = model.Player.Where(x => x.Login == oldOne).FirstOrDefault();

                if (user is null) return;
                user.Login = newOne;
                model.SaveChanges();
            }
        }

        public static void UpdateUserPassword(string login, string newPassword)
        {
            using (MonopolyModel model = new MonopolyModel())
            {
                Player user = model.Player.Where(x => x.Login == login).FirstOrDefault();

                if (user is null) return;
                user.Password = newPassword;
                model.SaveChanges();
            }
        }

        public static void SetToPlayerLastPicId(string userLogin)
        {
            using (MonopolyModel model = new MonopolyModel())
            {
                Player user = model.Player.Where(x => x.Login == userLogin).FirstOrDefault();

                if (user is null) return;
                user.PictureId = GetLastPicId();
                model.SaveChanges();
            }
        }

        public static User GetUserByLoginAndPassword(string login, string password)
        {
            using (MonopolyModel model = new MonopolyModel())
            {
                Player player = model.Player.Where(x => x.Login == login && x.Password == password).FirstOrDefault();

                if (!(player is null))
                {
                    return new User(player.Login, player.Id, player.PictureId, player.Password);
                }
            }
            return null;
        }

        public static List<Monopoly.Cell.Cell> GetBasicBoardCells()
        {

            List<Monopoly.Cell.Cell> res = new List<Monopoly.Cell.Cell>();
            using (MonopolyModel model = new MonopolyModel())
            {
                model.Database.ExecuteSqlCommand("CHECKPOINT; DBCC DROPCLEANBUFFERS;");

                var cells = model.Cell.ToList();
                for (int i = 0; i < cells.Count(); i++)
                {
                    Monopoly.Cell.Cell par = GetParentByCell(cells[i]);
                    if (!(par is null))
                    {
                        res.Add(par);
                    }
                }
            }

            return res;
        }

        private static Monopoly.Cell.Cell GetParentByCell(DBService.Cell cell)
        {   
            Monopoly.Enums.CellType? type = GetTypeByCell(cell);
            if (type is null) return null;

            int cellIdForIndex = cell.Id - 1;

            switch ((Monopoly.Enums.CellType)type)
            {
                case Monopoly.Enums.CellType.Start:
                    return new Start(cell.Name, cellIdForIndex);
                case Monopoly.Enums.CellType.UsualBusiness:
                    return GetBusByCell(cell, (Monopoly.Enums.CellType)type);
                case Monopoly.Enums.CellType.CarBusiness:
                    return GetBusByCell(cell, (Monopoly.Enums.CellType)type);
                case Monopoly.Enums.CellType.Prison:
                    return new Prison(cell.Name, cellIdForIndex);
                case Monopoly.Enums.CellType.Chance:
                    return new Monopoly.Cell.Chance(cell.Name, cellIdForIndex);
                case Monopoly.Enums.CellType.GameBusiness:
                    return GetBusByCell(cell, (Monopoly.Enums.CellType)type);
                case Monopoly.Enums.CellType.Casino:
                    return new Monopoly.Cell.AngleCells.Casino(cell.Name, cellIdForIndex);
                case Monopoly.Enums.CellType.GoToPrison:
                    return new GoToPrison(cell.Name, cellIdForIndex);
                case Monopoly.Enums.CellType.Tax:
                    return GetTaxByCell(cell, cellIdForIndex);
            }
            return null;
        }

        private static Monopoly.Cell.Tax GetTaxByCell(DBService.Cell cell, int cellIndex)
        {
            using (MonopolyModel model = new MonopolyModel())
            {
                var taxes = cell.Tax.ToList();
                var taxTypes = model.TaxType.ToList();

                int moneyToPay = (int)taxTypes[taxes.Where(x => x.CellId == cell.Id).ToList().First().Id - 1].TaxMoney;

                return new Monopoly.Cell.Tax(cell.Name, moneyToPay, cellIndex);

            }
        }

        private static Business GetBusByCell(DBService.Cell cell, Monopoly.Enums.CellType busType)
        {
            using (MonopolyModel model = new MonopolyModel())
            {
                var stations = model.Station.ToList();

                for (int i = 0; i < stations.Count; i++)
                {
                    if (cell.Id == stations[i].CellId)
                    {
                        List<int> paymentLevels = GetClearedPaymentLevels(GetPaymentLevels(stations[i]));


                        switch (busType)
                        {
                            case Monopoly.Enums.CellType.UsualBusiness:
                                {
                                    return new RegularBusiness(cell.Name, (int)stations[i].Price, (int)stations[i].DepositPrice, (int)stations[i].RebuyPrice, 
                                        paymentLevels, SystemParamsService.GetNumByName("MaxDepositCounter"), 0, (int)stations[i].UpgradePrice,
                                        SystemParamsService.GetNumByName("NoOwnerIndex"), (BusinessType)stations[i].TypeId, false, i);
                                }
                            case Monopoly.Enums.CellType.CarBusiness:
                                {
                                    return new CarBusiness(cell.Name, (int)stations[i].Price, (int)stations[i].DepositPrice, (int)stations[i].RebuyPrice,
                                        paymentLevels, SystemParamsService.GetNumByName("MaxDepositCounter"), 0,
                                         SystemParamsService.GetNumByName("NoOwnerIndex"), (BusinessType)stations[i].TypeId, false, i);
                                }
                            case Monopoly.Enums.CellType.GameBusiness:
                                {
                                    return new GameBusiness(cell.Name, (int)stations[i].Price, (int)stations[i].DepositPrice, (int)stations[i].RebuyPrice,
                                        paymentLevels, SystemParamsService.GetNumByName("MaxDepositCounter"), 0,
                                         SystemParamsService.GetNumByName("NoOwnerIndex"), (BusinessType)stations[i].TypeId, false, i);
                                }
                        }

                    }

                }

            }
            return null;
        }

        private static List<int> GetClearedPaymentLevels(List<int> levels)
        {
            const int toClear = -1;

            return new List<int>(levels.Where(x => x != toClear));
        }

        private static List<int> GetPaymentLevels(Station station)
        {
            List<int> res = new List<int>();

            using (MonopolyModel model = new MonopolyModel())
            {
                var prices = model.PriceForLevel.ToList();
                PriceForLevel price = prices[station.Id - 1];

                res.Add((int)price.FirstLevel);
                res.Add((int)price.SecondLevel);
                res.Add((int)price.ThirdLevel);
                res.Add((int)price.FourthLevel);
                res.Add((int)price.FifthLevel);
                res.Add((int)price.SixthLevel);
            }
            return res;
        }

        private static Monopoly.Enums.CellType? GetTypeByCell(DBService.Cell cell)
        {
            for (int i = 1; i <= (int)Monopoly.Enums.CellType.Tax; i++)
            {
                if (((Monopoly.Enums.CellType)i).ToString() ==
                    GetCellTypeInStringById((int)cell.CellType))
                {
                    return (Monopoly.Enums.CellType)i;
                }
            }

            return null;
        }

        private static string GetCellTypeInStringById(int id)
        {
            string res = string.Empty;
            using (MonopolyModel model = new MonopolyModel())
            {
                var types = model.CellType.ToList();
                res =  types[id - 1].Name.ToString();

                return res;
            }
        }

    }
}
