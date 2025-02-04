using MonopolyDLL.Monopoly.InventoryObjs;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace MonopolyDLL.Monopoly
{
    public class MonopolySystem
    {
        public User LoggedUser { get; set; }
        public Game MonGame { get; set; }
        public UserInventory UserInventory { get; set; }

        public MonopolySystem()
        {
            LoggedUser = new User();
            MonGame = new Game();

            //UserInventory = GetInventoryItems();
        }

   /*     public UserInventory GetInventoryItems()
        {
            List<InventoryObjs.Item> inventoryItems = new List<InventoryObjs.Item>();

            //Getting lotbox items
            inventoryItems.AddRange(GetLotBoxesForInventory());

            //Getting station items
            inventoryItems.AddRange(GetItemsFromItems());

            return new UserInventory(inventoryItems);
        }

        public List<InventoryObjs.Item> GetItemsFromItems()
        {
            List<InventoryObjs.Item> res = new List<InventoryObjs.Item>();

            using (DBNotFiltered.MonopolyModel model = new DBNotFiltered.MonopolyModel())
            {
                List<string> names = new List<string>();
                foreach (var item in model.Items)
                {
                    if (!(bool)item.IsBox)
                    {
                        res.Add(GetBoxItemById((int)item.Id));
                    }
                    names.Add(item.BoxId.ToString());
                }
            }
            return res;
        }

        public InventoryObjs.BoxItem GetBoxItemById(int id)
        {
            using (DBNotFiltered.MonopolyModel model = new DBNotFiltered.MonopolyModel())
            {
                foreach (var item in model.BoxItems)
                {
                    if (item.Id == id)
                    {
                        return new InventoryObjs.BoxItem(item.Name, GetImagePathById((int)item.PicId),
                            GetRearityById(id), (Enums.BusinessType)item.StationTypeId, (int)item.StationId, (int)item.MultiplierId);

                    }
                }
            }
            throw new Exception("No such boxItem");
        }

        public Enums.BusinessType GetBusTypeByid(int id)
        {
            return (Enums.BusinessType)id;
        }

        public Enums.BusRearity GetRearityById(int id)
        {
            using (DBNotFiltered.MonopolyModel model = new DBNotFiltered.MonopolyModel())
            {
                foreach (var rearity in model.Rearities)
                {
                    if (rearity.Id == id)
                    {
                        return (Enums.BusRearity)id;
                    }
                }
            }
            throw new Exception("no such Rearity");
        }

        public List<CaseBox> GetLotBoxesForInventory()
        {
            List<CaseBox> res = new List<CaseBox>();
            using (DBNotFiltered.MonopolyModel model = new DBNotFiltered.MonopolyModel())
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

        public CaseBox GetBoxById(int boxId)
        {
            using (DBNotFiltered.MonopolyModel model = new DBNotFiltered.MonopolyModel())
            {
                foreach (var dbBox in model.LotBoxes)
                {
                    if (dbBox.Id == boxId)
                    {
                        return new CaseBox(dbBox.Name, GetImagePathById((int)dbBox.PicId));
                    }
                }
            }
            throw new Exception("No box with such id");
        }

        public string GetImagePathById(int picId)
        {
            using (DBNotFiltered.MonopolyModel model = new DBNotFiltered.MonopolyModel())
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
*/

    }
}
