using HStyleApi.Models.EFModels;

namespace HStyleApi.Models.DTOs
{
    public class AddAddressDTO
    {
        public bool Gender { get; set; }
        public int AddressId { get; set; }
        public int MemberId { get; set; }

        public string DestinationName { get; set; }
        public string Destination { get; set; }
        public string DestinationThe { get; set; }
        public bool Preset { get; set; }
        //public string DestinationCategory { get; set; }
        

    }
    public static class AddAddressDTOxts
    {
        public static AddAddressDTO ToDTO(this Address source)
        {
            return new AddAddressDTO
            {
                AddressId = source.AddressId,
                MemberId = source.MemberId,
                DestinationName = source.DestinationName,
                Destination = source.Destination,
                DestinationThe= source.DestinationThe,
                Preset=source.Preset ?? false,

            };
        }
    }
}
