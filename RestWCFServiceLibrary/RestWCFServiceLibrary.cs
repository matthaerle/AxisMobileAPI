using System;
using System.Collections.Generic;
using System.Linq;
using RestWCFServiceLibrary.MethodObject;
using RestWCFServiceLibrary.Entities;
using System.Diagnostics;
using RestWCFServiceLibrary.ObjectViews;

namespace RestWCFServiceLibrary
{
    public class RestWCFServiceLibrary : IRestWCFServiceLibrary
    {
        public List<SendInventoryGroup> GetActiveInventoryGroups()
        {
            try
            {
                List<SendInventoryGroup> inventoryGroupsList = new List<SendInventoryGroup>();
                using (var arsnet = new ARSNETEntities())
                {
                    /*var invGroups = (from i in arsnet.InventoryGroups
                                     join prod in arsnet.InventoryGroupProducts on i.InventoryGroupID equals prod.InventoryGroupID
                                     where i.IsStockTakingComplete == false
                                     select i);
                                     */
                    var invGroups = (from i in arsnet.InventoryGroups
                                     where i.IsStockTakingComplete == false
                                     select i);
                    foreach (var groups in invGroups)
                    {
                        SendInventoryGroup inventoryGroups = new SendInventoryGroup();
                        inventoryGroups.InventoryGroupID = groups.InventoryGroupID;
                        inventoryGroups.GroupName = groups.GroupName;
                        inventoryGroups.ProductCount = groups.InventoryGroupProducts.Count;
                        inventoryGroupsList.Add(inventoryGroups);
                    }
                                 }
                return inventoryGroupsList;
            } catch (Exception e)
            {
                EventLog.WriteEntry("Axis Mobile API", e.Message + " \n " + e.GetBaseException());
                return null;
            }
        }

