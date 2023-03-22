namespace HStyleApi.Models.DTOs
{
    public class EditMemberDTO
    {
        public string Name { get; set; }
        //public string Email { get; set; }
        //public string Account { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public bool? Gender { get; set; }
        public DateTime? Birthday { get; set; }
    }
}
