﻿namespace MagicVilla_CouponAPI.Model
{
    public class Coupon
    {

        public int Id { get; set; }
        public String Name { get; set; }
        public int Percent { get; set; }
        public bool IsActive { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastUpdated { get; set; }


    }
}
