using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using AuthorizeNet;
using AuthorizeNet.APICore;

namespace HealthyChef.AuthNet
{
    public class CustomerInformationManager
    {
        protected AuthNetConfig anConfig { get; private set; }
        protected CustomerGateway Gateway { get; private set; }

        public CustomerInformationManager()
        {
            anConfig = new AuthNetConfig();
            Gateway = new CustomerGateway(anConfig.Settings.ApiKey, anConfig.Settings.TransactionKey, anConfig.Settings.TestMode? ServiceMode.Test : ServiceMode.Live);
        }

        public Customer GetCustomer(string profileId)
        {
            return Gateway.GetCustomer(profileId);
        }

        public string[] GetCustomerIDs(out string error)
        {
            return Gateway.GetCustomerIDs(out error);
        }

        public validateCustomerPaymentProfileResponse ValidateProfile(string profileId, string paymentProfileId, ValidationMode mode)
        {
            return Gateway.HCCValidateProfile(profileId, paymentProfileId, mode);
        }

        public Customer CreateCustomer(string email, string description)
        {
            Customer customer = null;
            try
            {
                customer = Gateway.CreateCustomer(email, description);
                return customer;
            }
            catch
            {
                throw;
            }
            finally
            {
                OnCreateCustomer(customer);
            }
        }

        public string AddCreditCard(Customer customer, string cardNumber, int expirationMonth, int expirationYear, string cvv2, Address address)
        {
            string addCardResult = string.Empty;
            try
            {
                addCardResult = Gateway.AddCreditCard(customer.ProfileID, cardNumber, expirationMonth, expirationYear, cvv2, address);
                return addCardResult;
            }
            catch
            {
                throw;
            }
            finally
            {
                OnAddCard(addCardResult);
            }
        }

        public string AddShippingAddress(Customer customer, Address address)
        {
            if (customer == null)
            {
                throw new ArgumentNullException("customer", ErrorMessages.CustomerNullErrorCode + ErrorMessages.CustomerNullErrorMessage);
            }

            if (address == null)
            {
                throw new ArgumentNullException("address", ErrorMessages.AddressNullErrorCode + ErrorMessages.AddressNullErrorMessage);
            }

            string addAddressResult = string.Empty;

            try
            {
                addAddressResult = Gateway.AddShippingAddress(customer.ProfileID, address);
                return addAddressResult;
            }
            catch
            {
                throw;
            }
            finally
            {
                OnAddShippingAddress(customer, address, addAddressResult);
            }
        }

        public IGatewayResponse Authorize(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException("order", ErrorMessages.OrderNullErrorCode + ErrorMessages.OrderNullErrorMessage);
            }

            IGatewayResponse response = null;
            try
            {
                response = Gateway.Authorize(order);
                return response;
            }
            catch
            {
                throw;
            }
            finally
            {
                OnAuthorize(response);
            }
        }

        public IGatewayResponse AuthorizeAndCapture(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException("order", ErrorMessages.OrderNullErrorCode + ErrorMessages.OrderNullErrorMessage);
            }