        public List<ProductView> GetProductByUPC(SearchByUPC upcSearch)
        {
            try
            {
                List<ProductView> products = new List<ProductView>();
                using (var arsnet = new ARSNETEntities())
                {
                    //var product = from p in arsnet.Products
                    //               join data in arsnet.SerialUPCs
                    //               on p.ProductID equals data.ProductID into joined
                    //               from j in joined.DefaultIfEmpty()
                    //               where p.ProductUPC == upcSearch.ProductUPC
                    //               select  new
                    //               {   

                    //                   Products = p,
                    //                   j?.Active ?? bool.FalseString : j.Active
                    //               };

                    var productA = (from Products in arsnet.Products
                                    let s = arsnet.SerialUPCs.Any(x => x.Active && x.ProductID == Products.ProductID)
                                    where Products.ProductUPC == upcSearch.ProductUPC
                                    select new
                                    {
                                        Products, s

                                    });
                    foreach (var prod in productA)
                    {
                        ProductView productView = new ProductView();
                        productView.ProductUPC = prod.Products.ProductUPC;
                        productView.MinLevel = prod.Products.MinLevel;
                        productView.MaxLevel = prod.Products.MaxLevel;
                        productView.ItemNmbr = prod.Products.ItemNmbr;
                        productView.Price = prod.Products.Price;
                        productView.ShortDescription = prod.Products.ShortDescription;
                        productView.PhysicalQoH = prod.Products.PhysicalQoH;
                        productView.QtyOnOrder = prod.Products.QtyOnOrder;
                        productView.QtyCommitted = prod.Products.QtyCommitted;
                        productView.Manufacturer = prod.Products.Manufacturer.Name;
                        productView.Department = prod.Products.Department.Name;
                        productView.ProductID = prod.Products.ProductID;
                        productView.Active = prod.Products.Active;
                        productView.IsStock = prod.Products.IsStockItem;
                        productView.IsFirearm = prod.Products.IsFirearm;
                        productView.IsSerialNonFirearm = prod.s;
                        products.Add(productView);
                    }

                }
                return products;
            } catch (Exception e)
            {
                EventLog.WriteEntry("Axis Mobile API", e.Message + " \n " + e.GetBaseException());
                return null;
            }
        }
        public List<ProductView> GetProducts()
        {
            try
            {
                List<ProductView> products = new List<ProductView>();
                using (var arsnet = new ARSNETEntities())
                {
                    var product = (from p in arsnet.Products
                                   where p.Active == true
                                   select p);
                    foreach (var prod in product)
                    {
                        ProductView productView = new ProductView();
                        productView.ProductUPC = prod.ProductUPC;
                        productView.MinLevel = prod.MinLevel;
                        productView.MaxLevel = prod.MaxLevel;
                        productView.ItemNmbr = prod.ItemNmbr;
                        productView.Price = prod.Price;
                        productView.ShortDescription = prod.ShortDescription;
                        productView.PhysicalQoH = prod.PhysicalQoH;
                        productView.QtyOnOrder = prod.QtyOnOrder;
                        productView.QtyCommitted = prod.QtyCommitted;
                        productView.Manufacturer = prod.Manufacturer.Name;
                        productView.Department = prod.Department.Name;
                        productView.ProductID = prod.ProductID;

                        products.Add(productView);
                    }

                }
                return products;
            } catch (Exception e)
            {
                EventLog.WriteEntry("Axis Mobile API", e.Message + " In Catch " + e.GetBaseException());
                return null;
            }
        }
        public ProductView HandleInventoryGroupProductID(GetInventoryGroupProductID getInventoryGroupProductID)
        {
            try
            {
                ProductView productView = new ProductView();
                using (var arsnet = new ARSNETEntities())
                {
                    var groupProduct = (from gp in arsnet.InventoryGroupProducts
                                        where gp.Product.ProductUPC == getInventoryGroupProductID.ProductUPC
                                        && gp.InventoryGroupID == getInventoryGroupProductID.GroupID
                                        select gp).SingleOrDefault();
                    
                    productView.ProductID = groupProduct.Product.ProductID;
                    productView.ProductUPC = groupProduct.Product.ProductUPC;
                    productView.QtyCommitted = groupProduct.Product.QtyCommitted;
                    productView.QtyOnOrder = groupProduct.Product.QtyOnOrder;
                    productView.ShortDescription = groupProduct.Product.ShortDescription;
                    productView.Price = groupProduct.Product.Price;
                    productView.PhysicalQoH = groupProduct.Product.PhysicalQoH;
                    productView.MinLevel = groupProduct.Product.MinLevel;
                    productView.MaxLevel = groupProduct.Product.MaxLevel;
                    productView.Manufacturer = groupProduct.Product.Manufacturer.Name;
                    productView.ItemNmbr = groupProduct.Product.ItemNmbr;
                    productView.Department = groupProduct.Product.Department.Name;
                }
                return productView;
            } catch (Exception e)
            {
                EventLog.WriteEntry("Axis Mobile API", e.Message + " \n " + e.GetBaseException());
                return null;
            }
            
        }
        public UpdateStatus InsertProductCount(SubmitItemCount submitItemCount)
        {
            try
            {
                
                using (var arsnet = new ARSNETEntities())
                {
                    var invInfo = (from inv in arsnet.InventoryGroupProducts
                                   where inv.Product.ProductUPC == submitItemCount.ProductUPC
                                   && inv.InventoryGroupID == submitItemCount.GroupID
                                   select inv).SingleOrDefault();
                    int groupId = submitItemCount.GroupID;
                    Int64 groupProductId = invInfo.InventoryGroupProductID;

                    InventoryGroupProductByMachine invGrpByMach = new InventoryGroupProductByMachine
                    {
                        InventoryGroupProductID = invInfo.InventoryGroupProductID,
                        Count = submitItemCount.CountQty,
                        //Machine name might get changed to get info from Android but for now its hardcoded
                        MachineName = "Axis Mobile",
                        UpdatedDate = DateTime.Now,
                        UpdatedEmployeeId = submitItemCount.EmployeeID
                    };
                    arsnet.InventoryGroupProductByMachines.Add(invGrpByMach);
                    arsnet.SaveChanges();
                    UpdateStatus updateStatus = new UpdateStatus { IsSuccesfull = true };
                    return updateStatus;
                }
            } catch (Exception e)
            {
                UpdateStatus updateStatus = new UpdateStatus { IsSuccesfull = false};
                EventLog.WriteEntry("Axis Mobile API", e.Message + " \n " + e.GetBaseException());
                return updateStatus;
            }
        }
        public UpdateStatus UpdateMinMax(MinMaxUpdate update)
        {
            
            try
            {
                //EventLog.WriteEntry("Axis Mobile API", update.ProductID.ToString());
                using (var arsnet = new ARSNETEntities())
                {
                    Int64 productID = update.ProductID;
                    var product = (from p in arsnet.Products
                                   where p.ProductID == productID
                                   select p).FirstOrDefault();
                    product.MinLevel = update.MinLevel;
                    product.MaxLevel = update.MaxLevel;
                    product.UpdateUserID = update.EmployeeID;
                    arsnet.SaveChanges();
                    UpdateStatus updateStatus = new UpdateStatus { IsSuccesfull = true};
                    return updateStatus;
                }
            } catch (Exception e)
            {
                EventLog.WriteEntry("Axis Mobile API",e.Message + " In Catch " + e.GetBaseException() );
                UpdateStatus updateStatus = new UpdateStatus { IsSuccesfull = false};
                return updateStatus;
            }
        }
        public IsFirearmDisposed VerifyFirearmNotDisposed(FirearmStockScan firearmStock)
        {
            
            try
            {
                switch (firearmStock.BoundBookType)
                {
                    
                    
                    case ("NFA"):
                        using (var arsnet = new ARSNETEntities())
                        {
                            IsFirearmDisposed isFirearmDisposed;
                            if (firearmStock.LogScanned)
                            {
                                isFirearmDisposed = new IsFirearmDisposed();
                                var firearmInfo = (from ft in arsnet.FirearmTypes
                                                   where ft.Active == true
                                                   && ft.NFAind == true
                                                   from bb in arsnet.FirearmInventories
                                                   where bb.InvNbr == firearmStock.Log
                                                   && bb.IsLatestRevision == true
                                                   && bb.Active == true
                                                   && bb.TypeOfAction == ft.TypeDesc
                                                   select bb).SingleOrDefault();
                                if (firearmInfo.Status == "I")
                                {
                                    isFirearmDisposed.Disposed = false;
                                    isFirearmDisposed.InventoryNumber = firearmInfo.InventoryNumber;
                                }
                                else
                                {
                                    isFirearmDisposed.Disposed = true;
                                    isFirearmDisposed.InventoryNumber = firearmInfo.InventoryNumber;
                                }
                                return isFirearmDisposed;
                            }
                            else if (firearmStock.SerialScanned)
                            {
                                isFirearmDisposed = new IsFirearmDisposed();
                                var firearmInfo = (from bb in arsnet.FirearmInventories
                                                   where bb.SerialNumber == firearmStock.SerialNumber
                                                   && bb.IsLatestRevision == true
                                                   && bb.Active == true
                                                   select bb).SingleOrDefault();
                                if (firearmInfo.Status == "I")
                                {
                                    isFirearmDisposed.Disposed = false;
                                    isFirearmDisposed.InventoryNumber = firearmInfo.InventoryNumber;
                                }
                                else
                                {
                                    isFirearmDisposed.Disposed = true;
                                    isFirearmDisposed.InventoryNumber = firearmInfo.InventoryNumber;
                                }
                                return isFirearmDisposed;
                            }
                            return null;
                        }
                    case ("NON-NFA"):
                        using (var arsnet = new ARSNETEntities())
                        {
                            IsFirearmDisposed isFirearmDisposed;
                            if (firearmStock.LogScanned)
                            {
                                isFirearmDisposed = new IsFirearmDisposed();
                                var firearmInfo = ( from ft in arsnet.FirearmTypes
                                                    where ft.Active == true
                                                    && ft.NFAind == false
                                                    from bb in arsnet.FirearmInventories
                                                    where bb.InvNbr == firearmStock.Log
                                                    && bb.IsLatestRevision == true
                                                    && bb.Active == true
                                                    && bb.TypeOfAction == ft.TypeDesc
                                                    select bb).SingleOrDefault();
                                if (firearmInfo.Status == "I")
                                {
                                    isFirearmDisposed.Disposed = false;
                                    isFirearmDisposed.InventoryNumber = firearmInfo.InventoryNumber;
                                }
                                else
                                {
                                    isFirearmDisposed.Disposed = true;
                                    isFirearmDisposed.InventoryNumber = firearmInfo.InventoryNumber;
                                }
                                return isFirearmDisposed;
                            }
                            else if (firearmStock.SerialScanned)
                            {
                                isFirearmDisposed = new IsFirearmDisposed();
                                var firearmInfo = (from bb in arsnet.FirearmInventories
                                                   where bb.SerialNumber == firearmStock.SerialNumber
                                                   && bb.IsLatestRevision == true
                                                   && bb.Active == true
                                                   select bb).SingleOrDefault();
                                if (firearmInfo.Status == "I")
                                {
                                    isFirearmDisposed.Disposed = false;
                                    isFirearmDisposed.InventoryNumber = firearmInfo.InventoryNumber;
                                }
                                else
                                {
                                    isFirearmDisposed.Disposed = true;
                                    isFirearmDisposed.InventoryNumber = firearmInfo.InventoryNumber;
                                }
                                return isFirearmDisposed;
                            }
                            return null;
                        }
                    case ("GUNSMITHING"):
                        
                        using (var arsnet = new ARSNETEntities())
                        {
                            IsFirearmDisposed isFirearmDisposed;
                            if (firearmStock.LogScanned)
                            {
                                isFirearmDisposed = new IsFirearmDisposed();
                                var firearmInfo = (from bb in arsnet.GunsmithFirearmInventories
                                                   where bb.LogNumber == firearmStock.Log
                                                   && bb.IsLatestRevision == true
                                                   && bb.Active == true
                                                   select bb).SingleOrDefault();
                                if (firearmInfo.Status == "O")
                                {
                                    isFirearmDisposed.Disposed = true;
                                    isFirearmDisposed.InventoryNumber = firearmInfo.GunsmithFirearmInventoryID;
                                }
                                else
                                {
                                    isFirearmDisposed.Disposed = false;
                                    isFirearmDisposed.InventoryNumber = firearmInfo.GunsmithFirearmInventoryID;
                                }
                                return isFirearmDisposed;
                            }
                            else if (firearmStock.SerialScanned)
                            {
                                isFirearmDisposed = new IsFirearmDisposed();
                                var firearmInfo = (from bb in arsnet.GunsmithFirearmInventories
                                                   where bb.LogNumber == firearmStock.Log
                                                   && bb.IsLatestRevision == true
                                                   && bb.Active == true
                                                   select bb).SingleOrDefault();
                                if (firearmInfo.Status == "O")
                                {
                                    isFirearmDisposed.Disposed = true;
                                    isFirearmDisposed.InventoryNumber = firearmInfo.GunsmithFirearmInventoryID;
                                }
                                else
                                {
                                    isFirearmDisposed.Disposed = false;
                                    isFirearmDisposed.InventoryNumber = firearmInfo.GunsmithFirearmInventoryID;
                                }
                                return isFirearmDisposed;
                            }
                            return null;
                        }
                    default:
                        return null;
                }
            } catch (Exception e)
            {
                EventLog.WriteEntry("Axis Mobile API", e.Message + " \n " + e.GetBaseException());
                return null; 
            }
        }
        public UpdateStatus CountFirearm(FirearmStockUpdate firearmStock)
        {
            try
            {
                switch (firearmStock.BoundBookType)
                {
                    case "NON-NFA":
                    case "NFA-NFA":
                        using (var arsnet = new ARSNETEntities())
                        {
                            //Getting CompanyID and StoreID
                            var storeInfo = (from str in arsnet.CompanyStores
                                             select new { str.CompanyID, str.CompanyStoreID }).FirstOrDefault();
                            //Getting firearm record that last count date is going to be updated.
                            var firearm = (from firearmTbl in arsnet.FirearmInventories
                                           where firearmTbl.InventoryNumber == firearmStock.InventoryNumber
                                           select firearmTbl).FirstOrDefault();
                            firearm.LastScanDateTime = DateTime.Today;
                            firearm.UpdateUserID = firearmStock.EmployeeID;
                            firearm.UpdateDateTime = DateTime.Now;
                            //Hardcoding machine name
                            firearm.UpdateMachineName = firearmStock.MachineName;
                            arsnet.SaveChanges();

                            FirearmCount fCountInsert = new FirearmCount
                            {
                                CompanyID = storeInfo.CompanyID,
                                StoreID = storeInfo.CompanyStoreID,
                                InventoryNumber = firearmStock.InventoryNumber,
                                CountDateTime = DateTime.Now,
                                UpdateUserID = firearmStock.EmployeeID,
                                UpdateDateTime = DateTime.Now,
                                //Hardcoded machine name
                                UpdateMachineName = firearmStock.MachineName,
                                Active = true
                            };
                            arsnet.FirearmCounts.Add(fCountInsert);
                            arsnet.SaveChanges();
                            UpdateStatus updateStatus = new UpdateStatus { IsSuccesfull = true };
                            return updateStatus;
                        }
                    case "GUNSMITHING":
                        using (var arsnet = new ARSNETEntities())
                        {
                            var storeInfo = (from str in arsnet.CompanyStores
                                             select new { str.CompanyID, str.CompanyStoreID }).FirstOrDefault();
                            var firearm = (from firearmTbl in arsnet.GunsmithFirearmInventories
                                           where firearmTbl.GunsmithFirearmInventoryID == firearmStock.InventoryNumber
                                           select firearmTbl).FirstOrDefault();
                            firearm.LastScanDateTime = DateTime.Today;
                            firearm.UpdateUserID = firearmStock.EmployeeID;
                            firearm.UpdateDateTime = DateTime.Now;
                            //Hardcoding machine name
                            firearm.UpdateMachineName = firearmStock.MachineName;
                            arsnet.SaveChanges();

                        }

                            break;
                }
                
            } catch (Exception e)
            {
                EventLog.WriteEntry("Axis Mobile API", e.Message + " \n " + e.GetBaseException());
                UpdateStatus updateStatus = new UpdateStatus { IsSuccesfull = false};
                return updateStatus;
            }
        }
        public IsConnected VerifyConnection()
        {
            IsConnected isConnected = new IsConnected { ConnectionVerified = true };
            return isConnected;
        }
        public List<GetEmployees> GetActiveEmployees()
        {
            try
            {
                List<GetEmployees> employees = new List<GetEmployees>();
                using (var arsnet = new ARSNETEntities())
                {
                    var emp = (from e in arsnet.Employees
                               where e.Active == true
                               && e.EmployeeNumber != ""
                               select e);
                    foreach (var emps in emp)
                    {
                        GetEmployees indEmployee = new GetEmployees();
                        indEmployee.EmployeeID = emps.EmployeeID;
                        indEmployee.FirstName = emps.FirstName;
                        indEmployee.MiddleName = emps.MiddleName;
                        indEmployee.LastName = emps.LastName;
                        indEmployee.EmployeeNumber = emps.EmployeeNumber;
                        indEmployee.PasswordEncrypted = emps.PasswordEncrypted;
                        indEmployee.PasswordHashed = emps.PasswordHashed;
                        employees.Add(indEmployee);
                    }
                }
                return employees;
            } catch (Exception e)
            {
                EventLog.WriteEntry("Axis Mobile API", e.Message + " \n " + e.GetBaseException());
                return null;
            }
        }

