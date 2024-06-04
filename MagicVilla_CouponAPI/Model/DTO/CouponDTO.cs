namespace MagicVilla_CouponAPI.Model.DTO
{
    public class CouponDTO
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public int Percent { get; set; }
        public bool IsActive { get; set; }
        public DateTime? Created { get; set; }
    }
}