            IGatewayResponse response = null;
            try
            {
                response = Gateway.AuthorizeAndCapture(order);
                return response;
            }
            catch
            {
                throw;
            }
            finally
            {
                OnAuthorizeAndCapture(response);
            }
        }

        public IGatewayResponse Refund(Customer customer, string paymentProfileId, string approvalCode, string transactionId, decimal amount)
        {
            if (string.IsNullOrWhiteSpace(paymentProfileId))
            {
                throw new ArgumentNullException("paymentProfileId", ErrorMessages.PaymentProfileIdNullErrorCode + ErrorMessages.PaymentProfileIdNullErrorMessage);
            }

            if (string.IsNullOrWhiteSpace(approvalCode))
            {
                throw new ArgumentNullException("approvalCode", ErrorMessages.ApprovalCodeNullErrorCode + ErrorMessages.ApprovalCodeNullErrorMessage);
            }

            if (string.IsNullOrWhiteSpace(transactionId))
            {
                throw new ArgumentNullException("transactionId", ErrorMessages.TransactionIdNullErrorCode + ErrorMessages.TransactionIdNullErrorMessage);
            }

            if (amount == decimal.Zero)
            {
                throw new ArgumentException(ErrorMessages.AmountZeroErrorCode + ErrorMessages.AmountZeroErrorMessage, "amount");
            }

            IGatewayResponse response = null;

            try
            {
                response = Gateway.Refund(customer.ProfileID, paymentProfileId, approvalCode, transactionId, amount);
                return response;
            }
            catch
            {
                throw;
            }
            finally
            {
                OnRefund(response);
            }
        }

        public bool UpdateCustomer(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException("customer", ErrorMessages.CustomerNullErrorCode + ErrorMessages.CustomerNullErrorMessage);
            }

            bool result = false;
            try
            {
                result = Gateway.UpdateCustomer(customer);
                return result;
            }
            catch
            {
                throw;
            }
            finally
            {
                OnUpdateCustomer(result, customer);
            }
        }

        /// <summary>
        /// w/ card exp date in format YYYY-MM
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="paymentProfile"></param>
        /// <returns></returns>
        public ANetApiResponse UpdatePaymentProfile(string profileId, PaymentProfile paymentProfile)
        {
            if (string.IsNullOrWhiteSpace(profileId))
            {
                throw new ArgumentNullException("profileId", ErrorMessages.ProfileIdNullErrorCode + ErrorMessages.ProfileIdNullErrorMessage);
            }

            if (paymentProfile == null)
            {
                throw new ArgumentNullException("paymentProfile", ErrorMessages.PaymentProfileNullErrorCode + ErrorMessages.PaymentProfileNullErrorMessage);
            }

            ANetApiResponse result = null;
            try
            {
                result = Gateway.UpdatePaymentProfile(profileId, paymentProfile);
                return result;
            }
            catch
            {
                throw;
            }
            finally
            {
                OnUpdatePaymentProfile(result, profileId, paymentProfile);
            }
        }

        public bool UpdateShippingAddress(string profileId, Address address)
        {
            if (address == null)
            {
                throw new ArgumentNullException("address", ErrorMessages.AddressNullErrorCode + ErrorMessages.AddressNullErrorMessage);
            }

            if (string.IsNullOrWhiteSpace(profileId))
            {
                throw new ArgumentNullException("profileId", ErrorMessages.ProfileIdNullErrorCode + ErrorMessages.ProfileIdNullErrorMessage);
            }

            bool result = false;
            try
            {
                result = Gateway.UpdateShippingAddress(profileId, address);
                return result;
            }
            catch
            {
                throw;
            }
            finally
            {
                OnAddressUpdated(result, profileId, address);
            }
        }

        public bool DeletePaymentProfile(string profileId, string paymentProfileId)
        {
            if (string.IsNullOrWhiteSpace(profileId))
                throw new ArgumentNullException("profileId", ErrorMessages.ProfileIdNullErrorCode + ErrorMessages.ProfileIdNullErrorMessage);

            if (paymentProfileId == null)
                throw new ArgumentNullException("paymentProfile", ErrorMessages.PaymentProfileNullErrorCode + ErrorMessages.PaymentProfileNullErrorMessage);
          
            bool result = false;
            try
            {
                result = Gateway.DeletePaymentProfile(profileId, paymentProfileId);
                return result;
            }
            catch
            {
                throw;
            }
            finally
            {
                OnDeletePaymentProfile(result, profileId, paymentProfileId);
            }
        }

        public Customer GetCustomerByEmail(string email, out string error)
        {
            string[] ids = GetCustomerIDs(out error);

            if (ids == null)
                return null;

            foreach (string id in ids)
            {
                Customer cust = GetCustomer(id);

                if (cust.Email == email)
                    return cust; 
            }

            return null;
        }

        #region Protected Methods
        protected virtual void OnCreateCustomer(Customer customer)
        {

        }

        protected virtual void OnAddCard(string card)
        {

        }

        protected virtual void OnAddShippingAddress(Customer customer, Address address, string addAddressResult)
        {

        }

        protected virtual void OnAuthorize(IGatewayResponse response)
        {

        }

        protected virtual void OnAuthorizeAndCapture(IGatewayResponse response)
        {

        }

        protected virtual void OnRefund(IGatewayResponse response)
        {

        }

        protected virtual void OnUpdateCustomer(bool result, Customer customer)
        {

        }

        protected virtual void OnUpdatePaymentProfile(ANetApiResponse result, string profileId, PaymentProfile paymentProfile)
        {

        }

        protected virtual void OnDeletePaymentProfile(bool result, string profileId, string paymentProfileId)
        {

        }

        protected virtual void OnAddressUpdated(bool updated, string profileId, Address address)
        {

        }
        #endregion

        public static string GetAvsResponse(IGatewayResponse response)
        {
            if (response is GatewayResponse)
            {
                return getAvsResponse((GatewayResponse)response);
            }

            return string.Empty;
        }

        private static string getAvsResponse(GatewayResponse response)
        {
            return response.AVSResponse;
        }
    }
}
