﻿using astute.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial interface IParcel_Master_Service
    {
        Task<int> Insert_Update_Parcel_Master(Parcel_Master parcel_Master);
        Task<int> Delete_Parcel_Master(int parcel_Id);
        Task<List<Dictionary<string, object>>> Get_Parcel_Master(int parcel_Id);
        Task<List<Dictionary<string, object>>> Get_Parcel_Master_By_Cat_Val_Id(int cat_Val_Id);
    }
}
