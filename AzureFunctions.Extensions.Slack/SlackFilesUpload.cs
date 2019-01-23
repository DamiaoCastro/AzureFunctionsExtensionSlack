using System;
using System.Collections.Generic;
using System.Text;

namespace AzureFunctions.Extensions.Slack
{
    public class SlackFilesUpload
    {

        public string content { get; set; }
        public string filename { get; set; }
        public string filetype { get; set; }
        public string initial_comment { get; set; }
        public string title { get; set; }
    }
}
