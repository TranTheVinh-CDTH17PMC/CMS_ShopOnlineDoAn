﻿using CMS_Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Database.Interfaces
{
    public interface ICTKhuyenMai :IGenericRepository<CTKhuyenmai>
    {
        IQueryable<CTKhuyenmai> GetById(int id);
    }
}