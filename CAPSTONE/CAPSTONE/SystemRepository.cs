using System;
using System.Collections.Generic;
using System.Text;

namespace CAPSTONE
{
    public class SystemRepository
    {
        string siteLink = "http://www.capstone.somee.com/Systems";
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
    }
}