        public FirearmInfo GetFirearmInformation(FirearmStockScan firearmStock)
        {
            try
            {
                FirearmInfo firearmInfoJson = new FirearmInfo();
                using (var arsnet = new ARSNETEntities())
                {
                    if (firearmStock.LogScanned)
                    {
                        var firearmInfo = (from bb in arsnet.FirearmInventories
                                           where bb.InvNbr == firearmStock.Log
                                           && bb.IsLatestRevision == true
                                           && bb.Active == true
                                           select bb).SingleOrDefault();
                        firearmInfoJson.InventoryNumber = firearmInfo.InventoryNumber;
                        firearmInfoJson.Description = firearmInfo.Description;
                        firearmInfoJson.GaugeCaliber = firearmInfo.GaugeCaliber;
                        firearmInfoJson.InvNbr = firearmInfo.InvNbr;
                        firearmInfoJson.Manufacturer = firearmInfo.Manufacturer;
                        firearmInfoJson.Model = firearmInfo.Model;
                        firearmInfoJson.NewUsed = firearmInfo.NewUsed;
                        firearmInfoJson.SerialNumber = firearmInfo.SerialNumber;
                        firearmInfoJson.Status = firearmInfo.Status;
                        firearmInfoJson.TypeOfAction = firearmInfo.TypeOfAction;
                        firearmInfoJson.UPC = firearmInfo.UPC;
                        firearmInfoJson.Importer = firearmInfo.Importer;

                    }
                    else if (firearmStock.SerialScanned)
                    {
                        var firearmInfo = (from bb in arsnet.FirearmInventories
                                           where bb.SerialNumber == firearmStock.SerialNumber
                                           && bb.IsLatestRevision == true
                                           && bb.Active == true
                                           select bb).SingleOrDefault();
                        firearmInfoJson.InventoryNumber = firearmInfo.InventoryNumber;
                        firearmInfoJson.Description = firearmInfo.Description;
                        firearmInfoJson.GaugeCaliber = firearmInfo.GaugeCaliber;
                        firearmInfoJson.InvNbr = firearmInfo.InvNbr;
                        firearmInfoJson.Manufacturer = firearmInfo.Manufacturer;
                        firearmInfoJson.Model = firearmInfo.Model;
                        firearmInfoJson.NewUsed = firearmInfo.NewUsed;
                        firearmInfoJson.SerialNumber = firearmInfo.SerialNumber;
                        firearmInfoJson.Status = firearmInfo.Status;
                        firearmInfoJson.TypeOfAction = firearmInfo.TypeOfAction;
                        firearmInfoJson.UPC = firearmInfo.UPC;
                        firearmInfoJson.Importer = firearmInfo.Importer;
                    }

                }
                return firearmInfoJson;
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("Axis Mobile API", e.Message + " \n " + e.GetBaseException());
                return null;
            }
        }
    }
}