using System.Collections;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Diagnostics;

namespace RefactorName.WebApp
{
    //public static class ADHelper
    //{
    //    const string DOMAINLDABNAME = "MCI.GOV";

    //    public static bool Exists(string objectPath = DOMAINLDABNAME)
    //    {
    //        bool found = false;
    //        if (DirectoryEntry.Exists("LDAP://" + objectPath))
    //        {
    //            found = true;
    //        }
    //        return found;
    //    }

    //    public static string FriendlyDomainToLdapDomain(string friendlyDomainName)
    //    {
    //        string ldapPath = null;
    //        try
    //        {
    //            DirectoryContext objContext = new DirectoryContext(
    //                DirectoryContextType.Domain, friendlyDomainName);
    //            Domain objDomain = Domain.GetDomain(objContext);
    //            ldapPath = objDomain.Name;
    //        }
    //        catch (DirectoryServicesCOMException e)
    //        {
    //            ldapPath = e.Message.ToString();
    //        }
    //        return ldapPath;
    //    }
    //}

    public static class ADProperties
    {
        public const String OBJECTCLASS = "objectClass";
        public const String CONTAINERNAME = "cn";
        public const String LASTNAME = "sn";
        public const String COUNTRYNOTATION = "c";
        public const String CITY = "l";
        public const String STATE = "st";
        public const String TITLE = "title";
        public const String POSTALCODE = "postalCode";
        public const String PHYSICALDELIVERYOFFICENAME = "physicalDeliveryOfficeName";
        public const String FIRSTNAME = "givenName";
        public const String MIDDLENAME = "initials";
        public const String DISTINGUISHEDNAME = "distinguishedName";
        public const String INSTANCETYPE = "instanceType";
        public const String WHENCREATED = "whenCreated";
        public const String WHENCHANGED = "whenChanged";
        public const String DISPLAYNAME = "displayName";
        public const String USNCREATED = "uSNCreated";
        public const String MEMBEROF = "memberOf";
        public const String USNCHANGED = "uSNChanged";
        public const String COUNTRY = "co";
        public const String DEPARTMENT = "department";
        public const String COMPANY = "company";
        public const String PROXYADDRESSES = "proxyAddresses";
        public const String STREETADDRESS = "streetAddress";
        public const String DIRECTREPORTS = "directReports";
        public const String NAME = "name";
        public const String OBJECTGUID = "objectGUID";
        public const String USERACCOUNTCONTROL = "userAccountControl";
        public const String BADPWDCOUNT = "badPwdCount";
        public const String CODEPAGE = "codePage";
        public const String COUNTRYCODE = "countryCode";
        public const String BADPASSWORDTIME = "badPasswordTime";
        public const String LASTLOGOFF = "lastLogoff";
        public const String LASTLOGON = "lastLogon";
        public const String PWDLASTSET = "pwdLastSet";
        public const String PRIMARYGROUPID = "primaryGroupID";
        public const String OBJECTSID = "objectSid";
        public const String ADMINCOUNT = "adminCount";
        public const String ACCOUNTEXPIRES = "accountExpires";
        public const String LOGONCOUNT = "logonCount";
        public const String LOGINNAME = "sAMAccountName";
        public const String SAMACCOUNTTYPE = "sAMAccountType";
        public const String SHOWINADDRESSBOOK = "showInAddressBook";
        public const String LEGACYEXCHANGEDN = "legacyExchangeDN";
        public const String USERPRINCIPALNAME = "userPrincipalName";
        public const String EXTENSION = "ipPhone";
        public const String SERVICEPRINCIPALNAME = "servicePrincipalName";
        public const String OBJECTCATEGORY = "objectCategory";
        public const String DSCOREPROPAGATIONDATA = "dSCorePropagationData";
        public const String LASTLOGONTIMESTAMP = "lastLogonTimestamp";
        public const String EMAILADDRESS = "mail";
        public const String MANAGER = "manager";
        public const String MOBILE = "mobile";
        public const String PAGER = "pager";
        public const String FAX = "facsimileTelephoneNumber";
        public const String HOMEPHONE = "homePhone";
        public const String MSEXCHUSERACCOUNTCONTROL = "msExchUserAccountControl";
        public const String MDBUSEDEFAULTS = "mDBUseDefaults";
        public const String MSEXCHMAILBOXSECURITYDESCRIPTOR = "msExchMailboxSecurityDescriptor";
        public const String HOMEMDB = "homeMDB";
        public const String MSEXCHPOLICIESINCLUDED = "msExchPoliciesIncluded";
        public const String HOMEMTA = "homeMTA";
        public const String MSEXCHRECIPIENTTYPEDETAILS = "msExchRecipientTypeDetails";
        public const String MAILNICKNAME = "mailNickname";
        public const String MSEXCHHOMESERVERNAME = "msExchHomeServerName";
        public const String MSEXCHVERSION = "msExchVersion";
        public const String MSEXCHRECIPIENTDISPLAYTYPE = "msExchRecipientDisplayType";
        public const String MSEXCHMAILBOXGUID = "msExchMailboxGuid";
        public const String NTSECURITYDESCRIPTOR = "nTSecurityDescriptor";
        public const String MSRTCSIPUSERENABLED = "msrtcsip-userenabled";
    }

