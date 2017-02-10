namespace JNUE_ADAPI.Models
{
    /// Properties
    public class Properties
    {
        public static string FacLicense { get { return "78e66a63-337a-4a9a-8959-41c6654dfb56"; } }
        public static string PlusLicense { get { return "e82ae690-a2d5-4d76-8d30-7c6e01e6022e"; } }
        public static string StuLicense { get { return "314c4481-f395-4525-be8b-2ec4bb1e9d91"; } }
        public static string disables { get { return "\"a23b959c-7ce8-4e57-9140-b90eb88a9e97\",\"882e1d05-acd1-4ccb-8708-6ee03664b117\",\"2078e8df-cff6-4290-98cb-5408261a760a\",\"e03c7e47-402c-463c-ab25-949079bedb21\",\"0feaeb32-d00e-4d66-bd5a-43b5b83db82c\",\"0a4983bb-d3e5-4a09-95d8-b2d0127b3df5\",\"9aaf7827-d63c-4b61-89c3-182f06f82e5c\",\"76846ad7-7776-4c40-a281-a386362dd1b9\",\"c68f8d98-5534-41c8-bf36-22fa496fa792\",\"bea4c11e-220a-4e6d-8eb8-8ea15d019f90\",\"9b5de886-f035-4ff2-b3d8-c9127bea3620\""; } }
        /// LDAP 경로
        public static string LDAPPath { get { return "LDAP://211.225.159.44"; } } //://211.225.159.44
        public static string LDAPUser { get { return @"hddemo\administrator"; } }
        public static string LDAPPassword { get { return "P@ssw0rd"; } }

        /// Azure Client Id
        public static string AzClientID { get { return "2582b477-78b9-4bf2-92c1-c48aeccc8511"; } }

        /// Azure Client Secret
        public static string AzClientSecret { get { return "2CMZATzbB3xKW8yuQm/FFQfc5xw9I4FhVnkE2OHWmCI="; } }

        /// Azure Graph Api
        public static string AzGraphApi { get { return "https://graph.windows.net/hddemo.co.kr/"; } }

        /// Azure Sevice Url
        /// TODO: Azure domain url : jnue.kr/ 주소 확인해야 함
        public static string AzDomainUrl { get { return "hddemo.co.kr"; } } //jnue.ac.kr
        
        /// TODO: Azure service url : https://login.microsoftonline.com/jnue.kr/oauth2/token 주소 확인해야 함(안쓰는 것으로 현재 확인)
        public static string AzADAuthority { get { return "https://login.microsoftonline.com/263b037d-11f1-47e8-9d87-d6d61b5cc753/oauth2/token"; } } //login.microsoftonline.com/hddemo

        /// Azure Sevice Url
        public static string LDAPSiteKey { get { return "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"; } }

        /// AD Federation Server Url
        public static string ADFS_URL { get { return @"https://adfs.hddemo.co.kr/adfs/ls/?lc=1042&client-request-id=4d5bbac1-8655-464f-acdc-862c63f8729b&username=%40hddemo.co.kr&wa=wsignin1.0&wtrealm=urn%3afederation%3aMicrosoftOnline&wctx=estsredirect%3d2%26estsrequest%3drQIIAeNisFLOKCkpKLbS1y_ILypJzNHLT0vLTE7VS87P1csvSs9MAbGKhLgEDu6K9g1t83dbc6dNJ_lH0exVjGo4dernJOalZOal6yUWF1RcYGTsYmIxNDAx2sTE6uvs6-R5gmnCWblbTIL-RemeKeHFbqkpqUWJJZn5eY-YeEOLU4v883IqQ_KzU_N2MauYmaaZmFgYGeomJiUn6ZqYmJnpWlgkp-iaGKeamhmlJiYnJpkcYNkQcoFF4BULjwGzFQcHlwCDBIMCww8WxkWsQIcHTr9_ZHZxv3Pj4e63y8TZ6k-x6od6V3mZGjmnGmVb5Jik-7oX5WubuCa65mcaJke4h3kY-BYkZkQ5WlalZbjamlsZTmATmsDGtIvTljgP25ckFqWnltiqGqWlpKYlluaUgIUB0&popupui="; }
        }
    }
}
//https://mso365adfs.jnue.ac.kr/adfs/ls/?lc=1042&client-request-id=20669219-886e-4dc3-8baa-07aed4ed1503&wa=wsignin1.0&wtrealm=urn%3afederation%3aMicrosoftOnline&wctx=estsredirect%3d2%26estsrequest%3drQIIAeNisFLOKCkpKLbS1y_ILypJzNHLT0vLTE7VS87P1csvSs9MAbGKhLgEJCelKeR1HPbtXsW-7spbUeZVjGo4dernJOalZOal6yUWF1RcYGTsYmIxNDAx2sTE6uvs6-R5gmnCWblbTIL-RemeKeHFbqkpqUWJJZn5eY-YeEOLU4v883IqQ_KzU_MmMfPl5Kdn5sUXF6XFp-XklwMFgMYXJCaXxJdkJmenluxiVjFLsQAaYGCha2RmmKhrYmZoqptoZGqpa2RpkGpoZGZilGaWcoBlQ8gFFoEfLIyLWIF-Edr3q1k4v8Z3DS9Pa0wRl9IpVv2stOxwZ6-MsMhKy6AUz6p8x1BfHzNvY7N0i2SDNAtPx-Is_YB0baPUAgNPWxMrw12ctsT53r4ksSg9tcRW1SgtJTUtsTSnBCwMAA2&popupui=