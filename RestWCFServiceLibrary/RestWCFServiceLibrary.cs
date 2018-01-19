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
                var inventoryGroupsList = new List<SendInventoryGroup>();
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
                        var inventoryGroups = new SendInventoryGroup
                        {
                            InventoryGroupID = groups.InventoryGroupID,
                            GroupName = groups.GroupName,
                            ProductCount = groups.InventoryGroupProducts.Count
                        };
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
                var products = new List<ProductView>();
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
                        var productView = new ProductView
                        {
                            ProductUPC = prod.Products.ProductUPC,
                            MinLevel = prod.Products.MinLevel,
                            MaxLevel = prod.Products.MaxLevel,
                            ItemNmbr = prod.Products.ItemNmbr,
                            Price = prod.Products.Price,
                            ShortDescription = prod.Products.ShortDescription,
                            PhysicalQoH = prod.Products.PhysicalQoH,
                            QtyOnOrder = prod.Products.QtyOnOrder,
                            QtyCommitted = prod.Products.QtyCommitted,
                            Manufacturer = prod.Products.Manufacturer.Name,
                            Department = prod.Products.Department.Name,
                            ProductID = prod.Products.ProductID,
                            Active = prod.Products.Active,
                            IsStock = prod.Products.IsStockItem,
                            IsFirearm = prod.Products.IsFirearm,
                            IsSerialNonFirearm = prod.s
                        };
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
                var products = new List<ProductView>();
                using (var arsnet = new ARSNETEntities())
                {
                    var product = (from p in arsnet.Products
                                   where p.Active == true
                                   select p);
                    foreach (var prod in product)
                    {
                        var productView = new ProductView
                        {
                            ProductUPC = prod.ProductUPC,
                            MinLevel = prod.MinLevel,
                            MaxLevel = prod.MaxLevel,
                            ItemNmbr = prod.ItemNmbr,
                            Price = prod.Price,
                            ShortDescription = prod.ShortDescription,
                            PhysicalQoH = prod.PhysicalQoH,
                            QtyOnOrder = prod.QtyOnOrder,
                            QtyCommitted = prod.QtyCommitted,
                            Manufacturer = prod.Manufacturer.Name,
                            Department = prod.Department.Name,
                            ProductID = prod.ProductID
                        };

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
                var productView = new ProductView();
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
                    var groupId = submitItemCount.GroupID;
                    var groupProductId = invInfo.InventoryGroupProductID;

                    var invGrpByMach = new InventoryGroupProductByMachine
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
                    var updateStatus = new UpdateStatus { IsSuccesfull = true };
                    return updateStatus;
                }
            } catch (Exception e)
            {
                var updateStatus = new UpdateStatus { IsSuccesfull = false};
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
                    var productID = update.ProductID;
                    var product = (from p in arsnet.Products
                                   where p.ProductID == productID
                                   select p).FirstOrDefault();
                    product.MinLevel = update.MinLevel;
                    product.MaxLevel = update.MaxLevel;
                    product.UpdateUserID = update.EmployeeID;
                    arsnet.SaveChanges();
                    var updateStatus = new UpdateStatus { IsSuccesfull = true};
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
                                var firearmInfo = (from ft in arsnet.FirearmTypes
                                                   where ft.Active == true
                                                   && ft.NFAind == true
                                                    from bb in arsnet.FirearmInventories
                                                   where bb.SerialNumber == firearmStock.SerialNumber
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
                                var firearmInfo = (from ft in arsnet.FirearmTypes
                                                   where ft.Active == true
                                                   && ft.NFAind == false
                                                   from bb in arsnet.FirearmInventories
                                                   where bb.SerialNumber == firearmStock.SerialNumber
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
                    case "NFA":
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
                            UpdateStatus updateStatus = new UpdateStatus { IsSuccesfull = true };
                            return updateStatus;
                        }
                    default:
                        return null;
                }
            } catch (Exception e)
            {
                EventLog.WriteEntry("Axis Mobile API", e.Message + " \n " + e.GetBaseException());
                var updateStatus = new UpdateStatus { IsSuccesfull = false};
                return updateStatus;
            }
        }
        public IsConnected VerifyConnection()
        {
            var isConnected = new IsConnected { ConnectionVerified = true };
            return isConnected;
        }
        public List<GetEmployees> GetActiveEmployees()
        {
            try
            {
                var employees = new List<GetEmployees>();
                using (var arsnet = new ARSNETEntities())
                {
                    var emp = (from e in arsnet.Employees
                               where e.Active == true
                               && e.EmployeeNumber != ""
                               select e);
                    foreach (var emps in emp)
                    {
                        var indEmployee = new GetEmployees
                        {
                            EmployeeID = emps.EmployeeID,
                            FirstName = emps.FirstName,
                            MiddleName = emps.MiddleName,
                            LastName = emps.LastName,
                            EmployeeNumber = emps.EmployeeNumber,
                            PasswordEncrypted = emps.PasswordEncrypted,
                            PasswordHashed = emps.PasswordHashed
                        };
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
                switch (firearmStock.BoundBookType)
                {
                    case "NON-NFA":
                        using (var arsnet = new ARSNETEntities())
                        {
                            FirearmInfo firearmInfoJson;
                            if (firearmStock.LogScanned)
                            {

                                var firearmInfo = (from ft in arsnet.FirearmTypes
                                    where ft.Active == true
                                          && ft.NFAind == false
                                    from bb in arsnet.FirearmInventories
                                    where bb.InvNbr == firearmStock.Log
                                          && bb.IsLatestRevision == true
                                          && bb.Active == true
                                          && bb.TypeOfAction == ft.TypeDesc
                                    orderby bb.FFLID descending
                                    select bb).FirstOrDefault();
                                firearmInfoJson = new FirearmInfo
                                {
                                    InventoryNumber = firearmInfo.InventoryNumber,
                                    Description = firearmInfo.Description,
                                    GaugeCaliber = firearmInfo.GaugeCaliber,
                                    InvNbr = firearmInfo.InvNbr,
                                    Manufacturer = firearmInfo.Manufacturer,
                                    Model = firearmInfo.Model,
                                    NewUsed = firearmInfo.NewUsed,
                                    SerialNumber = firearmInfo.SerialNumber,
                                    Status = firearmInfo.Status,
                                    TypeOfAction = firearmInfo.TypeOfAction,
                                    UPC = firearmInfo.UPC,
                                    Importer = firearmInfo.Importer
                                };

                                return firearmInfoJson;
                            }
                            else if (firearmStock.SerialScanned)
                            {

                                var firearmInfo = (from ft in arsnet.FirearmTypes
                                    where ft.Active == true
                                          && ft.NFAind == false
                                    from bb in arsnet.FirearmInventories
                                    where bb.SerialNumber == firearmStock.SerialNumber
                                          && bb.IsLatestRevision == true
                                          && bb.Active == true
                                          && bb.TypeOfAction == ft.TypeDesc
                                    orderby      bb.FFLID descending 
                                    select bb).FirstOrDefault();
                                firearmInfoJson = new FirearmInfo
                                {
                                    InventoryNumber = firearmInfo.InventoryNumber,
                                    Description = firearmInfo.Description,
                                    GaugeCaliber = firearmInfo.GaugeCaliber,
                                    InvNbr = firearmInfo.InvNbr,
                                    Manufacturer = firearmInfo.Manufacturer,
                                    Model = firearmInfo.Model,
                                    NewUsed = firearmInfo.NewUsed,
                                    SerialNumber = firearmInfo.SerialNumber,
                                    Status = firearmInfo.Status,
                                    TypeOfAction = firearmInfo.TypeOfAction,
                                    UPC = firearmInfo.UPC,
                                    Importer = firearmInfo.Importer
                                };

                                return firearmInfoJson;
                            }
                            return null;
                        }
                    case "NFA":
                        using (var arsnet = new ARSNETEntities())
                        {
                            FirearmInfo firearmInfoJson;
                            if (firearmStock.LogScanned)
                            {

                                var firearmInfo = (from ft in arsnet.FirearmTypes
                                    where ft.Active == true
                                          && ft.NFAind == true
                                    from bb in arsnet.FirearmInventories
                                    where bb.InvNbr == firearmStock.Log
                                          && bb.IsLatestRevision == true
                                          && bb.Active == true
                                          && bb.TypeOfAction == ft.TypeDesc
                                    orderby bb.FFLID descending
                                    select bb).FirstOrDefault();
                                firearmInfoJson = new FirearmInfo
                                {
                                    InventoryNumber = firearmInfo.InventoryNumber,
                                    Description = firearmInfo.Description,
                                    GaugeCaliber = firearmInfo.GaugeCaliber,
                                    InvNbr = firearmInfo.InvNbr,
                                    Manufacturer = firearmInfo.Manufacturer,
                                    Model = firearmInfo.Model,
                                    NewUsed = firearmInfo.NewUsed,
                                    SerialNumber = firearmInfo.SerialNumber,
                                    Status = firearmInfo.Status,
                                    TypeOfAction = firearmInfo.TypeOfAction,
                                    UPC = firearmInfo.UPC,
                                    Importer = firearmInfo.Importer
                                };

                                return firearmInfoJson;
                            }
                            else if (firearmStock.SerialScanned)
                            {

                                var firearmInfo = (from ft in arsnet.FirearmTypes
                                    where ft.Active == true
                                          && ft.NFAind == true
                                    from bb in arsnet.FirearmInventories
                                    where bb.SerialNumber == firearmStock.SerialNumber
                                          && bb.IsLatestRevision == true
                                          && bb.Active == true
                                          && bb.TypeOfAction == ft.TypeDesc
                                    orderby bb.FFLID descending
                                    select bb).FirstOrDefault();
                                firearmInfoJson = new FirearmInfo
                                {
                                    InventoryNumber = firearmInfo.InventoryNumber,
                                    Description = firearmInfo.Description,
                                    GaugeCaliber = firearmInfo.GaugeCaliber,
                                    InvNbr = firearmInfo.InvNbr,
                                    Manufacturer = firearmInfo.Manufacturer,
                                    Model = firearmInfo.Model,
                                    NewUsed = firearmInfo.NewUsed,
                                    SerialNumber = firearmInfo.SerialNumber,
                                    Status = firearmInfo.Status,
                                    TypeOfAction = firearmInfo.TypeOfAction,
                                    UPC = firearmInfo.UPC,
                                    Importer = firearmInfo.Importer
                                };

                                return firearmInfoJson;
                            }
                            return null;
                        }
                    case "GUNSMITHING":
                        using (var arsnet = new ARSNETEntities())
                        {
                            FirearmInfo firearmInfoJson;
                            if (firearmStock.LogScanned)
                            {

                                var firearmInfo = (from ft in arsnet.FirearmTypes
                                    where ft.Active == true
                                          && ft.NFAind == false
                                    from bb in arsnet.GunsmithFirearmInventories
                                    where bb.LogNumber == firearmStock.Log
                                          && bb.IsLatestRevision == true
                                          && bb.Active == true
                                          && bb.TypeOfAction == ft.TypeDesc
                                    orderby bb.FFLID descending
                                    select bb).FirstOrDefault();
                                firearmInfoJson =
                                    new FirearmInfo
                                    {
                                        InventoryNumber = firearmInfo.GunsmithFirearmInventoryID,
                                        Description = "Gunsmithing Firearm",
                                        GaugeCaliber = firearmInfo.GaugeCaliber,
                                        InvNbr = firearmInfo.LogNumber,
                                        Manufacturer = firearmInfo.Manufacturer,
                                        Model = firearmInfo.Model,
                                        NewUsed = "Used",
                                        SerialNumber = firearmInfo.SerialNumber,
                                        Status = firearmInfo.Status,
                                        TypeOfAction = firearmInfo.TypeOfAction,
                                        UPC = "",
                                        Importer = firearmInfo.Importer
                                    };

                                return firearmInfoJson;
                            }
                            else if (firearmStock.SerialScanned)
                            {

                                var firearmInfo = (from ft in arsnet.FirearmTypes
                                    where ft.Active == true
                                          && ft.NFAind == false
                                    from bb in arsnet.GunsmithFirearmInventories
                                    where bb.SerialNumber == firearmStock.SerialNumber
                                          && bb.IsLatestRevision == true
                                          && bb.Active == true
                                          && bb.TypeOfAction == ft.TypeDesc
                                    orderby bb.FFLID descending
                                    select bb).FirstOrDefault();
                                firearmInfoJson =
                                    new FirearmInfo
                                    {
                                        InventoryNumber = firearmInfo.GunsmithFirearmInventoryID,
                                        Description = "Gunsmithing Firearm",
                                        GaugeCaliber = firearmInfo.GaugeCaliber,
                                        InvNbr = firearmInfo.LogNumber,
                                        Manufacturer = firearmInfo.Manufacturer,
                                        Model = firearmInfo.Model,
                                        NewUsed = "Used",
                                        SerialNumber = firearmInfo.SerialNumber,
                                        Status = firearmInfo.Status,
                                        TypeOfAction = firearmInfo.TypeOfAction,
                                        UPC = "",
                                        Importer = firearmInfo.Importer
                                    };

                                return firearmInfoJson;
                            }
                            return null;
                        }
                    default:
                        return null;
                }
                
                
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("Axis Mobile API", e.Message + " \n " + e.GetBaseException());
                return null;
            }
        }

        public List<EmployeeRoles> GetEmployeeRoles(CurrentEmployee currentEmployee)
        {
            try
            {
                var roles = new List<EmployeeRoles>();
                using (var arsnet = new ARSNETEntities())
                {
                    var role = (from ri in arsnet.EmployeeXRoles
                        where ri.EmployeeID == currentEmployee.EmployeeID
                        select ri);
                    foreach (var rls in role)
                    {
                        var employeeRoles = new EmployeeRoles
                        {
                            RoleID = rls.Role.RoleID,
                            RoleDiscription = rls.Role.RoleDescription
                        };
                        roles.Add(employeeRoles);
                    }
                }
                return roles;
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("Axis Mobile API", e.Message + " \n " + e.GetBaseException());
                return null;
            }
        }
    }
}