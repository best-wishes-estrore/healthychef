using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HealthyChef.Common
{
    public class PathBuilder
    {
        public static string UrlPathBuilder(List<string> paths)
        {
            string builder = string.Empty;

            foreach (string path in paths)
            {
                builder = urlPathBuilder(builder, path);
            }

            return builder;
        }

        private static string urlPathBuilder(string path, string directory)
        {
            return pathBuilder(path, directory, "/");
        }

        private static string pathBuilder(string path, string directory, string separator)
        {
            char[] toStrip = separator.ToCharArray();
            directory = directory.Trim(toStrip).Trim();

            if (!path.EndsWith(separator))
            {
                path += separator;
            }

            return path + directory + separator;
        }
    }
}
