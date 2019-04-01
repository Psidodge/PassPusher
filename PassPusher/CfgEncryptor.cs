using System;
using System.Configuration;

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
