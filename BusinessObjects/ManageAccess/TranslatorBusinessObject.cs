﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ManageAccess
{
    public class TranslatorBusinessObject
    {
    }

    public class TranslatorInfo
    {
        public long ItemNumber { get; set; }
        public long TranslatorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public int Status { get; set; }
        public bool IsInUse { get; set; }

    }

    public class PageLoad_TranslatorList
    {
        public List<TranslatorInfo> TranslatorList { get; set; }
        public long TotalCount { get; set; }
    }
}
