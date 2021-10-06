using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Portal.JPDS.Domain.Common
{
    public class Helper
    {
        public static string GetRepairRequestTypeBySurveyType(SurveyType surveyType)
        {
            switch (surveyType)
            {
                case SurveyType.PreDelivery: return "Voorschouw";
                case SurveyType.Delivery: return "Opleverpunt";
                case SurveyType.Inspection: return "Inspectie";
                default: return string.Empty;
            }
        }

        public static BuildingManagerRole GetBuildingManagerRoleFromLoginRole(AccountType loginAccountType, List<string> loginRole)
        {
            switch (loginAccountType)
            {
                case AccountType.Buyer:
                    return BuildingManagerRole.gebouw_gebruiker;
                case AccountType.Employee:
                    return BuildingManagerRole.medewerker;
                case AccountType.Relation:
                    if (loginRole.Any(x => x == LoginRoles.PropertyManager))
                        return BuildingManagerRole.vastgoed_beheerder;
                    else
                        return BuildingManagerRole.overige;
                default: return BuildingManagerRole.overige;
            }
        }

        public static string GetLoginRoleName(string roleName)
        {
            foreach (FieldInfo field in typeof(LoginRoles).GetFields())
            {
                var loginRole = field.GetValue(null);
                if (loginRole.Equals(roleName))
                {
                    return field.Name;
                }              
            }       
            return null;
        }
    }
}
