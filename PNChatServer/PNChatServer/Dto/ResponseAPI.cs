﻿
using Newtonsoft.Json;

namespace PNChatServer.Dto
{
    public class ResponseAPI
    {
        public int Status { get; set; }
        public string? Message { get; set; }

        private object _data = string.Empty;
        public object Data
        {
            get
            {
                return JsonConvert.SerializeObject(_data);
            }
            set
            {
                _data = value;
            }
        }
    }
}
