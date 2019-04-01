using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassPusher
{
    public static class CfgEncryptor
    {
        public static void EncryptConnectionString(bool encrypt)
        {
            try
            {
                // Open the configuration file and retrieve the connectionStrings section.
                ConnectionStringsSection configSection = ConfigurationManager.GetSection("connectionStrings") as ConnectionStringsSection;

                if ((!(configSection.ElementInformation.IsLocked)) &&
                    (!(configSection.SectionInformation.IsLocked)))
                {
                    if (encrypt && !configSection.SectionInformation.IsProtected)
                    {
                        configSection.SectionInformation.ProtectSection
                            ("DataProtectionConfigurationProvider");
                    }

                    configSection.SectionInformation.ForceSave = true;
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
