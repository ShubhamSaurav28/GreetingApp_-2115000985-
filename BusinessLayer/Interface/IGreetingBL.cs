﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;

namespace BusinessLayer.Interface
{
    public interface IGreetingBL
    {
        public string GetHelloBL();
        public string PostHelloBL(PostRequestModel postRequestModel);
    }
}
