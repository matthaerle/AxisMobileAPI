using System.ServiceModel;
using System.ServiceModel.Web;
using System.Collections.Generic;
using RestWCFServiceLibrary.MethodObject;
using RestWCFServiceLibrary.ObjectViews;

namespace RestWCFServiceLibrary
{
    [ServiceContract]
    public interface IRestWCFServiceLibrary
    {
        //Gets a list of all products
        //Not sure if this will be used as it takes 
        //a little while to run.
        [OperationContract]
        [WebInvoke(
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            UriTemplate = "GetProducts")]
        List<ProductView> GetProducts();

        //Search for products matching UPC
        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,            
            UriTemplate = "GetProductsByUPC")]
        List<ProductView> GetProductByUPC(SearchByUPC upcSearch);

        //Updates min and max of single product
        [OperationContract]
        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "UpdateMinMax")]
        UpdateStatus UpdateMinMax(MinMaxUpdate update);

        //Below gets Inventory Groups and returns info to phone.
        [OperationContract]
        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "GetActiveInventoryGroups")]
        List<SendInventoryGroup> GetActiveInventoryGroups();

        //Verifies product exsists in the group and then returns the product information.
        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "InventoryGroupProductID")]
        ProductView HandleInventoryGroupProductID(GetInventoryGroupProductID getInventoryGroupProductID);

        //Submits a count for a product that has already been verified exsits in the stocktaking group
        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "InsertProductCountInventory")]
        UpdateStatus InsertProductCount(SubmitItemCount submitItemCount);

        //Check if firearm is Disposed or not for firearm inventory
        [OperationContract]
        [WebInvoke(
            Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            UriTemplate = "VerifyFirearmNotDisposed")]
        IsFirearmDisposed VerifyFirearmNotDisposed(FirearmStockScan firearmStock);

        //Update firearm has been counted and already verified if the gun has been disposed or not.
        [OperationContract]
        [WebInvoke(
            Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            UriTemplate = "CountFirearm")]
        UpdateStatus CountFirearm(FirearmStockUpdate firearmStock);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "IsConnected")]
        IsConnected VerifyConnection();

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "GetActiveEmployees")]
        List<GetEmployees> GetActiveEmployees();

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "GetFirearmInformation")]
        FirearmInfo GetFirearmInformation(FirearmStockScan firearmStock);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "GetEmployeeRoles")]
        List<EmployeeRoles> GetEmployeeRoles(CurrentEmployee currentEmployee);
    }
}