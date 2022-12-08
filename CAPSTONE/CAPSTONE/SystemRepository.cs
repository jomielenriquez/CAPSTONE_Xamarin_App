using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace CAPSTONE
{
    public class SystemRepository
    {
        string siteLink = "https://trafficmanagementofficetanauan.somee.com/Systems";
        public string GetFinesLink()
        {
            return siteLink+ "/get_violation";
        }
        public string GetLoginLink(string username, string password)
        {
            return siteLink+"/LoginUser?username="+ username + "&&password="+ password + "&&acnttype=TENFORCER";
        }
        public string GetReport()
        {
            return siteLink + "/getreport";
        }
        public string GetViolation()
        {
            return siteLink + "/get_violation";
        }
        public string getCurrentUser(string acntuid)
        {
            return siteLink + "/getuserfullname?acntuid=" + acntuid;
        }
        public string getinsertcitationlink(
            string fname,
            string address,
            string licneseno,
            string birthdate,
            string dateofapprehension,
            string placeofviolation,
            string violationid,
            string vehicletype,
            string classification,
            string plateno
        )
        {
            return siteLink + "/insert_citation?fname=" + fname
                + "&&address=" + address
                + "&&licneseno=" + licneseno
                + "&&birthdate=" + birthdate
                + "&&dateofapprehension=" + dateofapprehension
                + "&&placeofviolation=" + placeofviolation
                + "&&violationid=" + violationid
                + "&&vehicletype=" + vehicletype
                + "&&classification=" + classification
                + "&&plateno=" + plateno;
        }
        public string prop_get_compound()
        {
            return siteLink + "/prop_get_compound";
        }
        public string proc_update_user_location(string lat, string longi, string acntuid)
        {
            return siteLink + "/proc_update_user_location?uid=" + acntuid
                + "&&lat=" + lat
                + "&&longi=" + longi;
            //https://trafficmanagementofficetanauan.somee.com/Systems/proc_update_user_location?uid=6EA4806B-98D7-47A9-B3B5-955D3BFBBF2B&&lat=testing&&longi=testing
        }
        public async Task<bool> UpdateLocation(string UID)
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();
                if (location != null)
                {
                    string lat = location.Latitude.ToString();
                    string longi = location.Longitude.ToString();

                    using (var client = new HttpClient())
                    {
                        var uri = proc_update_user_location(lat, longi, UID);
                        var result = await client.GetStringAsync(uri);
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {

            }
            return false;
        }
    }
}
