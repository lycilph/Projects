using System;
using HgCo.WindowsLive.SkyDrive;

namespace SkydriveTest
{
    class Program
    {
        static void Main()
        {
            // Instantiate SkyDrive API client
            var sky_drive_client = new SkyDriveServiceClient();

            try
            {
                // Log on to a user account
                sky_drive_client.LogOn("lycilph@hotmail.com", "aquiFapoin");

                // Get SkyDrive storage info
                var webDriveInfo = sky_drive_client.GetWebDriveInfo();

                // List folders in SkyDrive's root
                var rootWebFolders = sky_drive_client.ListRootWebFolders();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadLine();
            }
            finally
            {
                // Clean up
            }
        }
    }
}
