using Grpc.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace InventoryManagmentSystem.BLL
{
    public class UserBLL
    {
        public static string RenameImage(HttpPostedFileBase profile, string targetDirectory)
        {
            if (profile != null && profile.ContentLength > 0)
            {
                // Get the original file name
                string originalFileName = Path.GetFileName(profile.FileName);

                // Rename the file as needed (e.g., add a timestamp or a unique identifier)
                string newFileName = GenerateUniqueFileName(originalFileName);

                // Get the file extension
                string fileExtension = Path.GetExtension(originalFileName);

                // Combine the new file name and extension
                string newFileNameWithType = newFileName + fileExtension;

                // Combine the new file name with the target directory
                string newFilePath = Path.Combine(targetDirectory, newFileNameWithType);

                // Save the uploaded file with the new name
                profile.SaveAs(newFilePath);

                // Return the new file name and file type (without the directory path)
                return newFileNameWithType;
            }

            return null; // Return null if no file was provided

        }

        private static string GenerateUniqueFileName(string originalFileName)
        {
            string uniquePart = Guid.NewGuid().ToString();
            string newFileName = Path.GetFileNameWithoutExtension(originalFileName) + "_" + uniquePart;
            return newFileName;
        }
    }
}