    public class ADUserDetails
    {
        const string DOMAINLDABNAME = "MCI.GOV";

        public string Department { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string LoginName { get; set; }
        public string LoginNameWithDomain { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string HomePhone { get; set; }
        public string Extension { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string EmailAddress { get; set; }
        public string Title { get; set; }
        public string Company { get; set; }
        public string Manager { get; set; }
        public string ManagerName { get; set; }
        public bool Enabled { get; set; }


        private ADUserDetails(dynamic propertyCollection)
        {

            String domainAddress;
            String domainName;
            FirstName = GetProperty(propertyCollection, ADProperties.FIRSTNAME);
            MiddleName = GetProperty(propertyCollection, ADProperties.MIDDLENAME);
            LastName = GetProperty(propertyCollection, ADProperties.LASTNAME);
            LoginName = GetProperty(propertyCollection, ADProperties.LOGINNAME);
            String userPrincipalName = GetProperty(propertyCollection, ADProperties.USERPRINCIPALNAME);
            if (!string.IsNullOrEmpty(userPrincipalName))
                domainAddress = userPrincipalName.Split('@')[1];
            else
                domainAddress = String.Empty;

            if (!string.IsNullOrEmpty(domainAddress))
                domainName = domainAddress.Split('.').First();
            else
            {
                domainName = String.Empty;
            }
            LoginNameWithDomain = String.Format(@"{0}\{1}", domainName, LoginName);
            StreetAddress = GetProperty(propertyCollection, ADProperties.STREETADDRESS);
            City = GetProperty(propertyCollection, ADProperties.CITY);
            State = GetProperty(propertyCollection, ADProperties.STATE);
            PostalCode = GetProperty(propertyCollection, ADProperties.POSTALCODE);
            Country = GetProperty(propertyCollection, ADProperties.COUNTRY);
            Company = GetProperty(propertyCollection, ADProperties.COMPANY);
            Department = GetProperty(propertyCollection, ADProperties.DEPARTMENT);
            HomePhone = GetProperty(propertyCollection, ADProperties.HOMEPHONE);
            Extension = GetProperty(propertyCollection, ADProperties.EXTENSION);
            Mobile = GetProperty(propertyCollection, ADProperties.MOBILE);
            Fax = GetProperty(propertyCollection, ADProperties.FAX);
            EmailAddress = GetProperty(propertyCollection, ADProperties.EMAILADDRESS);
            Title = GetProperty(propertyCollection, ADProperties.TITLE);
            Manager = GetProperty(propertyCollection, ADProperties.MANAGER);
            if (!String.IsNullOrEmpty(Manager))
            {
                String[] managerArray = Manager.Split(',');
                ManagerName = managerArray[0].Replace("CN=", "");
            }
            int userAccountControl = int.Parse(GetProperty(propertyCollection, "userAccountControl"));

            Enabled = !Convert.ToBoolean(userAccountControl & 0x0002);
        }

        private static String GetProperty(dynamic userDetail, String propertyName)
        {
            if (userDetail.Contains(propertyName))
            {
                return userDetail[propertyName][0].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        private static ADUserDetails GetUser(DirectoryEntry userEntry)
        {
            return new ADUserDetails(userEntry.Properties);
        }

        private static ADUserDetails GetUser(PropertyCollection propertyCollection)
        {
            return new ADUserDetails(propertyCollection);
        }

        private static ADUserDetails GetUser(ResultPropertyCollection resultPropertyCollection)
        {
            return new ADUserDetails(resultPropertyCollection);
        }

        public static bool Authenticate(string userName, string password, string domain = DOMAINLDABNAME)
        {
            bool authentic = false;
            try
            {
                DirectoryEntry entry = new DirectoryEntry("LDAP://" + domain, userName, password);
                object nativeObject = entry.NativeObject;
                authentic = true;
            }
            catch (DirectoryServicesCOMException) { }
            return authentic;
        }

        public static ADUserDetails AuthenticateUser(string userName, string password, string domain = DOMAINLDABNAME)
        {
            ADUserDetails result = null;
            try
            {
                DirectoryEntry entry = new DirectoryEntry("LDAP://" + domain, userName, password);
                object nativeObject = entry.NativeObject;
                result = ADUserDetails.GetADUserDetails(userName);
            }
            catch (DirectoryServicesCOMException)
            {
            }
            return result;
        }

        public static ADUserDetails GetADUserDetails(string userName, string domain = DOMAINLDABNAME)
        {
            ADUserDetails result = null;

            DirectoryEntry entry = new DirectoryEntry("LDAP://" + domain);
            DirectorySearcher search = new DirectorySearcher(entry);
            search.Filter = "(&(objectClass=user)(objectCategory=person)(SAMAccountName=" + userName + "))";
            SearchResult resultCol = search.FindOne();
            if (resultCol != null)
            {
                result = ADUserDetails.GetUser(resultCol.Properties);
            }

            return result;
        }

    }
}