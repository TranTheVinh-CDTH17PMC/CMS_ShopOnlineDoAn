﻿using CMS_Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Database.Interfaces
{
    public interface IThanhPham : IGenericRepository<ThanhPham>
    {
        void Delele(int id);
        ThanhPham GetbyId(int Id);
        ThanhPham GetbyName(string name);
    }
}
